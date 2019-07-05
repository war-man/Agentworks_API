using MoneyGram.Common.Models;
using System;
using System.Collections.Generic;

namespace MoneyGram.Common.Cache.InMemory
{
    public class InMemoryCacheManager : ICacheManager
    {
        private Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(string key, CacheRegion region)
        {
            if (_dictionary.ContainsKey(key))
            {
                var cachedItem = _dictionary[key];
                // Check for expiration
                System.Reflection.PropertyInfo pi = cachedItem.GetType().GetProperty(nameof(CachedObjectResponseContainer<object>.CacheMetadata));
                if (pi != null)
                {
                    CachedObjectMetadata metaData = (CachedObjectMetadata)(pi.GetValue(cachedItem, null));
                    if (metaData != null && metaData.ExpirationDate <= DateTime.Now)
                    {
                        return false;
                    }
                }
                else
                {
                    // We are unable to evaluate expiration date, so we will always do live call.
                    return false;
                }
                return cachedItem != null;
            }
            return false;
        }

        public ContainsResult<T> Contains<T>(string key, CacheRegion region) where T : class
        {
            var containsResult = new ContainsResult<T>();
            containsResult.Exists = Contains(key, region);
            if (containsResult.Exists)
            {
                containsResult.CachedObj = Get<T>(key, region);
            }

            return containsResult;
        }

        public T Get<T>(string key, CacheRegion region) where T : class
        {
            object cachedObj;
            var cachedItem = _dictionary.TryGetValue(key, out cachedObj) == true ? cachedObj as T : null;
            return cachedItem;
        }

        public bool Remove(string key, CacheRegion region)
        {
            if (!Contains(key, region))
            {
                return false;
            }

            _dictionary.Remove(key);
            return true;
        }

        public void Save(string key, object toSave, CacheRegion region)
        {
            // non specified timeout is hardcoded for 24 hours.
            Save(key, toSave, region, CachePolicies.Default);
        }

        public void Save(string key, object toSave, CacheRegion region, TimeSpan expiration)
        {
            _dictionary[key] = toSave;
        }
        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                Message = "InMemory Health Check - Success",
                ServiceName = ServiceNames.Cache,
                StatusCode = StatusCode.Ok
            };
        }
    }
}