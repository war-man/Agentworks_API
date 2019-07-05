using System;
using System.Collections.Generic;

namespace MoneyGram.Common.Cache.Couchbase
{
    public interface ICouchbaseCacheConfig
    {
        string AdminPassword { get; set; }
        string AdminUsername { get; set; }
        string BucketName { get; set; }
        List<Uri> Hosts { get; set; }
        string Password { get; set; }
        string Username { get; set; }
    }
}