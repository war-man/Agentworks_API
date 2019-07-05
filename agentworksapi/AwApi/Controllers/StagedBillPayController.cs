using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.Business.BusinessRules;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    /// <summary>
    /// Staged Bill Pay api endpoints
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/staged-bill-pay")]
    public class StagedBillPayController : ApiController
    {
        private readonly IStagedBillPayBusiness _business;
        private readonly ICommonBusiness _commonBusiness;

        /// <summary>
        /// Constructor, used by IoC to inject the Bill Pay business object
        /// </summary>
        /// <param name="business">An instance of an IBillPayBusiness implementation</param>
        public StagedBillPayController(IStagedBillPayBusiness business, ICommonBusiness commonBusiness)
        {
            business.ThrowIfNull(nameof(business));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));

            _business = business;
            _commonBusiness = commonBusiness;
        }

        [HttpPost]
        [Route("list")]
        public IHttpActionResult SearchStagedTransactions([FromBody] SearchStagedTransactionsRequest req)
        {
            // Create request view model
            req.MgiSessionType = SessionType.BP;

            var sendStagedListRespVm = _commonBusiness.SearchStagedTransactions(req);

            return Ok(sendStagedListRespVm);
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            // Create request view model
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));
            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.BillPayCompletion;

            var transactionLookupResponse = _commonBusiness.TransactionLookup(transactionLookupRequest);
            _commonBusiness.ExceedsTransactionalLimit(SessionType.BP, transactionLookupResponse?.ResponseData?.Payload?.SendAmounts?.SendAmount ?? 0m);
            return Ok(transactionLookupResponse);
        }

        /// <summary>
        /// Get list of products for Bill Pay receiver
        /// </summary>
        /// <param name="req">Staged Bill Pay Product query parameters</param>
        /// <returns>List of Products for given search parameters</returns>
        [HttpPost]
        [Route("products")]
        public IHttpActionResult FeeLookup([FromBody] FeeLookupRequest req)
        {
            req.MgiSessionType = SessionType.BP;
            req.ProductVariant = req.ProductVariant ?? ProductVariantType.EP;
            var resp = _commonBusiness.FeeLookup(req);

            return Ok(resp);
        }

        /// <summary>
        /// Validate Bill Pay transaction
        /// </summary>
        /// <param name="req">Staged Bill Pay Validate request parameters</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult BPValidation([FromBody] BPValidationRequest req)
        {
            var resp = _business.BPValidation(req);

            return Ok(resp);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.BP;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}