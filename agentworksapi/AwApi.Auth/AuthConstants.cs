namespace AwApi.Auth
{
    public static class AuthConstants
    {
        public static string ClaimsIdentityName => "OpenIdConnect";
        public static string PrincipalIdentityName => "PrincipalIdentity";
        public static string OpenAm => "OpenAm";
        public static string OpenAmDevice => "OpenAmDevice";
        public static string OpenAmUnregisteredDevice => "OpenAmUnregisteredDevice";
        public static string ApiKeyAuth => "ApiKeyAuth";
        public static string FakeAuth => "FakeAuth";
        public static string SupportAuth => "SupportAuth";
    }
}