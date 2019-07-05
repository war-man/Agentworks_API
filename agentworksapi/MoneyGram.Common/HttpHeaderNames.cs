namespace MoneyGram.Common
{
    public static class HttpHeaderNames
    {
        public static string Authorization => "Authorization";
        public static string AcceptLanguage => "Accept-Language";
        public static string Otp => "x-moneygram-transactional-otp";
        public static string MgiSsoSession => "mgiSsoSession";
        public static string MgiDeviceSessionUnregistered => "mgiDeviceSessionUnregistered";
        public static string MgiSupport => "mgiSupport";
    }
}