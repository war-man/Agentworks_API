using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using System.Web.Http;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin), AwDeviceAuthorization(DeviceSecurityLevel.Any)]
    [RoutePrefix("api/operator")]
    public class OperatorController : ApiController
    {
        private readonly IOperatorBusiness _operatorBusiness;

        public OperatorController(IOperatorBusiness operatorBusiness)
        {
            operatorBusiness.ThrowIfNull(nameof(operatorBusiness));

            _operatorBusiness = operatorBusiness;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetOperator()
        {
            var resp = _operatorBusiness.GetOperator();

            return Ok(resp);
        }

        [HttpGet]
        [Route("device")]
        public IHttpActionResult GetDevice()
        {
            var resp = _operatorBusiness.GetDevice();

            return Ok(resp);
        }
    }
}