using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace AwApi.ViewModels
{
    public static class MgiAwRole
    {
        // These are CAMS roles.
        public const string Teller = "APP - AgentWorks - Teller";
        public const string Manager = "APP - AgentWorks - Manager";
        public const string Admin = "APP - AgentWorks - Administrator";
        public const string TellerTraining = "APP - AgentWorks - Teller Training Mode";

        // Application defined role combinations.
        public const string TellerManagerAdmin = "TellerManagerAdmin";

        public const string Dt4Device = "Dt4 device";
        public const string Dt4UnregisteredDevice = "Dt4 unregistered device";

        // Application defined role for Support controller functionality
        // USE FOR diagnostics/troubleshooting only
        public const string Support = "AgentWorks - Support";

        // Application defined role for Dt4Support to use EventController
        // USE FOR Event Logging ONLY
        public const string Dt4Support = "AgentWorks - DT4 Support";

        public static string GetApplicationDefinedRoles(string key)
        {
            switch(key)
            {
                case TellerManagerAdmin:
                    return $"{Teller},{Manager},{Admin},{TellerTraining}";
                default:
                    return key;
            }            
        }

        public static bool IsInTransactionalRole(List<Claim> claims)
        {
            var roles = claims.Where(claim => claim.Type.Contains("role")).ToList();
            return roles.Any(r => string.Compare(r.Value, Teller, true, CultureInfo.InvariantCulture) == 0);
        }

    }
}