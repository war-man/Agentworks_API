using System.Web.Http;
using AwApi.Auth;
using AwApi.Business;
using AwApi.ViewModels;
using AwApi.ViewModels.Event;
using MoneyGram.Common.Extensions;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin, MgiAwRole.Dt4UnregisteredDevice, MgiAwRole.Dt4Device, MgiAwRole.Dt4Support)]
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        private readonly IEventBusiness _eventBusiness;

        public EventController(IEventBusiness eventBusiness)
        {
            eventBusiness.ThrowIfNull(nameof(eventBusiness));

            _eventBusiness = eventBusiness;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Process([FromBody] EventRequest request)
        {
            var responseVm = _eventBusiness.Process(request);

            return Ok(responseVm);
        }
    }
}