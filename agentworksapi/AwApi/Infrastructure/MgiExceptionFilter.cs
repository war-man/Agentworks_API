using System;
using MoneyGram.Common.Localization;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using MoneyGram.Common;
using AwApi.ViewModels;

namespace AwApi.Infrastructure
{
    public class MgiExceptionFilter : ExceptionFilterAttribute
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext context)
        {
            logger.Error(context.Exception);           
            if (context.Exception is FieldException)
            {
                ProcessFieldException(context);
            }
            else if (context.Exception is PrincipalException)
            {
                ProcessPrincipalException(context);
            }
            else if (context.Exception is TransactionalLimitsException)
            {
                ProcessTransactionalLimitsException(context);
            }
            else if (context.Exception is ArgumentException)
            {
                ProcessArgumentException(context);
            }
            else if(context.Exception is InvalidDeviceException)
            {
                ProcessInvalidDeviceException(context);
            }
            else if(context.Exception is TrainingModeException)
            {
                ProcessTrainingModeException(context);
            }
            else
            {
                ProcessGenericException(context);
            }
        }

        private static void ProcessArgumentException(HttpActionExecutedContext context)
        {
            var exc = (ArgumentException) context.Exception;
            var errorString = $"Argument exception for {exc.Message}";
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.ArgumentException,
                ErrorCode = 400,
                ErrorString = errorString
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.BadRequest,
                resp);
        }

        private static void ProcessTransactionalLimitsException(HttpActionExecutedContext context)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.TransactionalLimitsExceeded,
                ErrorCode = ((TransactionalLimitsException)context.Exception).ErrorCode,
                ErrorString = ((TransactionalLimitsException)context.Exception).ErrorString
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.Forbidden,
                resp);
        }

        private void ProcessInvalidDeviceException(HttpActionExecutedContext context)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.InvalidDevice,
                ErrorCode = ((InvalidDeviceException)context.Exception).ErrorCode,
                ErrorString = ((InvalidDeviceException)context.Exception).ErrorString
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.Forbidden,
                resp);
        }

        private static void ProcessGenericException(HttpActionExecutedContext context)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.ServicesException,
                ErrorString = LocalizationKeys.InternalServerError
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                resp
            );
        }

        private static void ProcessPrincipalException(HttpActionExecutedContext context)
        {
            var exception = (PrincipalException) context.Exception;

            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.PrincipalException,
                ErrorCode = exception.Code,
                ErrorString = context.Exception.Message
            };
            context.Response = context.Request.CreateResponse(
                exception.StatusCode,
                resp);
        }

        private static void ProcessFieldException(HttpActionExecutedContext context)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.AgentConnect,
                ErrorCode = ((FieldException)context.Exception).ErrorCode,
                ErrorString = ((FieldException)context.Exception).ErrorString,
                SubErrorCode = ((FieldException)context.Exception).SubErrorCode,
                OffendingField = ((FieldException)context.Exception).OffendingField,
                TimeStamp = ((FieldException)context.Exception).TimeStamp,
                DetailString = ((FieldException)context.Exception).DetailString,
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.BadRequest,
                resp
            );
        }

        private static void ProcessTrainingModeException(HttpActionExecutedContext context)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.ServicesException,
                ErrorString = ((TrainingModeException)context.Exception).ErrorString
            };
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.BadRequest,
                resp
            );
        }
    }
}