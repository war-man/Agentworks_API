using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.Teller), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/deposit")]
    public class DepositController : ApiController
    {
        private readonly IDepositBusiness depositBusiness;

        public DepositController(IDepositBusiness depositBusiness)
        {
            depositBusiness.ThrowIfNull(nameof(depositBusiness));

            this.depositBusiness = depositBusiness;
        }

        [HttpPost]
        [Route("information")]
        public IHttpActionResult GetDepositInformation([FromBody] GetDepositInformationRequest req)
        {
            req = req ?? new GetDepositInformationRequest();

            var response = depositBusiness.GetDepositInformation(req);

            return Ok(response);
        }

        [HttpPost]
        [Route("list")]
        public IHttpActionResult GetDepositBankList([FromBody] GetDepositBankListRequest req)
        {
            req = req ?? new GetDepositBankListRequest();

            var response = depositBusiness.GetDepositBankList(req);

            return Ok(response);
        }

        [HttpPost]
        [Route("announcement")]
        public IHttpActionResult DepositAnnouncement([FromBody] DepositAnnouncementRequest req)
        {
            req = req ?? new DepositAnnouncementRequest();

            var response = depositBusiness.DepositAnnouncement(req);

            return Ok(response);
        }
    }
}