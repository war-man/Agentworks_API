using System;
using System.Collections.Generic;
using System.Linq;
using AwApi.EntityMapper.LocationVmExtensions;
using AwApi.Integration;
using AwApi.ViewModels;
using AwApi.ViewModels.Location;
using MoneyGram.Common.Extensions;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using AwApi.EntityMapper;
using AwApi.Integration.Reports;

namespace AwApi.Business.LocationBusiness
{
    public class LocationBusiness : ILocationBusiness
    {
        private IPartnerServiceIntegration _partnerIntegration;

        private MoneyGram.AgentConnect.DomainModel.Agent _authAgent => AuthIntegration.GetAgent();

        private User _authUser => AuthIntegration.GetUser();

        private const int poeCode = 26;

        public LocationBusiness(IPartnerServiceIntegration repository)
        {
            _partnerIntegration = repository;
        }

        #region Public APIs
        public ApiResponse<LocationResVm, ApiData> GetLocations()
        {
            LocationResVm result = new LocationResVm();

            List<AgentVm> mainOfficeLocations = new List<AgentVm>();
            List<AgentVm> searchedLocationsWithMO = new List<AgentVm>();
            List<AgentVm> searchedLocationsExternal = new List<AgentVm>();

            AgentVm mainOfficeAgent = GetMainOfficeAgent();

            //Get all the lcoations for mainoffice(exluding mainoffice agent)
            mainOfficeLocations = LocationsForMainOffice(_authUser.MainOfficeAgentId);

            searchedLocationsWithMO.Add(mainOfficeAgent);
            searchedLocationsWithMO.AddRange(mainOfficeLocations);

            if (AuthIntegration.GetMainOfficeId().ToString() != string.Empty &&
                _authUser.Status.Equals(UserStatus.External) && _authUser.UserAgentList != null &&
                _authUser.UserAgentList.Count != 0)
            {
                UserAgentActivity userAgent = (_authUser.UserAgentList != null
                    ? _authUser.UserAgentList.FirstOrDefault()
                    : new UserAgentActivity());

                // if user has access to all locations 
                if (_authUser.AllLocationsAllowed)
                {
                    searchedLocationsExternal = LocationsForMainOffice(_authUser.MainOfficeAgentId).ToList();
                }
                //// if user is main office user and has the AgentAssistfunctionality
                else if (IsUserAgentAssistFuncionality(_authUser) &&
                         userAgent.AgentId.Equals(AuthIntegration.GetMainOfficeId()))
                {
                    searchedLocationsExternal = searchedLocationsWithMO;
                }
                else
                {
                    // if user is either sublevel or location user then

                    foreach (UserAgentActivity u in _authUser.UserAgentList)
                    {
                        var agent = mainOfficeLocations.Find(ag => ag.id.ToString() == u.AgentId);
                        if (agent != null)
                        {
                            if ((HierarchyLevel) agent.hierarchyLevel == HierarchyLevel.SubLevel)
                            {
                                //passing the agentid inplace of the sublevelagentid field
                                var locations = LocationsForMainOffice(_authUser.MainOfficeAgentId, agent.id);
                                searchedLocationsExternal.AddRange(locations);
                            }
                            else
                            {
                                searchedLocationsExternal.Add(agent);
                            }
                        }
                    }
                }
                result.Agents = searchedLocationsExternal;
            }
            else
            {
                result.Agents = searchedLocationsWithMO;
            }

            return new ApiResponse<LocationResVm, ApiData>
            {
                ResponseData = result,
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.PartnerService)
            };
        }

        public ApiResponse<PosResponse, ApiData> GetPosListForLocation(decimal locationId)
        {
            var req = new PosDeviceRequest();
            var header = new MoneyGram.PartnerService.DomainModel.Header();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;

            req.header.ProcessingInstruction.Action = "GetPOSDevice";
            req.AgentId = locationId;
            // Partner Service will allow now to pass NULL for PoeCode
            // If NULL is passed, Partner Service will return the list of all POS types that are active
            //req.PoeCode = poeCode;

            var posDeviceResp = _partnerIntegration.GetPOSById(req);
            var posDeviceRespVm = posDeviceResp.ToVm();

            return new ApiResponse<PosResponse, ApiData>
            {
                ResponseData = posDeviceRespVm,
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.PartnerService)
            };
        }

        public List<AgentVm> LocationsForMainOffice(string mainOfficeId, string subLevelNameId = null)
        {
            mainOfficeId.ThrowIfNullOrEmpty(nameof(mainOfficeId));

            var req = new POELocationsForMoRequest
            {
                header = new Header
                {
                    ProcessingInstruction = new ProcessingInstruction
                    {
                        Action = "GetPOELocationsForMo",
                        RollbackTransaction = false
                    }
                },
                MainOfficeId = Convert.ToDecimal(mainOfficeId),
                SubLevelNameId = subLevelNameId,
                POECode = null
            };

            var locationsResponse = _partnerIntegration.GetPOELocationsForMo(req);

            return locationsResponse?.ToVm()?.Agents;
        }
        #endregion

        #region "Private Methods"

        private List<AgentVm> GetLocationsForMo(LocationRequest poeLocRequest)
        {

            var req = new LocationsForMoRequest();
            var header = new MoneyGram.PartnerService.DomainModel.Header();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;

            req.header.ProcessingInstruction.Action = "GetLocationsForMo";
            req.MainOfficeId = poeLocRequest.MainOfficeId;
            req.StoreNameNumberAgentId = poeLocRequest.StoreNameNumberAgentId ?? string.Empty;
            req.SubLevelNameId = poeLocRequest.SubLevelNameId ?? string.Empty;
            req.City = poeLocRequest.City ?? string.Empty;
            req.StateProvince = poeLocRequest.StateProvince ?? string.Empty;
            req.Country = poeLocRequest.Country ?? string.Empty;
            req.ZipCode = poeLocRequest.ZipCode ?? string.Empty;

            var searchedLocationsResp = _partnerIntegration.GetLocationsForMo(req);
            return searchedLocationsResp.ToVm().Agents;
        }

        private AgentVm GetMainOfficeAgent()
        {
            //To get the Main Office Id, we need to pass 1 in the HierarchyLevel.
            var req = new LocationRequest
            {
                StoreNameNumberAgentId = _authUser.MainOfficeAgentId,
                HierarchyLevel = 1
            };
            
            return GetAgents(req)
                .FirstOrDefault();
        }

        private List<AgentVm> GetAgents(LocationRequest locRequest)
        {
            var req = new AgentsRequest();
            var header = new MoneyGram.PartnerService.DomainModel.Header();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;

            req.AgentId = Convert.ToDecimal(locRequest.StoreNameNumberAgentId);
            req.header.ProcessingInstruction.Action = "GetAgents";
            req.AgentIdSpecified = true;
            req.AgentName = locRequest.AgentName ?? string.Empty;
            req.City = locRequest.City ?? string.Empty;
            req.State = locRequest.State ?? string.Empty;
            req.StateCode = locRequest.StateCode ?? string.Empty;
            req.Country = locRequest.Country ?? string.Empty;
            req.ZipCode = locRequest.ZipCode ?? string.Empty;
            req.Phone = locRequest.Phone ?? string.Empty;
            req.HierarchyLevel = locRequest.HierarchyLevel;
            //req.HierarchyLevelSpecified = (locationReqVm.HierarchyLevelSpecified == null ? true : locationReqVm.HierarchyLevelSpecified);

            var contentAgentsResponse = _partnerIntegration.GetAgents(req);
            return contentAgentsResponse.ToVm().Agents;
        }

        private List<AgentVm> GetLocationsPerMainOffice()
        {
            var req = new AgentLocationMoRequest();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = new MoneyGram.PartnerService.DomainModel.Header();
            req.header.ProcessingInstruction = processInstruction;
            req.AgentId = Convert.ToDecimal(_authUser.MainOfficeAgentId);

            req.header.ProcessingInstruction.Action = "GetAgentLocationMo";

            var contentAgentLocationMoResponse = _partnerIntegration.GetAgentLocationsForMo(req);
            return contentAgentLocationMoResponse.ToVm().Agents;
        }

        public static bool IsUserAgentAssistFuncionality(User user)
        {
            if (user?.UserAgentList == null || user.UserAgentList.Count == 0)
            {
                return false;
            }
            var userAgentActivity = user.UserAgentList.FirstOrDefault();
            return userAgentActivity != null && userAgentActivity.UserActivityType.Equals(ActivityType.AA);
        }

        #endregion
    }
}