using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    /// <summary>
    /// Consumer-related endpoints
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/consumer")]
    public class ConsumerController : ApiController
    {
        private readonly IConsumerBusiness _business;

        public ConsumerController(IConsumerBusiness business)
        {
            business.ThrowIfNull(nameof(business));

            _business = business;
        }

        /// <summary>
        /// Get consumer history for a specific transaction type
        /// </summary>
        /// <param name="req">ConsumerHistoryLookupRequest object</param>
        /// <returns>ConsumerHistoryLookupResponse</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult ConsumerHistoryLookup(ConsumerHistoryLookupRequest req)
        {
            var consumerRespVm = _business.ConsumerHistoryLookup(req);

            return Ok(consumerRespVm);
        }

        /// <summary>
        /// Get receiver profile
        /// </summary>
        /// <param name="req">GetProfileReceiverRequest object</param>
        /// <returns>GetProfileReceiverResponse</returns>
        [HttpPost]
        [Route("receiver")]
        public IHttpActionResult GetProfileReceiver(GetProfileReceiverRequest req)
        {
            var getProfileReceiver = _business.GetProfileReceiver(req);

            return Ok(getProfileReceiver);
        }

        /// <summary>
        /// Get sender profile
        /// </summary>
        /// <param name="req">GetProfileSenderRequest object</param>
        /// <returns>GetProfileSenderResponse</returns>
        [HttpPost]
        [Route("sender")]
        public IHttpActionResult GetProfileSender(GetProfileSenderRequest req)
        {
            var getProfileSender = _business.GetProfileSender(req);

            return Ok(getProfileSender);
        }

        /// <summary>
        /// Create or update receiver profile
        /// </summary>
        /// <param name="req">CreateOrUpdateProfileReceiverRequest object</param>
        /// <returns>CreateOrUpdateProfileReceiverResponse</returns>
        [HttpPatch]
        [Route("receiver")]
        public IHttpActionResult CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverRequest req)
        {
            var createOrUpdateProfileReceiver = _business.CreateOrUpdateProfileReceiver(req);

            return Ok(createOrUpdateProfileReceiver);
        }

        /// <summary>
        /// Create or update sender profile
        /// </summary>
        /// <param name="req">CreateOrUpdateProfileSenderRequest object</param>
        /// <returns>CreateOrUpdateProfileSenderResponse</returns>
        [HttpPatch]
        [Route("sender")]
        public IHttpActionResult CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderRequest req)
        {
            var createOrUpdateProfileSender = _business.CreateOrUpdateProfileSender(req);

            return Ok(createOrUpdateProfileSender);
        }

        /// <summary>
        /// Get consumer profile base on requst
        /// </summary>
        /// <param name="req">GetProfileConsumerRequest object</param>
        /// <returns>GetProfileConsumerResponse</returns>
        [HttpPost]
        [Route("profile")]
        public IHttpActionResult GetProfileConsumer(GetProfileConsumerRequest req)
        {
            var consumerRespVm = _business.GetProfileConsumer(req);
            return Ok(consumerRespVm);
        }

        /// <summary>
        /// Create or update consumer profile base on request
        /// </summary>
        /// <param name="req">CreateOrUpdateProfileConsumerRequest object</param>
        /// <returns>CreateOrUpdateProfileConsumerResponse</returns>
        [HttpPatch]
        [Route("profile")]
        public IHttpActionResult CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerRequest req)
        {
            var consumerRespVm = _business.CreateOrUpdateProfileConsumer(req);
            return Ok(consumerRespVm);
        }

        /// <summary>
        /// Save Personal ID Image
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("image")]
        public IHttpActionResult SavePersonalIDImage(ViewModels.Consumer.SavePersonalIDImageRequest req)
        {
            var response = _business.SavePersonalIDImage(req);
            return Ok(response);
        }

        /// <summary>
        /// Get Personal ID Image
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("image")]
        public IHttpActionResult GetPersonalIDImage(GetPersonalIDImageRequest req)
        {
            var response = _business.GetPersonalIDImage(req);
            return Ok(response);
        }

        /// <summary>
        /// Search consumer profiles
        /// </summary>
        /// <param name="req">SearchConsumerProfilesRequest object</param>
        /// <returns>SearchConsumerProfilesResponse</returns>
        [HttpPost]
        [Route("search")]
        public IHttpActionResult SearchConsumerProfiles(SearchConsumerProfilesRequest req)
        {
            var searchConsumerVm = _business.SearchConsumerProfiles(req);
            return Ok(searchConsumerVm);
        }

        /// <summary>
        /// Get consumer profile transaction history
        /// </summary>
        /// <param name="req">GetConsumerProfileTransactionHistoryRequest object</param>
        /// <returns>GetConsumerProfileTransactionHistoryResponse</returns>
        [HttpPost]
        [Route("transactions")]
        public IHttpActionResult GetConsumerProfileTransactionHistory(GetConsumerProfileTransactionHistoryRequest req)
        {
            var historyVm = _business.GetConsumerProfileTransactionHistory(req);
            return Ok(historyVm);
        }


        /// <summary>
        /// Save Consumer Profile Document
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("document")]
        public IHttpActionResult SaveConsumerProfileDocument(ViewModels.Consumer.SaveConsumerProfileDocumentRequest req)
        {
            var response = _business.SaveConsumerProfileDocument(req);
            return Ok(response);
        }

        /// <summary>
        /// Get Consumer Profile Document
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("document")]
        public IHttpActionResult GetConsumerProfileDocument(GetConsumerProfileDocumentRequest req)
        {
            var response = _business.GetConsumerProfileDocument(req);
            return Ok(response);
        }
    }
}