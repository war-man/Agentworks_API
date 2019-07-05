namespace AwApi.Auth
{
    // This is temporary and should be removed ASAP
    public static class AuthCacheKeys
    {
        public static string AGENTPROFILEKEY => "AgentProfile.{0}.{1}";
        public static string AGENTPASSWORDKEY => "AgentPassword.{0}.{1}";

        // Claims related keys
        public static string AUTHCLAIMS => "AuthClaims.{0}";
        public static string AUTHROLES => "AuthRoles.{0}";
        public static string CAMSCLAIMS =>"CamsClaims.{0}";
        public static string AGENTPROFILECLAIMS => "AgentProfileClaims.{0}";
        public static string PARTNERSERVICECLAIMS => "PartnerServiceClaims.{0}";
    }
}