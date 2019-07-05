using System;

namespace MoneyGram.Common.Cache.IntegrationTest
{
    public static class CacheIntegrationTestHelper
    {
        public static TimeSpan CacheTimeout = new TimeSpan(0,1,0);
        public static string GetRandomCacheKey()
        {
            return string.Format("CacheIntegration.{0}", Guid.NewGuid());
        }
    }
}