using AwApi.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AwApi.Auth
{
    public class FakeAuthInfoVm : AuthClaimsVm
    {
        public string MainOfficeId { get; set; }
        public string AgentId { get; set; }
        public string Pos { get; set; }
        public string Password { get; set; }

        public FakeAuthInfoVm() : base()
        {
        }

        public AuthClaimsVm ToAuthClaimsVm()
        {
            // device info
            this.MgiAgentLocationId = this.MgiAgentLocationId ?? this.AgentId;
            this.MgiDeviceAgentId = this.MgiDeviceAgentId ?? this.AgentId;
            this.MgiDevicePosNumber = this.MgiDevicePosNumber ?? this.Pos;
            this.MgiMainOfficeId = this.MgiMainOfficeId ?? this.MainOfficeId;
            this.AgentPassword = this.Password;
            this.MgiPosUnitProfileId = this.MgiPosUnitProfileId ?? "1234567";

            // transactional limits
            this.MgiAppAwMaxAmendAmount = this.MgiAppAwMaxAmendAmount ?? "3000";
            this.MgiAppAwMaxBillPayAmount = this.MgiAppAwMaxBillPayAmount ?? "3000";
            this.MgiAppAwMaxReceiveAmount = this.MgiAppAwMaxReceiveAmount ?? "3000";
            this.MgiAppAwMaxReceiveCancellationAmount = this.MgiAppAwMaxReceiveCancellationAmount ?? "3000";
            this.MgiAppAwMaxSendAmount = this.MgiAppAwMaxSendAmount ?? "3000";
            this.MgiAppAwMaxSendCancellationAmount = this.MgiAppAwMaxSendCancellationAmount ?? "3000";
            this.MgiAppAwMaxMoneyOrderAmount = this.MgiAppAwMaxMoneyOrderAmount ?? "3000";
            this.MgiAppAwMaxVendorPaymentAmount = this.MgiAppAwMaxVendorPaymentAmount ?? "3000";

            // misc claims
            this.Sub = this.Sub ?? $"user{Pos}";
            this.MgiUserLastLoginTS = this.MgiUserLastLoginTS ?? "20170605140504.933";
            this.Given_Name = this.Given_Name ?? "User";
            this.Country = this.Country ?? "USA";
            this.Updated_At = this.Updated_At ?? "0";
            this.Email = this.Email ?? $"user{Pos}@moneygram.com";
            this.Name = this.Name ?? $"user{Pos}";
            this.Family_Name = this.Family_Name ?? $"POS{Pos}";
            this.Language = this.Language ?? "en-US";
            this.DSL = this.DSL ?? DeviceSecurityLevel.Fourty;
            this.HostVersionEOLDate = this.HostVersionEOLDate ?? "12-12-4000";
            this.PlatformVersionEOLDate = this.PlatformVersionEOLDate ?? "12-12-4000";
            this.MgiAppAwRoles = this.MgiAppAwRoles.Any() ?  this.MgiAppAwRoles : new List<string>()
            {
                MgiAwRole.Teller,
                MgiAwRole.Manager
            };

            return this;
        }
    }
}