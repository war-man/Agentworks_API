using System;
using System.ServiceModel;
using MoneyGram.PartnerService.DomainModel.Exceptions;
using MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService
{
    internal static class ServiceExtension
    {
        public static T UseService<T>(
            this IPartnerServiceProxyFactory factory,
            Func<WebPOEPartnerServicePortType, T> func)
        {
            bool success = false;
            var proxy = factory.CreateProxy();

            try
            {
                T result = func(proxy);

                // logging???
                // new Request and Response service base classes are serializable.
                // but paramenters aren't visible here. Intercept???
                // ?? Exception Handleing ?? wrap and throw here?
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
                var partnerServiceException = new PartnerServiceException(ex.Message, ex)
                {
                    ErrorCode = Convert.ToString(ErrorCodeInt),
                    ErrorMessage = ex.Detail.errorMessage,
                    ErrorStackTrace = ex.Detail.errorStackTrace,
                    TimeStamp = DateTime.Now,
                };

                throw partnerServiceException;
            }
            catch (CommunicationException comsEx)
            {
                // Differnt fatal error exception type?
                throw new Exception("CommunicationException - Unable to call PartnerService:" + comsEx.Message, comsEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception - Unable to call PartnerService:" + ex.Message, ex);
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