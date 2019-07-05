using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
using AwApi.Business.BusinessRules;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/receive")]
    public class ReceiveController : ApiController
    {
        private readonly IReceiveBusiness _receiveBusiness;
        private readonly ICommonBusiness _commonBusiness;
        private readonly ILookupBusiness _lookupBusiness;

        /// <summary>
        /// Controller for Receive endpoints
        /// </summary>
        /// <param name="receiveBusiness"></param>
        /// <param name="commonBusiness"></param>
        /// <param name="lookupBusiness"></param>
        public ReceiveController(IReceiveBusiness receiveBusiness, ICommonBusiness commonBusiness, ILookupBusiness lookupBusiness)
        {
            receiveBusiness.ThrowIfNull(nameof(receiveBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _receiveBusiness = receiveBusiness;
            _commonBusiness = commonBusiness;
            _lookupBusiness = lookupBusiness;
        }

        /// <summary>
        /// GetAllFields for Receive
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("fields")]
        public IHttpActionResult GetAllFields([FromBody] GetAllFieldsRequest getAllFieldsRequest)
        {
            getAllFieldsRequest.TransactionType = GetAllFieldsTransactionType.Receive;

            var respVm = _lookupBusiness.GetAllFields(getAllFieldsRequest);

            return Ok(respVm);
        }

        [HttpPost]
        [Route()]
        public IHttpActionResult TransactionLookup([FromBody] TransactionLookupRequest transactionLookupRequest)
        {
            transactionLookupRequest.ThrowIfNull(nameof(transactionLookupRequest));

            transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.Receive;

            var transactionLookupResponse = _commonBusiness.TransactionLookup(transactionLookupRequest);

            // If transactional limit is exceeded, it will throw an exception
            _commonBusiness.ExceedsTransactionalLimit(SessionType.RCV, transactionLookupResponse?.ResponseData?.Payload?.ReceiveAmounts?.ReceiveAmount ?? 0m);

            return Ok(transactionLookupResponse);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult ReceiveValidation([FromBody] ReceiveValidationRequest req)
        {
            var recvValidateRespVm = _receiveBusiness.ReceiveValidation(req);

            return Ok(recvValidateRespVm);
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