using AwApi.Cache;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;
using MoneyGram.PartnerService;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;

namespace AwApi.Integration
{
    public class PartnerServiceCache : PartnerServiceDecorator
    {
        private readonly ICacheManager _cacheManager;
        public PartnerServiceCache(IPartnerServiceRepository partnerService, ICacheManager cacheManager)
            : base(partnerService)
        {
            partnerService.ThrowIfNull(nameof(partnerService));
            cacheManager.ThrowIfNull(nameof(cacheManager));
            _cacheManager = cacheManager;
        }
        public override AgentPasswordResponse GetAgentPassword(AgentPasswordRequest agentPasswordRequest)
        {
            AgentPasswordResponse agentPasswordResponse;
            var agentPasswordKeyFormatted = string.Format(CacheKeys.AGENTPASSWORDKEY, agentPasswordRequest.AgentId, agentPasswordRequest.PosNumber);

            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<AgentPasswordResponse>>(agentPasswordKeyFormatted, CacheRegion.Global);

            if (cachedResult.Exists)
            {
                agentPasswordResponse = cachedResult.CachedObj.DataObject;
            }
            else
            {
                // Fetch payload and process
                agentPasswordResponse = base.GetAgentPassword(agentPasswordRequest);
                var agentPasswordCacheContainer = CacheAheadHelper.PopulateCacheMetadata<AgentPasswordResponse>(agentPasswordResponse, CachePolicies.FullWeek);
                // Cache it
                _cacheManager.Save(agentPasswordKeyFormatted, agentPasswordCacheContainer, CacheRegion.Global, CachePolicies.FullWeek);
            }
            return agentPasswordResponse;
        }
    }
}