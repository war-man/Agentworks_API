using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using MoneyGram.Common.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MoneyGram.Common.Cache.Couchbase
{
    public class CouchbaseCacheManager : ICacheManager
    {
        private Cluster cluster;
        private BinaryFormatter formatter;
        private IBucket bucket;
        public CouchbaseCacheManager(ICouchbaseCacheConfig model)
        {
            formatter = new BinaryFormatter();
            var authenticator = new PasswordAuthenticator(model.Username, model.Password);
            cluster = new Cluster(new ClientConfiguration
            {
                Servers = model.Hosts
            });
            cluster.Authenticate(authenticator);
            bucket = cluster.OpenBucket(model.BucketName);
        }
        ~CouchbaseCacheManager()
        {
            // Remove resources
            if(bucket != null)
            {
                bucket.Dispose();
            }
            if(cluster != null)
            {
                cluster.Dispose();
            }
        }
        public void Clear()
        {

            throw new NotImplementedException("Can only be flushed manually contact couchbase admin");
        }

        public bool Contains(string key, CacheRegion region)
        {
            return bucket.ExistsAsync(key).Result;
        }

        public ContainsResult<T> Contains<T>(string key, CacheRegion region) where T : class
        {
            var containsResult = new ContainsResult<T>();
            try
            {
                var retrievedObject = Get<T>(key, region);

                containsResult.CachedObj = retrievedObject;
                containsResult.Exists = containsResult.CachedObj != null;
            }
            catch (Exception ex)
            {
                // Try to remove key just in case its corrupt
                Remove(key, region);

                containsResult.Exists = false;
                containsResult.CachedObj = null;
            }
            return containsResult;
        }

        public T Get<T>(string key, CacheRegion region) where T : class
        {
            // Was advised by Couchbase vendor to use ASYNC api in synchronous manor vs using the synchronous call
            // Said it was more optimized. Just so you know.
            var bytesBackOut = bucket.GetAsync<byte[]>(key).Result.Value;
            if (bytesBackOut == null)
            {
                return null;
            }
            using (var stream = new MemoryStream(bytesBackOut))
            {
                var retrievedObject = (T)formatter.Deserialize(stream);
                return retrievedObject;
            }
        }

        public Common.Models.HealthCheckResponse HealthCheck()
        {
            var response = new HealthCheckResponse();
            response.Message = "Couchbase Health Check - Fail";
            response.StatusCode = StatusCode.InternalServerError;
            response.ServiceName = ServiceNames.Cache;
            try
            {
                var healthCheckKey = "Couchbase.HealthCheck";
                var toSave = "HealthCheck";
                // Save
                Save(healthCheckKey, toSave, CacheRegion.Global);
                // Check if saved
                var healthCheckCacheResult = Contains<string>(healthCheckKey, CacheRegion.Global);
                if (healthCheckCacheResult.Exists && healthCheckCacheResult.CachedObj == toSave)
                {
                    // Delete, sucess.
                    Remove(healthCheckKey, CacheRegion.Global);
                    response.Message = "Couchbase Health Check - Success";
                    response.StatusCode = StatusCode.Ok;
                    response.ServiceName = ServiceNames.Cache;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Couchbase Health Check - Fail | {ex.Message},";
                response.StatusCode = StatusCode.InternalServerError;
                response.ServiceName = ServiceNames.Cache;
                return response;
            }
            // Failed
            return response;
        }

        public bool Remove(string key, CacheRegion region)
        {
            // In case of CAS (optimistic concurrency is set) 
            // var loaded = bucket.GetDocument<dynamic>("document_id");
            // var removed = bucket.Remove(loaded);

            var result = bucket.RemoveAsync(key).Result;
            return result.Success;
        }

        public void Save(string key, object toSave, CacheRegion region)
        {
            Save(key, toSave, CacheRegion.Global, CachePolicies.Default);
        }

        public void Save(string key, object toSave, CacheRegion region, TimeSpan expiration)
        {
            // Test if object is serializable first
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    formatter.Serialize(mem, toSave);
                    bucket.InsertAsync<byte[]>(key, mem.ToArray(), expiration);
                }
            }
            catch (Exception ex)
            {
                // Object is not serializable
                return;
            }

        }
    }
}