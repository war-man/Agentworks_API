using System;
using System.Collections.Generic;
using System.ServiceModel;
using AwApi.Integration.Reports;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Localization;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;

namespace AwApi.Auth
{
    public class AgentPasswordPrincipalService : IAgentPasswordPrincipalService
    {
        private readonly IPartnerServiceIntegration partnerServiceIntegration;
        private readonly ICacheManager cacheManager;

        public AgentPasswordPrincipalService(IPartnerServiceIntegration partnerServiceIntegration, ICacheManager cacheManager)
        {
            this.partnerServiceIntegration = partnerServiceIntegration;
            this.cacheManager = cacheManager;
        }

        public string GetAgentPassword(string token, string agentId, string posNumber)
        {
            var agentPasswordCacheKey = string.Format(AuthCacheKeys.PARTNERSERVICECLAIMS, token);
            var partnerServiceCached = cacheManager.Contains<CachedObjectResponseContainer<string>>(agentPasswordCacheKey,
                CacheRegion.Global);

            if (partnerServiceCached.Exists)
            {
                // no password is cached
                if (string.IsNullOrEmpty(partnerServiceCached.CachedObj.DataObject))
                {
                    // We have incomplete data. Invalidate cache
                    partnerServiceCached.Exists = false;
                    cacheManager.Remove(agentPasswordCacheKey, CacheRegion.Global);
                }
                else
                {
                    // Cache exists and there are no missing claims, use cached claims
                    return partnerServiceCached.CachedObj.DataObject;
                }
            }

            var agentPassword = "";
            try
            {
                agentPassword = GetAgentPassword(agentId, posNumber);
            }
            catch (Exception ex)
            {
                if (ex is CommunicationException)
                {
                    // Unreachable.
                    throw PrincipalExceptionFactory.Create(PrincipalExceptionType.PartnerService,
                        LocalizationKeys.SystemUnavailable, null);
                }
                // Reachable but has a problem.
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.PartnerService,
                    LocalizationKeys.InvalidAgentProfile, null);
            }
            
            if (string.IsNullOrEmpty(agentPassword))
            {
                // Throw exception
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.PartnerService,
                    LocalizationKeys.InvalidAgentProfile, new List<string> { "agentPassword" });
            }

            var partnerServiceClaimsCacheContainer = CacheAheadHelper.PopulateCacheMetadata<string>(agentPassword, CachePolicies.FourHours);
            cacheManager.Save(agentPasswordCacheKey, partnerServiceClaimsCacheContainer, CacheRegion.Global,
                CachePolicies.FourHours);

            return agentPassword;
        }

        private string GetAgentPassword(string agentId, string posNumber)
        {
            var agentPasswordRequest = new AgentPasswordRequest
            {
                AgentId = decimal.Parse(agentId),
                AgentIdSpecified = true,
                PosNumber = decimal.Parse(posNumber),
                PosNumberSpecified = true
            };

            var agentPasswordRequestHeader = new Header();
            var agentPasswordProcessingInstruction = new ProcessingInstruction
            {
                Action = "GetAgentPassword",
                RollbackTransaction = false
            };

            agentPasswordRequestHeader.ProcessingInstruction = agentPasswordProcessingInstruction;
            agentPasswordRequest.header = agentPasswordRequestHeader;
            var getAgentPasswordResp = partnerServiceIntegration.GetAgentPassword(agentPasswordRequest);
            return getAgentPasswordResp.AgentPassword;
        }
    }
}
