using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.Business.BusinessRules;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    /// <summary>
    /// Amend processing endpoints
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/amend")]
    public class AmendController : ApiController
    {
        private readonly IAmendBusiness _amendBusiness;
        private readonly ICommonBusiness _commonBusiness;
        private readonly ILookupBusiness _lookupBusiness;

        /// <summary>
        /// Amend processing controller
        /// </summary>
        /// <param name="amendBusiness"></param>
        /// <param name="commonBusiness"></param>
        /// <param name="lookupBusiness"></param>
        public AmendController(IAmendBusiness amendBusiness, ICommonBusiness commonBusiness, ILookupBusiness lookupBusiness)
        {
            amendBusiness.ThrowIfNull(nameof(amendBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _amendBusiness = amendBusiness;
            _commonBusiness = commonBusiness;
            _lookupBusiness = lookupBusiness;
        }

        /// <summary>
        /// GetAllFields for Amend
        /// </summary>
        /// <remarks>
        /// If CachedVersion in the request is null, the latest GAF will be returned.
        /// </remarks>
        /// <response code="200">AcApiResponse&lt;GetAllFieldsResponse, ApiData&gt;</response> 
        /// <returns></returns>
        [HttpPost]
        [Route("fields")]
        public IHttpActionResult GetAllFields([FromBody] GetAllFieldsRequest getAllFieldsRequest)
        {
            getAllFieldsRequest.TransactionType = GetAllFieldsTransactionType.Amend;

            var respVm = _lookupBusiness.GetAllFields(getAllFieldsRequest);

            return Ok(respVm);
        }

        /// <summary>
        /// Look up a completed send transaction to begin amend.
        /// </summary>
        /// <param name="transactionLookupRequest">TransactionLookupRequest object</param>
        /// <returns>TransactionLookupResponse object</returns>
        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));

            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.Amend;

            var respVm = _commonBusiness.TransactionLookup(transactionLookupRequest);
            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.AMD, respVm?.ResponseData?.Payload?.SendAmounts?.SendAmount ?? 0m);

            return Ok(respVm);
        }

        /// <summary>
        /// Amend validation. Once validation is successful, AmendValidationResponse.Payload.ReadyForCommit will return true.
        /// </summary>
        /// <param name="amendValidationRequest">AmendValidationRequest object</param>
        /// <returns>AmendValidationResponse object</returns>
        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult AmendValidation([FromBody] AmendValidationRequest amendValidationRequest)
        {
            amendValidationRequest.ThrowIfNull(nameof(amendValidationRequest));

            var amendVationationResponse = _amendBusiness.AmendValidation(amendValidationRequest);

            return Ok(amendVationationResponse);
        }

        [HttpPatch]
        [Route("complete")]
        public IHttpActionResult CompleteSession([FromBody] CompleteSessionRequest completeSessionRequest)
        {
            completeSessionRequest.MgiSessionType = SessionType.AMD;

            var commitTransactionRespVm = _commonBusiness.CompleteSession(completeSessionRequest);

            return Ok(commitTransactionRespVm);
        }
    }
}