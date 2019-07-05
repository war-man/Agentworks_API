using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.Business.BusinessRules;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/status")]
    public class StatusController : ApiController
    {
        private readonly ICommonBusiness _commonBusiness;

        public StatusController(ICommonBusiness commonBusiness)
        {
            commonBusiness.ThrowIfNull(nameof(commonBusiness));

            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.Status;

            var detailLookupRespVm = _commonBusiness.TransactionLookup(transactionLookupRequest);

            return Ok(detailLookupRespVm);
        }
    }
}