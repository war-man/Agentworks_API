using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MoneyGram.Common.Diagnostics.ServiceModel
{
    /// <summary>
    ///     WCF Behavior that logs the raw SOAP message being sent and recieved by the client.
    /// </summary>
    public class MessageLogger : IClientMessageInspector, IEndpointBehavior
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IList<MessageLogFilter> MessageLogFilters { get; set; }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var action = string.Empty;
            if(correlationState != null)
            {
                action = correlationState.ToString();
            }

            logger.Info("Received from {0}", action);
            logger.Debug("\n{0}", MessageToString(ref reply));
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var action = string.Empty;
            if(request?.Headers != null)
            {
                action = request.Headers.Action;
            }

            logger.Info("Sending to {0}", action);
            logger.Debug("\n{0}", MessageToString(ref request));

            return action;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
        
        private string MessageToString(ref Message message)
        {
            if(message == null || message.IsEmpty)
            {
                return string.Empty;
            }

            var serializedMessage = message.ToString();

            if (this.MessageLogFilters != null && this.MessageLogFilters.Count > 0)
            {
                foreach(var filter in this.MessageLogFilters)
                {
                    serializedMessage = filter.RegexMatch.Replace(serializedMessage, filter.RegexReplacement);
                }
            }

            return serializedMessage;
        }
    }
}