using System.Linq;

namespace AwApi.Auth
{
    public static class DeviceSecurityLevel
    {
        public const string Ten = "10";
        public const string Twenty = "20";
        public const string Thirty = "30";
        public const string Fourty = "40";
        public const string Fifty = "50";

        public const string Any = "ANY";
        public const string Registered = "REGISTERED";
        public const string Transactional = "TRANSACTIONAL";

        public static string GetApplicationDefinedDevices(string key)
        {
            switch (key)
            {
                case Any:
                    return $"{Ten},{Twenty},{Thirty},{Fourty},{Fifty}";
                case Registered:
                    return $"{Thirty},{Fourty},{Fifty}";
                case Transactional:
                    return $"{Fourty},{Fifty}";
                default:
                    return key;
            }
        }

        public static bool MatchesSecurityLevel(string securityLevel, string claimValue)
        {
            var securityLevels = GetApplicationDefinedDevices(securityLevel).Split(',');
            return securityLevels.Contains(claimValue);
        }
    }
}