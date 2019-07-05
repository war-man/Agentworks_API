using System;
using System.Collections.Generic;
using System.Linq;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using AwApi.ViewModels.Lookup;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class LookupBusiness : ILookupBusiness
    {
        private readonly IAgentConnectIntegration _agentConnectIntegration;
        private readonly IProfileBusiness _profileBusiness;

        public LookupBusiness(IAgentConnectIntegration agentConnectIntegration, IProfileBusiness profileBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            profileBusiness.ThrowIfNull(nameof(profileBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _profileBusiness = profileBusiness;
        }

        public AcApiResponse<GetAllFieldsResponse, ApiData> GetAllFields(GetAllFieldsRequest req)
        {
            var resp = _agentConnectIntegration.GetAllFields(req);

            var apiResp = new AcApiResponse<GetAllFieldsResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }
        public AcApiResponse<GetEnumerationsResponse, ApiData> GetEnumerations(GetEnumerationsRequest req)
        {
            var resp = _agentConnectIntegration.GetEnumerations(req);

            var apiResp = new AcApiResponse<GetEnumerationsResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public GetEnumerationsResponse TryGetEnumerations(Request req)
        {
            try
            {
                var resp = _agentConnectIntegration.GetEnumerations(req.ToGetEnumerationsRequest());
                return resp;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public GetAllFieldsResponse TryGetAllFields(Request req, string cachedVersion, string tranType)
        {
            var getAllFieldsRequest = req.ToGetAllFieldsRequest();

            getAllFieldsRequest.TransactionType = tranType;
            getAllFieldsRequest.CachedVersion = cachedVersion;

            try
            {
                return _agentConnectIntegration.GetAllFields(getAllFieldsRequest);
            }
            catch(Exception exc)
            {
            }

            return null;
        }

        public AcApiResponse<GetCurrencyInfoResponse, ApiData> GetCurrencyInfo(GetCurrencyInfoRequest req)
        {
            //AgentConnect GetCurrencyInfo call
            var resp = _agentConnectIntegration.GetCurrencyInfo(req);

            var apiResp = new AcApiResponse<GetCurrencyInfoResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<GetCountryInfoResponse, ApiData> GetCountryInfo(GetCountryInfoRequest req)
        {
            var resp = _agentConnectIntegration.GetCountryInfo(req);

            var apiResp = new AcApiResponse<GetCountryInfoResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<GetCountrySubdivisionResponse, ApiData> GetCountrySubdivision(GetCountrySubdivisionRequest req)
        {
            var resp = _agentConnectIntegration.GetCountrySubdivision(req);

            var apiResp = new AcApiResponse<GetCountrySubdivisionResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<IndustryResponse, ApiData> Industry(IndustryRequest req)
        {
            var resp = _agentConnectIntegration.Industry(req);

            var apiResp = new AcApiResponse<IndustryResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<AllLookupResponse, ApiData> All(AllLookupRequest req)
        {
            var respVm = new AllLookupResponse();
            int? flags = null;

            var apiErrors = new Dictionary<string, string>();

            if(req.GetAllFieldsRequested)
            {
                try
                {
                    var fields = GetAllFields(req.GetAllFieldsReq);
                    respVm.GetAllFieldsResponse = fields.ResponseData;

                    flags = respVm.GetAllFieldsResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("GetAllFields", e.Message);
                }
            }

            if (req.EnumerationsRequested)
            {
                try
                {
                    var enumerations = GetEnumerations(req.EnumerationReq);
                    respVm.GetEnumerationsResponse = enumerations.ResponseData;

                    flags = respVm.GetEnumerationsResponse?.Payload?.Flags;
                }
                catch (Exception e)
                {
                    apiErrors.Add("Enumerations", e.Message);
                }
            }

            if (req.ProfileRequested)
            {
                try
                {
                    var profile = _profileBusiness.Profile(req.ProfileReq);
                    respVm.ProfileResponse = profile.ResponseData;

                    flags = respVm.ProfileResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("Profile", e.Message);
                }
            }

            if(req.CurrenciesRequested)
            {
                try
                {
                    var getCurrencyInfoResponse = GetCurrencyInfo(req.GetCurrencyInfoReq ?? new GetCurrencyInfoRequest());
                    respVm.GetCurrencyInfoResponse = getCurrencyInfoResponse.ResponseData;

                    flags = respVm.GetCurrencyInfoResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("Currencies", e.Message);
                }
            }

            if(req.CountriesRequested)
            {
                try
                {
                    var getCountryInfoResponse = GetCountryInfo(req.GetCountryInfoReq ?? new GetCountryInfoRequest());
                    respVm.GetCountryInfoResponse = getCountryInfoResponse.ResponseData;

                    flags = respVm.GetCountryInfoResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("Countries", e.Message);
                }
            }

            if(req.CountrySubdivisionsRequested)
            {
                try
                {
                    var getCountrySubdivisionResponse =
                        GetCountrySubdivision(req.GetCountrySubdivisionReq ?? new GetCountrySubdivisionRequest());
                    respVm.GetCountrySubdivisionResponse = getCountrySubdivisionResponse.ResponseData;

                    flags = respVm.GetCountrySubdivisionResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("CountrySubdivisions", e.Message);
                }
            }

            if(req.IndustriesRequested)
            {
                try
                {
                    var industryResponse = Industry(req.IndustryRequestReq ?? new IndustryRequest());
                    respVm.IndustryResponse = industryResponse.ResponseData;

                    flags = respVm.IndustryResponse?.Payload?.Flags;
                }
                catch(Exception e)
                {
                    apiErrors.Add("Industries", e.Message);
                }
            }

            var apiResp = new AcApiResponse<AllLookupResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(flags, DataSource.Lookup),
                ResponseData = respVm,
                ApiErrors = apiErrors.Any() ? apiErrors : null
            };

            return apiResp;
        }
    }
}