using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwApi.ViewModels;
using Microsoft.Owin;
using MoneyGram.Common;
using MoneyGram.Common.Json;
using MoneyGram.Common.Localization;

namespace AwApi.Auth
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public GlobalExceptionMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (PrincipalException ex)
            {
                logger.Error(ex);
                ProcessPrincipalException(context, ex);
            }
        }

        private void ProcessPrincipalException(IOwinContext context, PrincipalException ex)
        {
            var resp = new MgiExceptionVm
            {
                ExceptionType = MgiExceptionType.PrincipalException,
                ErrorCode = ex.Code,
                ErrorString = GetTranslation(context, ex.ErrorMessage)
            };
            // * IMPORTANT * InternalDetails property is for server side logging only, it may contain sensitive information and should
            // NEVER be mapped to the vm.
            if (!string.IsNullOrWhiteSpace(ex.InternalDetails))
            {
                logger.Error(ex.InternalDetails);
            }
            PopulateResponse(context, resp, ex.StatusCode);
        }

        private void PopulateResponse(IOwinContext context, MgiExceptionVm resp,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var jsonObject = JsonProcessor.SerializeObject(resp, camelCase: true);
            var vmByteArray = Encoding.ASCII.GetBytes(jsonObject);
            context.Response.Body.Write(vmByteArray, 0, vmByteArray.Length);
            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
        }


        /// <summary>
        /// Retrieves a localized string, attempting to use the locale of the request object and defaulting to en-US
        /// </summary>
        /// <param name="owinContext">Current Owin context</param>
        /// <param name="key">Localized string key</param>
        /// <returns>Localized string</returns>
        private string GetTranslation(IOwinContext owinContext, string key)
        {
            if (!key.IsLocalizedStringKey())
            {
                return key;
            }

            var acceptLanguageHeaders = owinContext.Request.Headers.Get(HttpHeaderNames.AcceptLanguage);
            var requestLanguages = acceptLanguageHeaders.RequestLanguages();
            var translation = LocalizationProvider.GetTranslation(LocalizationProvider.DefaultApplication, requestLanguages, key,
                "System unavailable. Contact MoneyGram for support.");

            return translation;
        }
    }
}