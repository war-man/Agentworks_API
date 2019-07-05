using System;
using System.Web.Http.Filters;
using MoneyGram.Common.Extensions;

namespace AwApi.Infrastructure
{
    public class DiagnosticsFilterAttribute : ActionFilterAttribute
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string message = CreateLogMessage(actionContext);

            logger.Debug(message);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
        }

        private string CreateLogMessage(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.ThrowIfNull(nameof(actionContext));

            string message =
                "Controller: " + actionContext.ControllerContext.ControllerDescriptor.ControllerType + Environment.NewLine +
                "RequestUri: " + actionContext.Request.RequestUri + Environment.NewLine +
                "HttpVerb: " + actionContext.Request.Method + Environment.NewLine +
                "Method: " + actionContext.ActionDescriptor.ActionName + Environment.NewLine +
                "Headers: " + Environment.NewLine;

            // Is there a library function to do this? HttpHeaders.ToString() does not.
            foreach (var header in actionContext.Request.Headers)
            {
                message += "    " + header.Key + ": " + string.Join(",", header.Value) + Environment.NewLine;
            }

            return message;
        }
    }
}