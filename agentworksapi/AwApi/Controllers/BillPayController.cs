using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Controllers
{
    /// <summary>
    /// Bill Pay api endpoints
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/bill-pay")]
    public class BillPayController : ApiController
    {
        private readonly IBillPayBusiness _billPayBusiness;
        private readonly ICommonBusiness _commonBusiness;
        private readonly IConsumerBusiness _consumerBusiness;
        private readonly ILookupBusiness _lookupBusiness;

        /// <summary>
        /// Constructor, used by IoC to inject the Bill Pay business object
        /// </summary>
        /// <param name="billPayBusiness">An instance of an IBillPayBusiness implementation</param>
        /// <param name="commonBusiness">An instance of an ICommonBusiness implementation</param>
        /// <param name="consumerBusiness">An instance of IConsumerBusiness implementation</param>
        public BillPayController(IBillPayBusiness billPayBusiness, ICommonBusiness commonBusiness, IConsumerBusiness consumerBusiness, ILookupBusiness lookupBusiness)
        {
            billPayBusiness.ThrowIfNull(nameof(billPayBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            consumerBusiness.ThrowIfNull(nameof(consumerBusiness));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _billPayBusiness = billPayBusiness;
            _commonBusiness = commonBusiness;
            _consumerBusiness = consumerBusiness;
            _lookupBusiness = lookupBusiness;
        }

        /// <summary>
        /// GetAllFields for Bill Pay
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("fields")]
        public IHttpActionResult GetAllFields([FromBody] GetAllFieldsRequest getAllFieldsRequest)
        {
            getAllFieldsRequest.TransactionType = GetAllFieldsTransactionType.BillPayment;

            var respVm = _lookupBusiness.GetAllFields(getAllFieldsRequest);

            return Ok(respVm);
        }

        /// <summary>
        /// Get list of billers by Biller Name, code, or keyword
        /// </summary>
        /// <param name="req">An instance of BillerSearchRequest, Biller search view model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("biller")]
        public IHttpActionResult BillerSearch(BillerSearchRequest req)
        {
            var billerList = _billPayBusiness.BillerSearch(req);
            return Ok(billerList);
        }

        /// <summary>
        /// Get consumer history for a specific transaction type
        /// </summary>
        /// <param name="req">ConsumerHistoryLookupRequest object</param>
        /// <returns>ConsumerHistoryLookupResponse</returns>
        [HttpPost]
        [Route("consumer")]
        public IHttpActionResult ConsumerHistoryLookup(ConsumerHistoryLookupRequest req)
        {
            req.MgiSessionType = SessionType.BP;

            var consumerRespVm = _consumerBusiness.ConsumerHistoryLookup(req);

            return Ok(consumerRespVm);
        }

        /// <summary>
        /// Get list of products for Bill Pay receiver
        /// </summary>
        /// <param name="req">Bill Pay Product query parameters</param>
        /// <returns>List of Products for given search parameters</returns>
        [HttpPost]
        [Route("products")]
        public IHttpActionResult FeeLookup(FeeLookupRequest req)
        {
            req.MgiSessionType = SessionType.BP;
            var resp = _commonBusiness.FeeLookup(req);
            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.BP, resp?.ResponseData?.Payload?.FeeInfos?[0].SendAmounts?.SendAmount ?? 0m);
            return Ok(resp);
        }

        /// <summary>
        /// Validate Bill Pay transaction
        /// </summary>
        /// <param name="req">Bill Pay Validate request parameters</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult BPValidation(BPValidationRequest req)
        {
            var resp = _billPayBusiness.BPValidation(req);
        
            return Ok(resp);
        }

        /// <summary>
        /// Complete Bill Pay transaction
        /// </summary>
        /// <param name="req">Complete Bill Pay input parameters</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession(CompleteSessionRequest req)
        {
            req.MgiSessionType = SessionType.BP;
            var commitTransactionRespVm = _commonBusiness.CompleteSession(req);

            return Ok(commitTransactionRespVm);
        }

        //        /// <summary>
        //        /// Get list of fee quotes for pre-paid cards by card number and amount
        //        /// </summary>
        //        /// <param name="prepayCardLoadProductsReqVm">Prepaid card load products request view model</param>
        //        /// <returns></returns>
        //        [HttpPost]
        //        [Route("prepaid-card/products")]
        //        public IHttpActionResult PrepaidCardProducts(PrepayCardLoadProductsReqVm prepayCardLoadProductsReqVm)
        //        {
        //            var billerList = _business.FindPrePaidCardProducts(prepayCardLoadProductsReqVm);
        //            return Ok(billerList);
        //        }

    }
}