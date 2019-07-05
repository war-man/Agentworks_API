using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
using AwApi.Business.BusinessRules;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/edit-transfer")]
    public class EditTransferController : ApiController
    {
        private readonly ICommonBusiness _commonBusiness;

        public EditTransferController(ICommonBusiness commonBusiness)
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