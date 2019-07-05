using System;
using System.Threading;

namespace MoneyGram.Common.Cache
{
    public static class CacheAheadHelper
    {
        public static readonly decimal cacheAheadExpirationPercentage = 0.5m;
        public static CachedObjectResponseContainer<T> PopulateCacheMetadata<T>(T cachedObj, TimeSpan cachePolicy) where T : class
        {
            var cachedContainer = new CachedObjectResponseContainer<T>();
            cachedContainer.DataObject = cachedObj;
            cachedContainer.CacheMetadata = new CachedObjectMetadata(cachePolicy);
            return cachedContainer;
        }
        public static void ExecuteCacheAheadProcess<T>(Func<T> acFunction, CachedObjectMetadata metaData) where T : class
        {
            var cachePolicyTimeSpanPercentage = TimeSpan.FromTicks((long)(metaData.CachePolicy.Ticks * cacheAheadExpirationPercentage));
            if (metaData.ExpirationDate < DateTime.Now.Add(cachePolicyTimeSpanPercentage) && metaData.CacheAhead)
            {
                // Execute Cache Ahead.
                Thread thread = new Thread(() => acFunction());
                thread.Start();
            }
        }
    }
}