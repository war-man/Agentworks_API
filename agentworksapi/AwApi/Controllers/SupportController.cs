using System.Net;
using System.Net.Http;
using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using AwApi.ViewModels.Cache;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Support, MgiAwRole.TellerManagerAdmin, MgiAwRole.Dt4Support)]
    [RoutePrefix("api/support")]
    public class SupportController : ApiController
    {
        private readonly ISupportBusiness _supportBusiness;
        private string AwSuccess => "AW_SUCCESS";
        private string AwFailure => "AW_FAILURE";

        public SupportController(ISupportBusiness supportBusiness)
        {
            supportBusiness.ThrowIfNull(nameof(supportBusiness));
            _supportBusiness = supportBusiness;
        }

        [HttpGet]
        [Route("health")]
        public IHttpActionResult Health()
        {
            return Ok(new {status = AwSuccess });
        }

        [AwAuthorization(MgiAwRole.Support, MgiAwRole.TellerManagerAdmin)]
        [HttpGet]
        [Route("health/deep")]
        public IHttpActionResult DeepHealth()
        {
            var response = _supportBusiness.DeepHealthCheck();
            var status = response.ApiErrors == null ? AwSuccess : AwFailure;
            return Ok(new { status, response });
        }

        [AwAuthorization(MgiAwRole.Support, MgiAwRole.TellerManagerAdmin)]
        [HttpDelete]
        [Route("cache")]
        public HttpResponseMessage DeleteCache()
        {
            var response = new HttpResponseMessage();

            _supportBusiness.DeleteCache();

            response.StatusCode = HttpStatusCode.NoContent;

            return response;
        }

        [AwAuthorization(MgiAwRole.Support, MgiAwRole.TellerManagerAdmin)]
        [HttpDelete]
        [Route("cache/agent")]
        [AwAuthorization(MgiAwRole.Support)]
        public HttpResponseMessage DeleteAgentCache(AgentCacheRequest request)
        {
            var response = new HttpResponseMessage();

            _supportBusiness.DeleteAgentCache(request);

            response.StatusCode = HttpStatusCode.NoContent;

            return response;
        }
    }
}