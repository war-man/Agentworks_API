using AwApi.Cache;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;
using MoneyGram.PartnerHierarchy;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;

namespace AwApi.Integration
{
    public class PartnerHierarchyCache : PartnerHierarchyDecorator
    {
        private readonly ICacheManager _cacheManager;
        public PartnerHierarchyCache(IPartnerHierarchyRepository partnerHierarchy, ICacheManager cacheManager) : base(partnerHierarchy)
        {
            partnerHierarchy.ThrowIfNull(nameof(partnerHierarchy));
            cacheManager.ThrowIfNull(nameof(cacheManager));
            _cacheManager = cacheManager;
        }

        public override PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request)
        {            
            PartnerHierarchyAgentResponse hierarchyResponse;
            var partnerHierarchyKeyFormatted = string.Format(CacheKeys.PARTNERHIERARCHYKEY, request.MainofficeId, request.LocationId);

            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<PartnerHierarchyAgentResponse>>(partnerHierarchyKeyFormatted, CacheRegion.Global);

            if (cachedResult.Exists)
            {
                hierarchyResponse = cachedResult.CachedObj.DataObject;
            }
            else
            {
                // Fetch payload and process
                hierarchyResponse = base.GetPartnerHierarchyAgent(request);
                var agentPasswordCacheContainer = CacheAheadHelper.PopulateCacheMetadata<PartnerHierarchyAgentResponse>(hierarchyResponse, CachePolicies.FullWeek);
                // Cache it
                _cacheManager.Save(partnerHierarchyKeyFormatted, agentPasswordCacheContainer, CacheRegion.Global, CachePolicies.FullWeek);
            }
            return hierarchyResponse;
        }
    }
}
