using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace AwApi.Infrastructure
{
    public class RequestTimingMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public RequestTimingMiddleware(OwinMiddleware next)
            : base(next)
        {
            _next = next;
        }

        public override async Task Invoke(IOwinContext owinContext)
        {
            var stopwatch = Stopwatch.StartNew();

            logger.Info("Begin Request {0}", owinContext.Request.Path);
            await _next.Invoke(owinContext);

            stopwatch.Stop();
            logger.Info($"End Request - HTTP Status: {owinContext.Response.StatusCode} Total Elapsed Time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}