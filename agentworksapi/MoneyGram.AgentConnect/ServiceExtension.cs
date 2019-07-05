using System;
using System.ServiceModel;
using MoneyGram.AgentConnect.DomainModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MoneyGram.AgentConnect.DomainModel.Exceptions;

namespace MoneyGram.AgentConnect
{
    internal static class ServiceExtension
    {
        public static async Task<T> UseService<T>(this IAgentConnectProxyFactory factory,
            Func<Service.AgentConnect, Task<T>> func, Agent agent)
        {
            var success = true;
            var proxy = factory.CreateProxy();

            try
            {
                using (var scope = new FlowingOperationContextScope(((ClientBase<Service.AgentConnect>)proxy).InnerChannel))
                {
                    AddMessageHeader(agent);

                    AddHttpHeader(agent);

                    return await func(proxy).ContinueOnScope(scope);
                }
            }
            catch (FaultException<Service.BusinessError> ex)
            {
                // If TryParse fails, ignore it and use default value.
                var errorCodeInt = 0;
                Int32.TryParse(ex.Detail.errorCode, out errorCodeInt);

                // TODO: Best way to do the data copy to new exception??? Extension?
                var acException = new AgentConnectException(ex.Message, ex)
                {
                    ErrorCode = errorCodeInt,
                    ErrorString = ex.Detail.messageShort,
                    OffendingField = ex.Detail.offendingField,
                    TimeStamp = DateTime.Now,
                    DetailString = ex.Detail.message
                };

                throw acException;
            }
            catch (FaultException<Service.SystemError> ex)
            {
                // If TryParse fails, ignore it and use default value.
                var errorCodeInt = 0;
                Int32.TryParse(ex.Detail.errorCode, out errorCodeInt);

                var subErrorCodeInt = 0;
                Int32.TryParse(ex.Detail.subErrorCode, out subErrorCodeInt);

                // TODO: Best way to do the data copy to new exception??? Extension?
                var acException = new AgentConnectException(ex.Message, ex)
                {
                    ErrorCode = errorCodeInt,
                    ErrorString = ex.Detail.errorString,
                    SubErrorCode = subErrorCodeInt,
                    TimeStamp = ex.Detail.timeStamp,
                    DetailString = ex.Detail.detailString
                };

                throw acException;
            }
            catch (FaultException ex)
            {
                MessageFaultError errObj = null;

                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    using (var reader = msgFault.GetReaderAtDetailContents())
                    {
                        var ser = new DataContractSerializer(typeof(MessageFaultError));
                        errObj = (MessageFaultError)ser.ReadObject(reader, true);
                    }
                }

                throw new Exception($"FaultException - {errObj?.ErrorString}");
            }
            catch (CommunicationException comsEx)
            {
                // Differnt fatal error exception type?
                throw new Exception("CommunicationException - Unable to call AgentConnect:" + comsEx.Message, comsEx);
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception("Exception - Unable to call AgentConnect:" + ex.Message, ex);
            }
            finally
            {
                if (proxy is ICommunicationObject)
                {
                    if (!success)
                    {
                        ((ICommunicationObject)proxy).Abort();
                    }
                    else
                    {
                        ((ICommunicationObject)proxy).Close();
                    }
                }
            }
        }

        public static SimpleAwaiter<TResult> ContinueOnScope<TResult>(this Task<TResult> @this, FlowingOperationContextScope scope)
        {
            return new SimpleAwaiter<TResult>(@this, scope.BeforeAwait, scope.AfterAwait);
        }

        // awaiter
        public class SimpleAwaiter<TResult> :
            System.Runtime.CompilerServices.INotifyCompletion
        {
            readonly Task<TResult> _task;

            readonly Action _beforeAwait;
            readonly Action _afterAwait;

            public SimpleAwaiter(Task<TResult> task, Action beforeAwait, Action afterAwait)
            {
                _task = task;
                _beforeAwait = beforeAwait;
                _afterAwait = afterAwait;
            }

            public SimpleAwaiter<TResult> GetAwaiter()
            {
                return this;
            }

            public bool IsCompleted
            {
                get
                {
                    // don't do anything if the task completed synchronously
                    // (we're on the same thread)
                    if (_task.IsCompleted)
                        return true;
                    _beforeAwait();
                    return false;
                }

            }

            public TResult GetResult()
            {
                return _task.Result;
            }

            // INotifyCompletion
            public void OnCompleted(Action continuation)
            {
                _task.ContinueWith(task =>
                {
                    _afterAwait();
                    continuation();
                },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    SynchronizationContext.Current != null ?
                        TaskScheduler.FromCurrentSynchronizationContext() :
                        TaskScheduler.Current);
            }
        }

        /// <summary>
        /// Add a UsernameToken as a SOAP Header to the outgoing request
        /// </summary>
        /// <param name="agent"></param>
        private static void AddMessageHeader(Agent agent)
        {
            var securityHeader = new RequestSecurityHeader
            {
                UsernameToken = new UsernameToken
                {
                    Username = agent.AgentId + agent.AgentSequence,
                    Password = agent.AgentPassword
                }
            };

            var usernameTokenHeader = MessageHeader.CreateHeader(
                "Security",
                "http://www.moneygram.com/AgentConnect1705",
                securityHeader);
            OperationContext.Current.OutgoingMessageHeaders.Add(usernameTokenHeader);
        }

        /// <summary>
        /// Add MFA OneTimePassword as HTTP header to the outgoing request
        /// </summary>
        /// <param name="agent"></param>
        private static void AddHttpHeader(Agent agent)
        {
            if (string.IsNullOrEmpty(agent.Otp))
            {
                return;
            }

            var requestProp = new HttpRequestMessageProperty();
            if (OperationContext.Current.OutgoingMessageProperties.Count > 0 &&
                OperationContext.Current.OutgoingMessageProperties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                requestProp =
                    (HttpRequestMessageProperty)OperationContext.Current.OutgoingMessageProperties[
                        HttpRequestMessageProperty.Name];
            }

            requestProp.Headers[HttpHeaderNames.Otp] = agent.Otp;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProp;
        }
    }
}