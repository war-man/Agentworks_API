using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.Common.Cache.InMemory;

namespace MoneyGram.Common.Cache.IntegrationTest
{
    [TestClass]
    public class InMemoryTest
    {
        private ICacheManager _InMemeoryCacheManager = new InMemoryCacheManager();

        [TestMethod]
        public void Save()
        {
            var cacheKey = CacheIntegrationTestHelper.GetRandomCacheKey();
            var cacheData = CacheAheadHelper.PopulateCacheMetadata<string>("Test", CachePolicies.FourHours);
            _InMemeoryCacheManager.Save(cacheKey, cacheData, CacheRegion.Global);
            var sucessfulSaveResult = _InMemeoryCacheManager.Contains<string>(cacheKey, CacheRegion.Global);
            Assert.IsTrue(sucessfulSaveResult.Exists);
        }

        [TestMethod]
        public void Remove()
        {
            var cacheKey = CacheIntegrationTestHelper.GetRandomCacheKey();
            var cacheData = CacheAheadHelper.PopulateCacheMetadata<string>("Test", CachePolicies.FourHours);
            _InMemeoryCacheManager.Save(cacheKey, cacheData, CacheRegion.Global);
            var sucessfulSaveResult = _InMemeoryCacheManager.Contains(cacheKey, CacheRegion.Global);
            var sucessfulRemoveResult = _InMemeoryCacheManager.Remove(cacheKey, CacheRegion.Global);
            Assert.IsTrue(sucessfulSaveResult);
            Assert.IsTrue(sucessfulRemoveResult);

        }

        [TestMethod]
        public void Contains()
        {
            var cacheKey = CacheIntegrationTestHelper.GetRandomCacheKey();
            var cacheData = CacheAheadHelper.PopulateCacheMetadata<string>("Test", CachePolicies.FourHours);
            _InMemeoryCacheManager.Save(cacheKey, cacheData, CacheRegion.Global);
            var sucessfulSaveResult = _InMemeoryCacheManager.Contains<CachedObjectResponseContainer<string>>(cacheKey, CacheRegion.Global);
            Assert.IsTrue(sucessfulSaveResult.Exists);
            Assert.IsTrue(sucessfulSaveResult.CachedObj is CachedObjectResponseContainer<string>);
            Assert.IsTrue((sucessfulSaveResult.CachedObj as CachedObjectResponseContainer<string>).DataObject.Equals("Test"));
        }
    }
}