using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin, MgiAwRole.Dt4Device), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        private readonly IProfileBusiness _profileBusiness;

        public ProfileController(IProfileBusiness profileBusiness)
        {
            profileBusiness.ThrowIfNull(nameof(profileBusiness));

            _profileBusiness = profileBusiness;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Profile([FromBody] ProfileRequest profileRequest)
        {
            profileRequest.ThrowIfNull(nameof(profileRequest));

            var profileRespVm = _profileBusiness.Profile(profileRequest);

            return Ok(profileRespVm);
        }

        [AwAuthorization(MgiAwRole.Manager), AwDeviceAuthorization(DeviceSecurityLevel.Transactional)]
        [HttpPatch]
        [Route("")]
        public IHttpActionResult SaveProfile([FromBody] SaveProfileRequest saveProfileRequest)
        {
            saveProfileRequest.ThrowIfNull(nameof(saveProfileRequest));

            var response = _profileBusiness.SaveProfile(saveProfileRequest);

            return Ok(response);
        }

        [HttpPatch]
        [Route("printer")]
        public IHttpActionResult SaveProfilePrinter([FromBody] SaveProfilePrinterRequest request)
        {
            request.ThrowIfNull(nameof(request));

            _profileBusiness.SaveProfilePrinter(request);

            return Ok();
        }
    }
}