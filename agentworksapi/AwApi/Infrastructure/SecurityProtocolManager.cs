using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace AwApi.Infrastructure
{
    public static class SecurityProtocolManager
    {
        public static void SetSecurityProtocol()
        {
            if(ConfigurationManager.AppSettings.AllKeys.Contains("SecurityProtocolType"))
            {
                SecurityProtocolType defaultSecurityProtocolType = SecurityProtocolType.Tls12;
                SecurityProtocolType sslSetting;
                var parseSuccess = Enum.TryParse(ConfigurationManager.AppSettings["SecurityProtocolType"], true, out sslSetting);
                ServicePointManager.SecurityProtocol = parseSuccess ? sslSetting : defaultSecurityProtocolType;
            }
        }
    }
}