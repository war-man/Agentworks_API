using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/send")]
    public class SendController : ApiController
    {
        private readonly ISendBusiness _sendBusiness;
        private readonly ICommonBusiness _commonBusiness;
        private readonly IConsumerBusiness _consumerBusiness;
        private readonly ILookupBusiness _lookupBusiness;

        public SendController(ISendBusiness sendBusiness, ICommonBusiness commonBusiness, IConsumerBusiness consumerBusiness, ILookupBusiness lookupBusiness)
        {
            sendBusiness.ThrowIfNull(nameof(sendBusiness));
            commonBusiness.ThrowIfNull(nameof(commonBusiness));
            consumerBusiness.ThrowIfNull(nameof(consumerBusiness));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _sendBusiness = sendBusiness;
            _commonBusiness = commonBusiness;
            _consumerBusiness = consumerBusiness;
            _lookupBusiness = lookupBusiness;
        }

        /// <summary>
        /// GetAllFields for Send
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("fields")]
        public IHttpActionResult GetAllFields([FromBody] GetAllFieldsRequest getAllFieldsRequest)
        {
            getAllFieldsRequest.TransactionType = GetAllFieldsTransactionType.Send;

            var respVm = _lookupBusiness.GetAllFields(getAllFieldsRequest);

            return Ok(respVm);
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
            req.MgiSessionType = SessionType.SEND;

            var consumerRespVm = _consumerBusiness.ConsumerHistoryLookup(req);

            return Ok(consumerRespVm);
        }

        [HttpPost]
        [Route("products")]
        public IHttpActionResult FeeLookup(FeeLookupRequest req)
        {
            req.MgiSessionType = SessionType.SEND;
            var resp = _commonBusiness.FeeLookup(req);

            return Ok(resp);
        }

        [HttpPatch]
        [Route("validate")]
        public IHttpActionResult SendValidation(SendValidationRequest sendValidationRequest)
        {
            var sendValidateRespVm = _sendBusiness.SendValidation(sendValidationRequest);
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