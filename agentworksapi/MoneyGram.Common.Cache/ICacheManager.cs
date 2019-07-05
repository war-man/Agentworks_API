using MoneyGram.Common.Models;
using System;

namespace MoneyGram.Common.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key, CacheRegion region) where T : class;
        void Save(string key, object toSave, CacheRegion region);
        void Save(string key, object toSave, CacheRegion region, TimeSpan expiration);
        bool Contains(string key, CacheRegion region);
        ContainsResult<T> Contains<T>(string key, CacheRegion region) where T : class;
        bool Remove(string key, CacheRegion region);
        void Clear();
        HealthCheckResponse HealthCheck();
    }
}
