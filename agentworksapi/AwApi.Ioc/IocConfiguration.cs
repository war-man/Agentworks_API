using MoneyGram.Common.Cache;
using System.Collections.Generic;

namespace AwApi.Ioc
{
    public class IocConfiguration
    {
        public CacheTypeEnum CacheType { get; set; }
        public List<string> RegexStringList {get; set;}
    }
}