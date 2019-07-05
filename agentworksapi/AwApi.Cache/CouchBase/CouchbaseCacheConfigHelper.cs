using MoneyGram.Common.Cache.Couchbase;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace AwApi.Cache.CouchBase
{
    public class CouchbaseCacheConfigHelper
    {
        public static bool ValidateConfigs()
        {
            var couchBaseConfigSection = (NameValueCollection)ConfigurationManager.GetSection("couchbaseConfig");
            if(couchBaseConfigSection != null && couchBaseConfigSection.Count > 0)
            {
                return !string.IsNullOrWhiteSpace(couchBaseConfigSection[CouchbaseConfigKeyConstants.BUCKETNAME]) &&
                !string.IsNullOrWhiteSpace(couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEHOST]) &&
                !string.IsNullOrWhiteSpace(couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEUSERNAME]) &&
                !string.IsNullOrWhiteSpace(couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEPASSWORD]);
            }
            else
            {
                return false;
            }
        }

        public static CouchbaseCacheConfig GetConfigModel()
        {
            var couchBaseConfigSection = (NameValueCollection)ConfigurationManager.GetSection("couchbaseConfig");
            var bucketName = couchBaseConfigSection[CouchbaseConfigKeyConstants.BUCKETNAME];
            // Raw Config values, requires processing
            var host = couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEHOST];
            var username = couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEUSERNAME];
            var password = couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEPASSWORD];

            var adminUser = couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEADMINUSERNAME];
            var adminPass = couchBaseConfigSection[CouchbaseConfigKeyConstants.COUCHBASEADMINPASSWORD];

            var model = new CouchbaseCacheConfig();
            model.BucketName = bucketName;
            var serversArray = host.Split(';');
            var serversUriList = serversArray.Select(x => new Uri(x)).ToList();
            model.Hosts = serversUriList;
            model.Username = username;
            model.Password = password;
            model.AdminPassword = adminPass;
            model.AdminUsername = adminUser;
            return model;
        }
    }
}