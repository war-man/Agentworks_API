using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using MoneyGram.Common.Models;
using System;
using AwApi.Cache;

namespace AwApi.Integration
{
    public class AgentConnectCache : AgentConnectDecorator
    {
        private readonly ICacheManager _cacheManager;

        public AgentConnectCache(IAgentConnect agentConnect, ICacheManager cacheManager)
            : base(agentConnect)
        {
            agentConnect.ThrowIfNull(nameof(agentConnect));
            cacheManager.ThrowIfNull(nameof(cacheManager));
            _cacheManager = cacheManager;
        }

        public override GetCurrencyInfoResponse GetCurrencyInfo(Agent agent, GetCurrencyInfoRequest getCurrencyInfoRequest)
        {
            GetCurrencyInfoResponse currencyInfoResp;
            string currencyInfoKeyFormatted = string.Format(CacheKeys.CURRENCYINFOKEY, agent.Language);
            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<GetCurrencyInfoResponse>>(currencyInfoKeyFormatted, CacheRegion.Global);

            Func<GetCurrencyInfoResponse> currencyInfoCacheSaveFunction = delegate ()
            {
                var version = getCurrencyInfoRequest.Version;
                getCurrencyInfoRequest.Version = null;

                currencyInfoResp = base.GetCurrencyInfo(agent, getCurrencyInfoRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<GetCurrencyInfoResponse>(currencyInfoResp, CachePolicies.FullWeek);
                _cacheManager.Save(currencyInfoKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);

                getCurrencyInfoRequest.Version = version;
                return currencyInfoResp;
            };

            if (cachedResult.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<GetCurrencyInfoResponse>(currencyInfoCacheSaveFunction, cachedResult.CachedObj.CacheMetadata);
                if (getCurrencyInfoRequest.Version != cachedResult.CachedObj.DataObject.Payload.Version)
                {
                    currencyInfoResp = cachedResult.CachedObj.DataObject;
                }
                else
                {
                    currencyInfoResp = new GetCurrencyInfoResponse {Payload = new GetCurrencyInfoResponsePayload {Version = getCurrencyInfoRequest.Version}};
                }
            }
            else
            {
                currencyInfoResp = currencyInfoCacheSaveFunction();
            }
            return currencyInfoResp;
        }

        public override GetCountrySubdivisionResponse GetCountrySubdivision(Agent agent, GetCountrySubdivisionRequest getCountrySubdivisionRequest)
        {
            GetCountrySubdivisionResponse countrySubdivisionResponse;
            var countrySubdivisionKeyFormatted = string.Format(CacheKeys.COUNTRYSUBDIVISIONKEY, agent.Language);
            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<GetCountrySubdivisionResponse>>(countrySubdivisionKeyFormatted, CacheRegion.Global);

            Func<GetCountrySubdivisionResponse> countrySubdivisionCacheSaveFunction = delegate ()
            {
                var version = getCountrySubdivisionRequest.CachedVersion;
                getCountrySubdivisionRequest.CachedVersion = null;

                countrySubdivisionResponse = base.GetCountrySubdivision(agent, getCountrySubdivisionRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata(countrySubdivisionResponse, CachePolicies.FullWeek);
                _cacheManager.Save(countrySubdivisionKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);

                getCountrySubdivisionRequest.CachedVersion = version;
                return countrySubdivisionResponse;
            };

            if (cachedResult.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<GetCountrySubdivisionResponse>(countrySubdivisionCacheSaveFunction, cachedResult.CachedObj.CacheMetadata);
                if (getCountrySubdivisionRequest.CachedVersion != cachedResult.CachedObj.DataObject.Payload.Version)
                {
                    countrySubdivisionResponse = cachedResult.CachedObj.DataObject;
                }
                else
                {
                    countrySubdivisionResponse = new GetCountrySubdivisionResponse {Payload = new GetCountrySubdivisionResponsePayload {Version = getCountrySubdivisionRequest.CachedVersion}};
                }
            }
            else
            {
                countrySubdivisionResponse = countrySubdivisionCacheSaveFunction();
            }

            return countrySubdivisionResponse;
        }

        public override GetCountryInfoResponse GetCountryInfo(Agent agent, GetCountryInfoRequest getCountryInfoRequest)
        {
            GetCountryInfoResponse countryInfo;
            var countryInfoKeyFormatted = string.Format(CacheKeys.COUNTRYINFOKEY, agent.Language);
            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<GetCountryInfoResponse>>(countryInfoKeyFormatted, CacheRegion.Global);
            Func<GetCountryInfoResponse> countryInfoCacheSaveFunction = delegate ()
            {
                var version = getCountryInfoRequest.CachedVersion;
                getCountryInfoRequest.CachedVersion = null;

                countryInfo = base.GetCountryInfo(agent, getCountryInfoRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<GetCountryInfoResponse>(countryInfo, CachePolicies.FullWeek);

                _cacheManager.Save(countryInfoKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);
                getCountryInfoRequest.CachedVersion = version;
                return countryInfo;
            };

            if (cachedResult.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<GetCountryInfoResponse>(countryInfoCacheSaveFunction, cachedResult.CachedObj.CacheMetadata);
                if (getCountryInfoRequest.CachedVersion != cachedResult.CachedObj.DataObject.Payload.Version)
                {
                    countryInfo = cachedResult.CachedObj.DataObject;
                }
                else
                {
                    countryInfo = new GetCountryInfoResponse {Payload = new GetCountryInfoResponsePayload {Version = getCountryInfoRequest.CachedVersion}};
                }
            }
            else
            {
                countryInfo = countryInfoCacheSaveFunction();
            }

            return countryInfo;
        }

        public override IndustryResponse Industry(Agent agent, IndustryRequest request)
        {
            IndustryResponse industriesResponse;
            var industryKeyFormatted = string.Format(CacheKeys.INDUSTRYKEY, agent.Language);
            var result = _cacheManager.Contains<CachedObjectResponseContainer<IndustryResponse>>(industryKeyFormatted, CacheRegion.Global);

            Func<IndustryResponse> industryCacheSaveFunction = delegate ()
            {
                industriesResponse = base.Industry(agent, request);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<IndustryResponse>(industriesResponse, CachePolicies.FullWeek);

                _cacheManager.Save(industryKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);
                return industriesResponse;
            };

            if (result.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<IndustryResponse>(industryCacheSaveFunction, result.CachedObj.CacheMetadata);
                industriesResponse = result.CachedObj.DataObject;
            }
            else
            {
                industriesResponse = industryCacheSaveFunction();
            }
            return industriesResponse;
        }

        public override ProfileResponse Profile(Agent agent, ProfileRequest profileRequest)
        {
            ProfileResponse agentProfile;
            var agentProfileKeyFormatted = string.Format(CacheKeys.AGENTPROFILEKEY, agent.AgentId, agent.AgentSequence);
            var cachedResult = _cacheManager.Contains<CachedObjectResponseContainer<ProfileResponse>>(agentProfileKeyFormatted, CacheRegion.Global);

            Func<ProfileResponse> profileCacheSaveFunction = delegate ()
            {
                Console.WriteLine("Entered profileCacheSaveFunction");
                agentProfile = base.Profile(agent, profileRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<ProfileResponse>(agentProfile, CachePolicies.FullWeek);
                _cacheManager.Save(agentProfileKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);
                return agentProfile;
            };

            if (cachedResult.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<ProfileResponse>(profileCacheSaveFunction, cachedResult.CachedObj.CacheMetadata);
                agentProfile = cachedResult.CachedObj.DataObject;
            }
            else
            {
                agentProfile = profileCacheSaveFunction();
            }
            return agentProfile;
        }
        public override GetAllFieldsResponse GetAllFields(Agent agent, GetAllFieldsRequest getAllFieldsRequest)
        {
            // GetAllFields key is TransactionType+Language+Target Audience
            // Product - GetAllFieldsRequest.TransactionType
            // Language - Request.Language
            // Target Audience - Request.TargetAudience (AGENT_FACING, CONSUMER_FACING, SERVICING)
            GetAllFieldsResponse allFieldsResponse;
            var allFieldsKeyFormatted = string.Format(CacheKeys.GETALLFIELDSKEY, getAllFieldsRequest.TransactionType, agent.Language, getAllFieldsRequest.TargetAudience);
            var result = _cacheManager.Contains<CachedObjectResponseContainer<GetAllFieldsResponse>>(allFieldsKeyFormatted, CacheRegion.Global);

            Func<GetAllFieldsResponse> allFieldsCacheSaveFunction = delegate ()
            {
                var version = getAllFieldsRequest.CachedVersion;
                getAllFieldsRequest.CachedVersion = null;

                allFieldsResponse = base.GetAllFields(agent, getAllFieldsRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<GetAllFieldsResponse>(allFieldsResponse, CachePolicies.FullWeek);
                if (!string.IsNullOrEmpty(allFieldsResponse.Payload.Version) && allFieldsResponse.Payload.Infos.Any())
                {
                    _cacheManager.Save(allFieldsKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);
                }

                getAllFieldsRequest.CachedVersion = version;
                return allFieldsResponse;
            };

            if (result.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<GetAllFieldsResponse>(allFieldsCacheSaveFunction, result.CachedObj.CacheMetadata);
                if (getAllFieldsRequest.CachedVersion != result.CachedObj.DataObject.Payload.Version)
                {
                    allFieldsResponse = result.CachedObj.DataObject;
                }
                else
                {
                    allFieldsResponse = new GetAllFieldsResponse {Payload = new GetAllFieldsResponsePayload {Version = getAllFieldsRequest.CachedVersion}};
                }
            }
            else
            {

                allFieldsResponse = allFieldsCacheSaveFunction();
            }
            return allFieldsResponse;
        }

        public override GetEnumerationsResponse GetEnumerations(Agent agent, GetEnumerationsRequest getEnumerationsRequest)
        {
            // GetEnumerations key is Language+EnumerationName
            // Language - Request.Language
            // EnumerationName- Request.EnumerationName (NAME_SUFFIX, OCCUPATION, PERSONAL_ID2_TYPE)
            GetEnumerationsResponse enumerationsResponse;
            var enumerationsKeyFormatted = string.Format(CacheKeys.GETENUMERATIONSKEY, agent.Language, getEnumerationsRequest.EnumerationName);
            var result = _cacheManager.Contains<CachedObjectResponseContainer<GetEnumerationsResponse>>(enumerationsKeyFormatted, CacheRegion.Global);
            Func<GetEnumerationsResponse> enumerationsCacheSaveFunction = delegate ()
            {
                var version = getEnumerationsRequest.CachedVersion;
                getEnumerationsRequest.CachedVersion = null;

                enumerationsResponse = base.GetEnumerations(agent, getEnumerationsRequest);
                var CachedContainer = CacheAheadHelper.PopulateCacheMetadata<GetEnumerationsResponse>(enumerationsResponse, CachePolicies.FullWeek);
                enumerationsResponse = base.GetEnumerations(agent, getEnumerationsRequest);
                if (!string.IsNullOrEmpty(enumerationsResponse.Payload.Version) && enumerationsResponse.Payload.Enumerations.Any())
                {
                    _cacheManager.Save(enumerationsKeyFormatted, CachedContainer, CacheRegion.Global, CachePolicies.FullWeek);
                }

                getEnumerationsRequest.CachedVersion = version;
                return enumerationsResponse;
            };

            if (result.Exists)
            {
                CacheAheadHelper.ExecuteCacheAheadProcess<GetEnumerationsResponse>(enumerationsCacheSaveFunction, result.CachedObj.CacheMetadata);
                if (getEnumerationsRequest.CachedVersion != result.CachedObj.DataObject.Payload.Version)
                {
                    enumerationsResponse = result.CachedObj.DataObject;
                }
                else
                {
                    enumerationsResponse = new GetEnumerationsResponse { Payload = new GetEnumerationsResponsePayload { Version = getEnumerationsRequest.CachedVersion } };
                }
            }
            else
            {
                enumerationsResponse = enumerationsCacheSaveFunction();
            }
            return enumerationsResponse;
        }
        private Dictionary<string, T> ConvertToDictionary<T>(List<KeyValueItem<T>> itemList) where T : class
        {
            var result = new Dictionary<string, T>();
            foreach(var item in itemList)
            {
                result.Add(item.Key, item.Val);
            }
            return result;
        }

        private List<KeyValueItem<T>> ConvertToList<T>(Dictionary<string, T> itemsDictionary) where T : class
        {
            var result = new List<KeyValueItem<T>>();
            foreach(var item in itemsDictionary)
            {
                result.Add(new KeyValueItem<T> {Key = item.Key, Val = item.Value});
            }
            return result;
        }
    }
}