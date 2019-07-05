using System.Collections.Generic;
using System.Linq;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.Integration.Reports;
using AwApi.ViewModels;
using AwApi.ViewModels.Cache;
using AwApi.ViewModels.Health;
using MoneyGram.Common;
using MoneyGram.Common.Extensions;
using MoneyGram.Common.Json;
using MoneyGram.Common.Models;

namespace AwApi.Business
{
    public class SupportBusiness : ISupportBusiness
    {
        private readonly IAgentConnectIntegration _agentConnectIntegration;
        private readonly ICacheIntegration _cacheIntegration;
        private readonly IDlsIntegration _dlsIntegration;
        private readonly IOpenAmIntegration _openAmIntegration;
        private readonly IOpenIdmIntegration _openIdmIntegration;
        private readonly IPartnerServiceIntegration _partnerServiceIntegration;

        public SupportBusiness(ICacheIntegration cacheIntegration, IAgentConnectIntegration agentConnectIntegration,
            IPartnerServiceIntegration partnerServiceIntegration, IDlsIntegration dlsIntegration,
            IOpenIdmIntegration openIdmIntegration, IOpenAmIntegration openAmIntegration)
        {
            cacheIntegration.ThrowIfNull(nameof(cacheIntegration));
            cacheIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            cacheIntegration.ThrowIfNull(nameof(partnerServiceIntegration));
            cacheIntegration.ThrowIfNull(nameof(dlsIntegration));
            cacheIntegration.ThrowIfNull(nameof(openAmIntegration));
            cacheIntegration.ThrowIfNull(nameof(openIdmIntegration));

            _cacheIntegration = cacheIntegration;
            _agentConnectIntegration = agentConnectIntegration;
            _partnerServiceIntegration = partnerServiceIntegration;
            _dlsIntegration = dlsIntegration;
            _openAmIntegration = openAmIntegration;
            _openIdmIntegration = openIdmIntegration;
        }

        public void DeleteCache()
        {
            _cacheIntegration.DeleteCache();
        }

        public void DeleteAgentCache(AgentCacheRequest request)
        {
            _cacheIntegration.DeleteAgentCache(request);
        }

        public ApiResponse<DeepHealthCheckResponse, ApiData> DeepHealthCheck()
        {
            var respVm = new DeepHealthCheckResponse
            {
                HealthStatus = new List<HealthCheckResponse>
                {
                    _agentConnectIntegration.HealthCheck(),
                    _dlsIntegration.HealthCheck(),
                    _partnerServiceIntegration.HealthCheck(),
                    _openAmIntegration.HealthCheck(),
                    _openIdmIntegration.HealthCheck(),
                    _cacheIntegration.HealthCheck()
                }
            };

            var apiErrors = ProcessApiErrors(respVm);

            return new ApiResponse<DeepHealthCheckResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Support),
                ResponseData = respVm,
                ApiErrors = apiErrors
            };
        }

        private static Dictionary<string, string> ProcessApiErrors(DeepHealthCheckResponse respVm)
        {
            // This processing is done at the request of DYNATRACE team
            //Parse the deep health check responses, throw & catch the exception for dynatrace monitoring. But return OK from the controller.
            Dictionary<string, string> apiErrors = null;
            try
            {
                var failedHealthCheck = respVm.HealthStatus.Any(x => x.StatusCode != StatusCode.Ok && x.StatusCode != StatusCode.NotImplemented);
                if(failedHealthCheck)
                {
                    throw new DeepHealthCheckException
                    {
                        ErrorString = "One or more dependencies have failed.",
                        DetailString = JsonProcessor.SerializeObject(respVm)
                    };
                }
            }
            catch(DeepHealthCheckException exc)
            {
                apiErrors = new Dictionary<string, string>
                {
                    {"DeepHealthCheckException", exc.ErrorString}
                };
            }

            return apiErrors;
        }
    }
}