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
    [RoutePrefix("api/staged-send")]
    public class StagedSendController : ApiController
    {
        private readonly IStagedSendBusiness _stagedSendBusiness;
        private readonly ICommonBusiness _commonBusiness;

        public StagedSendController(IStagedSendBusiness stagedSendBusiness, ICommonBusiness commonBusiness)
        {
            stagedSendBusiness.ThrowIfNull(nameof(stagedSendBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            _stagedSendBusiness = stagedSendBusiness;
            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route("list")]
        public IHttpActionResult SearchStagedTransactions([FromBody] SearchStagedTransactionsRequest req)
        {
            // Create request view model
            req.MgiSessionType = SessionType.SEND;

            var sendStagedListRespVm = _commonBusiness.SearchStagedTransactions(req);

            return Ok(sendStagedListRespVm);
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));
            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.SendCompletion;
            var transactionLookupResponse = _commonBusiness.TransactionLookup(transactionLookupRequest);
            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.SEND, transactionLookupResponse?.ResponseData?.Payload?.SendAmounts?.SendAmount ?? 0m);
            return Ok(transactionLookupResponse);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult SendValidation(SendValidationRequest sendValidationRequest)
        {
            var sendValidateRespVm = _stagedSendBusiness.SendValidation(sendValidationRequest);
            return Ok(sendValidateRespVm);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.SEND;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}