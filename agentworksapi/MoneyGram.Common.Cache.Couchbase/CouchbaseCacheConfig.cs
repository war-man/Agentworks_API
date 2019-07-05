
using System;
using System.Collections.Generic;

namespace MoneyGram.Common.Cache.Couchbase
{
    public class CouchbaseCacheConfig : ICouchbaseCacheConfig
    {
        public string BucketName { get; set; }
        public List<Uri> Hosts { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
    }
}