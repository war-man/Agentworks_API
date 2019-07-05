using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    /// <summary>
    /// Internal Controller exposes functionality available only to internal POE integrators
    /// </summary>
    [AwAuthorization(MgiAwRole.Teller, MgiAwRole.Dt4UnregisteredDevice)]
    [RoutePrefix("api/internal")]
    public class InternalController : ApiController
    {
        private IInternalBusiness _internalBusiness;

        public InternalController(IInternalBusiness internalBusiness)
        {
            _internalBusiness = internalBusiness;
        }

        [HttpPost]
        [Route("setup")]
        public IHttpActionResult DwSetup([FromBody] DwInitialSetupRequest dwInitialSetupReq)
        {
            dwInitialSetupReq.ThrowIfNull(nameof(dwInitialSetupReq));
            var resp = _internalBusiness.DwInitialSetup(dwInitialSetupReq);

            return Ok(resp);
        }

        [HttpPost]
        [Route("profile")]
        public IHttpActionResult DwProfile([FromBody] DwProfileRequest dwProfileReq)
        {
            dwProfileReq.ThrowIfNull(nameof(dwProfileReq));
            var resp = _internalBusiness.DwProfile(dwProfileReq);
            return Ok(resp);
        }

        [AwAuthorization(MgiAwRole.Teller, MgiAwRole.Dt4UnregisteredDevice)]
        [HttpPost]
        [Route("register-device")]
        public IHttpActionResult RegisterDevice(DwRegisterDeviceReqVm request)
        {
            var resp = _internalBusiness.DwRegisterDevice(request);
            return Ok(resp);
        }
    }
}