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
    [RoutePrefix("api/staged-receive")]
    public class StagedReceiveController : ApiController
    {
        private readonly IReceiveBusiness _receiveBusiness;
        private readonly ICommonBusiness _commonBusiness;

        /// <summary>
        /// Controller for Staged Receive endpoints
        /// </summary>
        /// <param name="commonBusiness"></param>
        public StagedReceiveController(IReceiveBusiness receiveBusiness, ICommonBusiness commonBusiness)
        {
            receiveBusiness.ThrowIfNull(nameof(receiveBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            _receiveBusiness = receiveBusiness;
            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route("list")]
        public IHttpActionResult SearchStagedTransactions([FromBody] SearchStagedTransactionsRequest req)
        {
            // Create request view model
            req.MgiSessionType = SessionType.RCV;

            var sendStagedListRespVm = _commonBusiness.SearchStagedTransactions(req);

            return Ok(sendStagedListRespVm);
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));
            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.ReceiveCompletion;

            var transactionLookupResponse = _commonBusiness.TransactionLookup(transactionLookupRequest);
            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.RCV, transactionLookupResponse?.ResponseData?.Payload?.ReceiveAmounts?.ReceiveAmount ?? 0m);
            return Ok(transactionLookupResponse);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult ReceiveValidation(ReceiveValidationRequest receiveValidationRequest)
        {
            var receiveValidateRespVm = _receiveBusiness.ReceiveValidation(receiveValidationRequest);
            return Ok(receiveValidateRespVm);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.RCV;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}