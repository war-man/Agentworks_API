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
    [RoutePrefix("api/send-reversal")]
    public class SendReversalController : ApiController
    {
        private readonly ISendReversalBusiness _sendReversalBusiness;
        private readonly ICommonBusiness _commonBusiness;

        public SendReversalController(ISendReversalBusiness sendReversalBusiness, ICommonBusiness commonBusiness)
        {
            sendReversalBusiness.ThrowIfNull(nameof(sendReversalBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));

            _sendReversalBusiness = sendReversalBusiness;
            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));

            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.SendReversal;

            var detailLookupRespVm = _commonBusiness.TransactionLookup(transactionLookupRequest);

            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.SREV, detailLookupRespVm?.ResponseData?.Payload?.SendAmounts?.SendAmount ?? 0m);

            return Ok(detailLookupRespVm);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult SendReversalValidation([FromBody] SendReversalValidationRequest sendReversalValidationRequest)
        {
            var sendReversalRespVm = _sendReversalBusiness.SendReversalValidation(sendReversalValidationRequest);

            return Ok(sendReversalRespVm);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.SREV;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}