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
    [RoutePrefix("api/receive-reversal")]
    public class ReceiveReversalController : ApiController
    {
        private readonly IReceiveReversalBusiness _receiveReversalBusiness;
        private readonly ICommonBusiness _commonBusiness;

        /// <summary>
        /// Controller for Receive endpoints
        /// </summary>
        /// <param name="receiveReversalBusiness"></param>
        /// <param name="commonBusiness"></param>
        public ReceiveReversalController(IReceiveReversalBusiness receiveReversalBusiness, ICommonBusiness commonBusiness)
        {
            receiveReversalBusiness.ThrowIfNull(nameof(receiveReversalBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));

            _receiveReversalBusiness = receiveReversalBusiness;
            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {            
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));
            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.ReceiveReversal;

            var transactionLookupResponse = _commonBusiness.TransactionLookup(transactionLookupRequest);

            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.RREV, transactionLookupResponse?.ResponseData?.Payload?.ReceiveAmounts?.ReceiveAmount ?? 0m);

            return Ok(transactionLookupResponse);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult ReceiveReversalValidation([FromBody] ReceiveReversalValidationRequest req)
        {
            var recvValidateRespVm = _receiveReversalBusiness.ReceiveReversalValidation(req);

            return Ok(recvValidateRespVm);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.RREV;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}