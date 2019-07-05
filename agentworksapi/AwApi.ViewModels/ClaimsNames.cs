namespace AwApi.ViewModels
{
    public static class ClaimsNames
    {
        public static string Token => "token";
        
        // CAMS for User
        public static string MgiAppAwMaxSendAmount => "mgiAppAwMaxSendAmount";
        public static string MgiAppAwMaxBillPayAmount => "mgiAppAwMaxBillPayAmount";
        public static string MgiAppAwMaxReceiveAmount => "mgiAppAwMaxReceiveAmount";
        public static string MgiAppAwMaxSendCancellationAmount => "mgiAppAwMaxSendCancellationAmount";
        public static string MgiAppAwMaxReceiveCancellationAmount => "mgiAppAwMaxReceiveCancellationAmount";
        public static string MgiAppAwMaxAmendAmount => "mgiAppAwMaxAmendAmount";
        public static string MgiAppAwMaxMoneyOrderAmount => "mgiAppAwMaxMoneyOrderAmount";
        public static string MgiAppAwMaxVendorPaymentAmount => "mgiAppAwMaxVendorPaymentAmount";
        public static string Sub => "sub";
        public static string MgiMainOfficeId => "mgiMainOfficeId";
        public static string MgiAgentLocationId => "mgiAgentLocationId";  // All locations for which I am allowed to act
        public static string MgiUserLastLoginTs => "mgiUserLastLoginTS";
        public static string GivenName => "given_name";
        public static string Country => "country";
        public static string UpdatedAt => "updated_at";
        public static string Email => "email";
        public static string Name => "name";
        public static string FamilyName => "family_name";
        public static string Language => "language";
        public static string MgiAppAwDualControlAllowed => "mgiAppAwDualControlAllowed";
        public static string Roles => "mgiAppAwRoles";

        // CAMS for Device
        public static string MgiDeviceId => "mgiDeviceId";

        public static string MgiDevicePosUnitProfileId => "mgiDevicePosUnitProfileId";
        public static string MgiDevicePosNumber => "mgiDevicePosNumber"; // The machine I am logged into
        public static string MgiDeviceAgentLocationId => "mgiDeviceAgentLocationId";     // The location I logged in at
        public static string MgiDeviceStatus => "deviceStatus";
        public static string Platform => "platform";
        public static string PlatformVersionEOLDate => "platformVersionEOLDate";
        public static string HostVersionEOLDate => "hostVersionEOLDate";
        public static string PlatformVersion => "platformVersion";
        public static string HostVersion => "hostVersion";
        public static string DSL => "dsl";

        // Partner Service
        public static string AgentPassword => "agentPassword";

        // Agent Profile
        public static string AgentName => "agentName";
        public static string StoreName => "storeName";
        public static string AgentTimeZone => "agentTimeZone";
        public static string AgentTelNo => "agentTelNo";
        public static string PrimaryReceiptLanguage => "primaryReceiptLanguage";
        public static string SecondaryReceiptLanguage => "secondaryReceiptLanguage";
        public static string CanSend => "canSend";
        public static string CanSendCompletion => "canSendCompletion";
        public static string CanAmend => "canAmend";
        public static string CanCancel => "canCancel";
        public static string CanSendReversal => "canSendReversal";
        public static string CanFeeRefund => "canFeeRefund";
        public static string CanReceive => "canReceive";
        public static string CanReceiveCompletion => "canReceiveCompletion";
        public static string CanReceiveReversal => "canReceiveReversal";
        public static string CanMoneyOrder => "canMoneyOrder";
        public static string CanVendorPay => "canVendorPay";
        public static string CanBillPay => "canBillPay";
        public static string CanExpressPayment => "canExpressPayment";
        public static string CanUtilityBillPay => "canUtilityBillPay";
        public static string CanLoadPrepaidCard => "canLoadPrepaidCard";
        public static string CanBillPayCompletion => "canBillPayCompletion";
        
        // Owin Context Claims
        public static string RemoteIpAddress => "remoteIpAddress";
        public static string PrincipalType => "principalType";
        public static string Otp => "otp";

        // Application derived Claims
        public static string DeviceType => "deviceType";
    }
}