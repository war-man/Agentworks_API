using AwApi.ViewModels.Cache;
using MoneyGram.Common.Models;

namespace AwApi.Integration
{
    public interface ICacheIntegration
    {
        void DeleteCache();
        void DeleteAgentCache(AgentCacheRequest request);
        HealthCheckResponse HealthCheck();
    }
}