using AwApi.ViewModels;
using AwApi.ViewModels.Cache;
using AwApi.ViewModels.Health;

namespace AwApi.Business
{
    public interface ISupportBusiness
    {
        void DeleteCache();

        void DeleteAgentCache(AgentCacheRequest request);

        ApiResponse<DeepHealthCheckResponse, ApiData> DeepHealthCheck();
    }
}