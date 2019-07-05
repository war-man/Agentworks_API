using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using AwApi.ViewModels;
using MoneyGram.AgentConnect;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Localization;
using MoneyGram.Common.Models;

namespace AwApi.Auth
{
    public class AgentProfilePrincipalService : IAgentProfilePrincipalService
    {
        private readonly IAgentConnect agentConnect;
        private readonly IAgentConnectConfig agentConnectConfig;
        private readonly ICacheManager cacheManager;

        public AgentProfilePrincipalService(IAgentConnect agentConnect, IAgentConnectConfig agentConnectConfig, ICacheManager cacheManager)
        {
            this.agentConnect = agentConnect;
            this.agentConnectConfig = agentConnectConfig;
            this.cacheManager = cacheManager;
        }

        public List<Claim> GetAgentProfileClaims(string agentId, string posNumber, string password, string language, string sessionId)
        {
            var agentProfileKey = string.Format(AuthCacheKeys.AGENTPROFILECLAIMS, sessionId);
            var agentProfileCachedClaims = cacheManager.Contains<CachedObjectResponseContainer<List<Tuple<string, string>>>>(agentProfileKey, CacheRegion.Global);

            if (agentProfileCachedClaims.Exists)
            {
                var missingClaimsInCache = ClaimsInventoryHelper.CheckAgentProfileClaims(agentProfileCachedClaims.CachedObj.DataObject.ToClaims());
                if (missingClaimsInCache.Any())
                {
                    agentProfileCachedClaims.Exists = false;
                    var agentProfileKeyFormatted = string.Format(AuthCacheKeys.AGENTPROFILEKEY, agentId, posNumber);
                    
                    cacheManager.Remove(agentProfileKey, CacheRegion.Global);
                    cacheManager.Remove(agentProfileKeyFormatted, CacheRegion.Global);
                }
                else
                {
                    // Cache exists and there are no missing claims, use cached claims
                    return agentProfileCachedClaims.CachedObj.DataObject.ToClaims();
                }
            }

            try
            {
                var agentProfile = GetAgentProfile(agentId, posNumber, password, language);
                var agentProfileClaims = ParseClaimsFromAgentProfile(agentProfile);
                var missingClaims = ClaimsInventoryHelper.CheckAgentProfileClaims(agentProfileClaims);

                if (missingClaims.Any())
                {
                    // Throw exception
                    throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                        LocalizationKeys.InvalidAgentProfile, missingClaims);
                }

                var agentProfileClaimsCacheContainer = CacheAheadHelper.PopulateCacheMetadata<List<Tuple<string, string>>>(agentProfileClaims.ToTupleList(), CachePolicies.FourHours);
                cacheManager.Save(agentProfileKey, agentProfileClaimsCacheContainer, CacheRegion.Global, CachePolicies.FourHours);

                return agentProfileClaims;
            }
            catch (Exception ex)
            {
                if (ex is CommunicationException)
                {
                    throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                        LocalizationKeys.SystemUnavailable, null);
                }

                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.InvalidAgentProfile, null);
            } 
        }

        private ProfileResponse GetAgentProfile(string agentId, string posNumber, string password, string language)
        {
            var agent = new Agent
            {
                AgentId = agentId,
                AgentPassword = password,
                AgentSequence = posNumber,
                Language = language
            };

            var profileRequest = new ProfileRequest();

            agentConnectConfig.DecorateRequest(profileRequest);

            return agentConnect.Profile(agent, profileRequest);
        }

        private List<Claim> ParseClaimsFromAgentProfile(ProfileResponse agentProfile)
        {
            var profileItems = agentProfile.Payload.ProfileItem;

            //var listOfSecondaryLanguages = getProfileItemValues(profileItems, "ALLOW_LANG_OPT").Where(lang => lang.ToLower() != "none");
            var primaryReceiptLanguage = getProfileItemValue(profileItems, ProfileKeyNames.PrimaryReceiptLanguage);

            var secondaryReceiptLanguage = profileItems
                .Where(profileItem => profileItem.Key == ProfileKeyNames.AllowLanguageOption)?
                .Where(profileItem => !(profileItem.Index == 1 && profileItem.Value == "eng"))?
                .FirstOrDefault(x => x.Value != null && x.Value.ToLower() != "none" && x.Value.ToLower() != primaryReceiptLanguage.ToLower())?
                .Value;

            var agentProfileClaims = new List<Claim>()
                .TryAddClaim(ClaimsNames.AgentName,
                    getProfileItemValue(profileItems, ProfileKeyNames.AgentName))
                .TryAddClaim(ClaimsNames.StoreName,
                    getProfileItemValue(profileItems, ProfileKeyNames.GenericDocumentStore))
                .TryAddClaim(ClaimsNames.AgentTimeZone,
                    getProfileItemValue(profileItems, ProfileKeyNames.AgentTimeZone))
                .TryAddClaim(ClaimsNames.AgentTelNo,
                    getProfileItemValue(profileItems, ProfileKeyNames.AgentPhone))
                .TryAddClaim(ClaimsNames.PrimaryReceiptLanguage, primaryReceiptLanguage)
                .TryAddClaim(ClaimsNames.SecondaryReceiptLanguage, secondaryReceiptLanguage);

            var prodAuthorizationClaims = GetProductAuthorizationsFromAgentProfile(agentProfile);
            agentProfileClaims.AddRange(prodAuthorizationClaims);
            return agentProfileClaims;
        }

        private List<Claim> GetProductAuthorizationsFromAgentProfile(ProfileResponse agentProfile)
        {
            var profileItems = agentProfile.Payload.ProfileItem;
            var productProfileItems = agentProfile.Payload.ProductProfileItem;
            var productGroups = productProfileItems.GroupBy(x => x.ProductID)
                .ToDictionary(grp => grp.Key, grp => grp.ToList());

            var sendProductStatus = getProductProfileItemValue(productGroups, ProfileProductType.Send, ProfileKeyNames.ProductStatus);
            var sendFullServiceEnabled = getProductProfileItemValue(productGroups, ProfileProductType.Send, ProfileKeyNames.FullServiceEnabled);
            var sendCompletionEnabled = getProductProfileItemValue(productGroups, ProfileProductType.Send, ProfileKeyNames.FormFreeEnabled);
            var allowAmends = getProfileItemValue(profileItems, ProfileKeyNames.AllowAmends);
            var allowSendCancels = getProfileItemValue(profileItems, ProfileKeyNames.AllowSendCancels);
            var allowSendReversal = getProfileItemValue(profileItems, ProfileKeyNames.AllowRefund);
            var allowFeeRefund = getProfileItemValue(profileItems, ProfileKeyNames.AllowFeeRefund);

            var receiveProductStatus = getProductProfileItemValue(productGroups, ProfileProductType.Receive, ProfileKeyNames.ProductStatus);
            var receiveFullServiceEnabled = getProductProfileItemValue(productGroups, ProfileProductType.Receive, ProfileKeyNames.FullServiceEnabled);
            var receiveCompletionEnabled = getProductProfileItemValue(productGroups, ProfileProductType.Receive, ProfileKeyNames.FormFreeEnabled);
            var allowReceiveReversals = getProfileItemValue(profileItems, ProfileKeyNames.AllowReceiveReversals);

            var billPayProductStatus = getProductProfileItemValue(productGroups, ProfileProductType.BillPayment, ProfileKeyNames.ProductStatus);
            var billPayFullServiceEnabled = getProductProfileItemValue(productGroups, ProfileProductType.BillPayment, ProfileKeyNames.FullServiceEnabled);
            var billPayCompletionEnabled = getProductProfileItemValue(productGroups, ProfileProductType.BillPayment, ProfileKeyNames.FormFreeEnabled);
            var prepaidCardLoadEnabled = getProductProfileItemValue(productGroups, ProfileProductType.BillPayment, ProfileKeyNames.ProductVariants, 3);

            var moneyOrderProductStatus = getProductProfileItemValue(productGroups, ProfileProductType.MoneyOrder, ProfileKeyNames.ProductStatus);
            var vendorPayProductStatus = getProductProfileItemValue(productGroups, ProfileProductType.VendorPayment, ProfileKeyNames.ProductStatus);

            // Send Products
            var send = sendProductStatus == "A" && sendFullServiceEnabled == "Y";
            var sendCompletion = sendProductStatus == "A" && sendCompletionEnabled == "Y";
            var amend = allowAmends == "Y";
            var cancel = allowSendCancels == "Y";
            var sendReversal = allowSendReversal == "Y";
            var feeRefund = allowFeeRefund == "Y";

            // Receive products
            var receive = receiveProductStatus == "A" && receiveFullServiceEnabled == "Y";
            var receiveCompletion = receiveProductStatus == "A" && receiveCompletionEnabled == "Y";
            var receiveReversal = allowReceiveReversals == "Y";

            // Bill Pay products
            var billPay = billPayProductStatus == "A" && billPayFullServiceEnabled == "Y";
            var billPayCompletion = billPayProductStatus == "A" && billPayCompletionEnabled == "Y";
            var loadCard = prepaidCardLoadEnabled == "Y";

            // Money Order products
            var moneyOrder = moneyOrderProductStatus == "A";
            var vendorPay = vendorPayProductStatus == "A";

            var agentProfileClaims = new List<Claim>()
                .TryAddClaim(ClaimsNames.CanSend, send.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanSendCompletion, sendCompletion.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanAmend, amend.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanCancel, cancel.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanSendReversal, sendReversal.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanFeeRefund, feeRefund.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanReceive, receive.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanReceiveCompletion, receiveCompletion.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanReceiveReversal, receiveReversal.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanBillPay, billPay.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanBillPayCompletion, billPayCompletion.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanLoadPrepaidCard, loadCard.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanMoneyOrder, moneyOrder.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanVendorPay, vendorPay.ToString(), ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanExpressPayment, "false", ClaimValueTypes.Boolean)
                .TryAddClaim(ClaimsNames.CanUtilityBillPay, "false", ClaimValueTypes.Boolean);

            return agentProfileClaims;
        }

        private string getProductProfileItemValue(Dictionary<int, List<ProductProfileItemType>> productGroups, int productGroup, string key, int? index = null)
        {
            if (!productGroups.ContainsKey(productGroup))
            {
                return null;
            }

            var productProfileItems = productGroups[productGroup];

            if (!index.HasValue)
            {
                return productProfileItems.FirstOrDefault(item => item.Key == key)?.Value;
            }
            else
            {
                return productProfileItems.FirstOrDefault(item => item.Key == key && item.Index == index.Value)?.Value;
            }
        }

        private string getProfileItemValue(List<ProfileItemType> profileItems, string key, int? index = null)
        {
            if (!index.HasValue)
            {
                return profileItems.FirstOrDefault(profileItem => profileItem.Key == key)?.Value;
            }
            else
            {
                return profileItems.FirstOrDefault(profileItem => profileItem.Key == key && profileItem.Index == index.Value)?.Value;
            }
        }

        private List<string> getProfileItemValues(List<ProfileItemType> profileItems, string key)
        {
            return profileItems.Where(profileItem => profileItem.Key == key)?.Select(item => item.Value).ToList();
        }
    }
}
