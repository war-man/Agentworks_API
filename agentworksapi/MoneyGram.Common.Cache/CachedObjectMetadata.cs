using System;

namespace MoneyGram.Common.Cache
{
    [Serializable]
    public class CachedObjectMetadata
    {
        public CachedObjectMetadata(TimeSpan cachePolicy, bool cacheAhead = true)
        {
            CacheAhead = cacheAhead;
            SaveDate = DateTime.Now;
            ExpirationDate = DateTime.Now.Add(cachePolicy);
            CachePolicy = cachePolicy;
        }
        public bool CacheAhead { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime SaveDate { get; set; }
        public TimeSpan CachePolicy { get; set; }
    }
}