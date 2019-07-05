using System;
using System.Configuration;

namespace MoneyGram.Common.Cache
{
    public static class CachePolicies
    {
        public static readonly TimeSpan Default = new TimeSpan(0, 24, 0, 0, 0);
        public static readonly TimeSpan FullDay = new TimeSpan(0, 24, 0, 0, 0);
        public static readonly TimeSpan FullWeek = new TimeSpan(0, 168, 0, 0, 0);
        public static readonly TimeSpan FourHours = new TimeSpan(0, 4, 0, 0, 0);
    }
}