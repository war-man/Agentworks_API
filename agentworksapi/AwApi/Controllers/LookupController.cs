using System.Net;
using System.Net.Http;
using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels.Lookup;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/lookup")]
    public class LookupController : ApiController
    {
        private readonly ILookupBusiness _lookupBusiness;

        public LookupController(ILookupBusiness lookupBusiness)
        {
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _lookupBusiness = lookupBusiness;
        }

        [HttpPost]
        [Route("currencies")]
        public IHttpActionResult GetCurrencyInfo([FromBody] GetCurrencyInfoRequest req)
        {
            req = req ?? new GetCurrencyInfoRequest();

            var currenciesRespVm = _lookupBusiness.GetCurrencyInfo(req);

            return Ok(currenciesRespVm);
        }

        [HttpPost]
        [Route("countries")]
        public IHttpActionResult GetCountryInfo([FromBody] GetCountryInfoRequest req)
        {
            req = req ?? new GetCountryInfoRequest();

            var countriesRespVm = _lookupBusiness.GetCountryInfo(req);

            return Ok(countriesRespVm);
        }

        [HttpPost]
        [Route("subdivisions")]
        public IHttpActionResult Subdivisions([FromBody] GetCountrySubdivisionRequest reqVm)
        {
            var resp = _lookupBusiness.GetCountrySubdivision(reqVm);

            return Ok(resp);
        }

        [HttpGet]
        [Route("industries")]
        public IHttpActionResult Industry([FromBody] IndustryRequest industryRequest)
        {
            industryRequest = industryRequest ?? new IndustryRequest();

            var industriesRespVm = _lookupBusiness.Industry(industryRequest);

            return Ok(industriesRespVm);
        }

        [HttpPost]
        [Route("all")]
        public HttpResponseMessage All([FromBody] AllLookupRequest req)
        {
            var allLookupResponse = _lookupBusiness.All(req);

            var statusCode = allLookupResponse.ApiErrors == null ? HttpStatusCode.OK : HttpStatusCode.PartialContent;

            var response = Request.CreateResponse(statusCode, allLookupResponse);

            return response;
        }
    }
}