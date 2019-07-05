using System;
using AwApi.ViewModels.Event;
using MoneyGram.Common.Json;

namespace AwApi.Integration.Event
{
    public class EventIntegration : IEventIntegration
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public EventResponse Process(EventRequest eventRequest)
        {
            EventResponse resp = null;

            switch(eventRequest.ActionType)
            {
                case EventActionType.Invalid:
                    throw new ArgumentException("actionType");
                case EventActionType.Log:
                    return Log(eventRequest);
                case EventActionType.Payload:
                    return LogPayload(eventRequest);
            }

            return resp;
        }

        private static EventResponse Log(EventRequest eventRequest)
        {
            var resp = new EventResponse();

            var userName = AuthIntegration.HasClaims() ? AuthIntegration.GetOperator().UserName : null;

            if (string.IsNullOrEmpty(userName))
            {
                return resp;
            }

            //[DATE TIME] [SEVERITY] [APPLICATION] [METHOD] DETAIL
            try
            {
                //LogManager.SetLogicalContextProperty(LogProperties.ApplicationName, $"{eventRequest.Source}");
                var jsonString = JsonProcessor.SerializeObject(eventRequest.Payload);
                var strLog = $"Msg='{jsonString}'";

                logger.Info(strLog);
            }
            catch
            {
                throw new Exception("Event could not be logged");
            }

            return resp;
        }

        private static EventResponse LogPayload(EventRequest eventRequest)
        {
            var resp = new EventResponse();

            var userName = AuthIntegration.HasClaims() ? AuthIntegration.GetOperator().UserName : null;

            if (string.IsNullOrEmpty(userName))
            {
                return resp;
            }

            //[DATE TIME] [SEVERITY] [APPLICATION] [METHOD] DETAIL
            try
            {
                //LogManager.SetLogicalContextProperty(LogProperties.ApplicationName, $"{eventRequest.Source}");

                foreach(var payload in eventRequest.Payload)
                {
                    logger.Info(Environment.NewLine + payload.Data);
                }
            }
            catch
            {
                throw new Exception("Event could not be logged");
            }

            return resp;
        }
    }
}