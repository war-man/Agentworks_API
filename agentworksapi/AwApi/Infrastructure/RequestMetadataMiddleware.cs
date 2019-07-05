using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using NLog;

namespace AwApi.Infrastructure
{
    public class RequestMetadataMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public RequestMetadataMiddleware(OwinMiddleware next)
            : base(next)
        {
            _next = next;
        }

        public override async Task Invoke(IOwinContext owinContext)
        {
            var headers = owinContext.Request.Headers.GetValues("User-Agent");

            var userAgent = string.Join(" ", headers);

            var timingHeader = owinContext.Request.Headers.Get("x-mgitimestamp");

            var browserLanguage = owinContext.Request.Headers.Get("Accept-Language");

            MappedDiagnosticsContext.Set("agenttimezone", timingHeader);

            MappedDiagnosticsContext.Set("browserlanguage", browserLanguage);

            MappedDiagnosticsContext.Set("useragent", userAgent);

            MappedDiagnosticsContext.Set("transactionId", Guid.NewGuid().ToString());

            await _next.Invoke(owinContext);
        }
    }
}