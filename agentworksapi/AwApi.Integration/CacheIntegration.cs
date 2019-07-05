using AwApi.Cache;
using AwApi.ViewModels.Cache;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;
using MoneyGram.Common.Models;

namespace AwApi.Integration
{
    public class CacheIntegration : ICacheIntegration
    {
        private readonly ICacheManager _cacheManager;

        public CacheIntegration(ICacheManager cacheManager)
        {
            cacheManager.ThrowIfNull(nameof(cacheManager));
            _cacheManager = cacheManager;
        }

        public void DeleteCache()
        {
            var token = AuthIntegration.GetToken();
            var agent = AuthIntegration.GetAgent();

            // Delete all auth-related cache
            var camsClaimsKey = string.Format(CacheKeys.CAMSCLAIMS, token);
            _cacheManager.Remove(camsClaimsKey, CacheRegion.Global);

            var authClaimsKey = string.Format(CacheKeys.AUTHCLAIMS, token);
            _cacheManager.Remove(authClaimsKey, CacheRegion.Global);

            var authRolesKey = string.Format(CacheKeys.AUTHROLES, token);
            _cacheManager.Remove(authRolesKey, CacheRegion.Global);

            var partnerServiceClaimsKey = string.Format(CacheKeys.PARTNERSERVICECLAIMS, token);
            _cacheManager.Remove(partnerServiceClaimsKey, CacheRegion.Global);

            var agentPasswordKeyFormatted = string.Format(CacheKeys.AGENTPASSWORDKEY, agent.AgentId, agent.AgentSequence);
            _cacheManager.Remove(agentPasswordKeyFormatted, CacheRegion.Global);

            var agentProfileClaimsKey = string.Format(CacheKeys.AGENTPROFILECLAIMS, token);
            _cacheManager.Remove(agentProfileClaimsKey, CacheRegion.Global);

            // Delete cached Agent Profile
            var agentProfileKeyFormatted = string.Format(CacheKeys.AGENTPROFILEKEY, agent.AgentId, agent.AgentSequence);
            _cacheManager.Remove(agentProfileKeyFormatted, CacheRegion.Global);

            // Delete cache for Countries, States, Currencies, Industries.
            var currencyInfoKeyFormatted = string.Format(CacheKeys.CURRENCYINFOKEY, agent.Language);
            _cacheManager.Remove(currencyInfoKeyFormatted, CacheRegion.Global);

            var countriesKeyFormatted = string.Format(CacheKeys.COUNTRYINFOKEY, agent.Language);
            _cacheManager.Remove(countriesKeyFormatted, CacheRegion.Global);

            var countrySubdivitionKeyFormatted = string.Format(CacheKeys.COUNTRYSUBDIVISIONKEY, agent.Language);
            _cacheManager.Remove(countrySubdivitionKeyFormatted, CacheRegion.Global);

            var industryKeyFormatted = string.Format(CacheKeys.INDUSTRYKEY, agent.Language);
            _cacheManager.Remove(industryKeyFormatted, CacheRegion.Global);
        }

        public void DeleteAgentCache(AgentCacheRequest request)
        {
            var agentProfileKeyFormatted = string.Format(CacheKeys.AGENTPROFILEKEY, request.AgentId, request.AgentSequence);
            _cacheManager.Remove(agentProfileKeyFormatted, CacheRegion.Global);

            var agentPasswordKeyFormatted = string.Format(CacheKeys.AGENTPASSWORDKEY, request.AgentId, request.AgentSequence);
            _cacheManager.Remove(agentPasswordKeyFormatted, CacheRegion.Global);
        }

        public HealthCheckResponse HealthCheck()
        {
            return _cacheManager.HealthCheck();
        }
    }
}