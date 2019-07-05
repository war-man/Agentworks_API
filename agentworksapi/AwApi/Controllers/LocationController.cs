using AwApi.Auth;
using MoneyGram.Common.Extensions;
using System.Web.Http;
using AwApi.Business.LocationBusiness;
using AwApi.ViewModels;

namespace AwApi.Controllers
{
    [AwAuthorization(MgiAwRole.TellerManagerAdmin), AwDeviceAuthorization(DeviceSecurityLevel.Registered)]
    [RoutePrefix("api/location")]
    public class LocationController : ApiController
    {
        private readonly ILocationBusiness _businessLocation;

        public LocationController(ILocationBusiness businessLocation)
        {
            businessLocation.ThrowIfNull(businessLocation.ToString());

            _businessLocation = businessLocation;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Locations()
        {
            var allLocationsForMoRespVm = _businessLocation.GetLocations();

            return Ok(allLocationsForMoRespVm);
        }

        [HttpGet]
        [Route("{locationId}/pos")]
        public IHttpActionResult GetPosList(decimal locationId)
        {
            var currentPos = _businessLocation.GetPosListForLocation(locationId);

            return Ok(currentPos);
        }
    }
}