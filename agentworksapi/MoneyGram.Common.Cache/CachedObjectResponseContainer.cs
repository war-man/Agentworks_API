using System;

namespace MoneyGram.Common.Cache
{
    [Serializable]
    public class CachedObjectResponseContainer<T> where T : class
    {
        public T DataObject { get; set; }
        public CachedObjectMetadata CacheMetadata { get; set; }
    }
}