using System;
using System.ServiceModel;
using MoneyGram.DLS.DomainModel.Exceptions;
using MoneyGram.DLS.Service;

namespace MoneyGram.DLS
{
    internal static class ServiceExtension
    {
        public static T UseService<T>(
            this IDLSProxyFactory factory,
            Func<DataLookupServicePortType, T> func)
        {
            bool success = false;
            var proxy = factory.CreateProxy();

            try
            {
                T result = func(proxy);

                // logging???
                // new Request and Response service base classes are serializable.
                // but paramenters aren't visible here. Intercept???
                // ?? Exception Handling ?? wrap and throw here?
                //ServiceUtilities.LogAgentConnect("moneyGramFrequentCustomerPhoneLookup", request, response);
                if (proxy is ICommunicationObject)
                {
                    ((ICommunicationObject)proxy).Close();
                }

                success = true;
                return result;
            }
            catch (FaultException<ServiceError> ex)
            {
                Console.WriteLine("Got ERROR FAULT - " + ex.Message);

                // If TryParse fails, ignore it and use default value.
                int ErrorCodeInt = 0;
                Int32.TryParse(ex.Detail.errorCode, out ErrorCodeInt);

                // TODO: Best way to do the data copy to new exception??? Extension?
                var dlsException = new DLSException(ex.Message, ex)
                {
                    ErrorCode = Convert.ToString(ErrorCodeInt),
                    ErrorMessage = ex.Detail.errorMessage,
                    ErrorStackTrace = ex.Detail.errorStackTrace,
                    TimeStamp = DateTime.Now,

                };

                throw dlsException;
            }
            catch (CommunicationException comsEx)
            {
                // Differnt fatal error exception type?
                throw new Exception("CommunicationException - Unable to call DLS:" + comsEx.Message, comsEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception - Unable to call DLS:" + ex.Message, ex);
            }

            finally
            {
                if (!success)
                {
                    if (proxy is ICommunicationObject)
                    {
                        ((ICommunicationObject)proxy).Abort();
                    }
                }
            }
        }

    }
}