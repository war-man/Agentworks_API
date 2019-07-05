using System.Configuration;
using System.Linq;

namespace AwApi.Cache
{
    public static class CacheKeys
    {
        private const string AWAPI = "Aw.Api.";

        // Language only params.
        public static string COUNTRYSUBDIVISIONKEY => CacheEnvironmentPrepend() + "CountrySubdivision.{0}";
        public static string CURRENCYINFOKEY => CacheEnvironmentPrepend() + "CurrencyInfoKey.{0}";
        public static string INDUSTRYKEY => CacheEnvironmentPrepend() + "IndustryKey.{0}";
        public static string COUNTRYINFOKEY => CacheEnvironmentPrepend() + "CountryInfoKey.{0}";

        // Agent and sequence params.
        public static string AGENTPROFILEKEY => CacheEnvironmentPrepend() + "AgentProfile.{0}.{1}";
        public static string GETALLFIELDSKEY => CacheEnvironmentPrepend() + "GetAllFields.{0}.{1}.{2}";
        public static string GETENUMERATIONSKEY => CacheEnvironmentPrepend() + "GetEnumerations.{0}.{1}";

        // PartnerService related keys
        public static string AGENTPASSWORDKEY => CacheEnvironmentPrepend() + "AgentPassword.{0}.{1}";

        // Partner hierarchy related keys
        public static string PARTNERHIERARCHYKEY => CacheEnvironmentPrepend() + "PartnerHierarchy.{0}.{1}";

        // Claims related keys
        public static string AUTHCLAIMS => CacheEnvironmentPrepend() + "AuthClaims.{0}";
        public static string AUTHROLES => CacheEnvironmentPrepend() + "AuthRoles.{0}";
        public static string CAMSCLAIMS => CacheEnvironmentPrepend() + "CamsClaims.{0}";
        public static string AGENTPROFILECLAIMS => CacheEnvironmentPrepend() + "AgentProfileClaims.{0}";
        public static string PARTNERSERVICECLAIMS => CacheEnvironmentPrepend() + "PartnerServiceClaims.{0}";


        /// <summary>
        /// This is for prepending to cache keys to ensure AWAPI cache keys are unique across environments
        /// </summary>
        /// <returns></returns>
        public static string CacheEnvironmentPrepend()
        {
            var keyName = "ServicesEnvironment";
            var environment = ConfigurationManager.AppSettings.AllKeys.Contains(keyName) ? ConfigurationManager.AppSettings[keyName] : string.Empty;
            return $"{AWAPI}{environment}.";
        }
    }
}