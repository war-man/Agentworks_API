using MoneyGram.Common.Models;
using System;

namespace MoneyGram.Common.Cache.NoCache
{
    public class NoCacheManager : ICacheManager
    {
        public void Clear()
        {
            
        }

        public bool Contains(string key, CacheRegion region)
        {
            return false;
        }

        public ContainsResult<T> Contains<T>(string key, CacheRegion region) where T : class
        {
            return new ContainsResult<T>
            {
                CachedObj = null,
                Exists = false
            };
        }

        public T Get<T>(string key, CacheRegion region) where T : class
        {
            return null;
        }

        public bool Remove(string key, CacheRegion region)
        {
            return true;
        }

        public void Save(string key, object toSave, CacheRegion region)
        {
            
        }

        public void Save(string key, object toSave, CacheRegion region, TimeSpan expiration)
        {
            
        }
        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                Message = "NoCache Health Check - Success",
                ServiceName = ServiceNames.Cache,
                StatusCode = StatusCode.Ok
            };
        }
    }
}