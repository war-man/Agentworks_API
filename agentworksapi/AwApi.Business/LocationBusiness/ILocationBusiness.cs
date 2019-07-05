using System.Collections.Generic;
using AwApi.ViewModels.Location;
using AwApi.ViewModels;

namespace AwApi.Business.LocationBusiness
{
    public interface ILocationBusiness
    {
        ApiResponse<LocationResVm, ApiData> GetLocations();
        ApiResponse<PosResponse, ApiData> GetPosListForLocation(decimal locationid);
        List<AgentVm> LocationsForMainOffice(string mainOfficeId, string subLevelNameId = null);
    }
}