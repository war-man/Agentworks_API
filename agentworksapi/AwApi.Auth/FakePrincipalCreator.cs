using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using AwApi.ViewModels;

namespace AwApi.Auth
{
    public abstract class FakePrincipalCreator
    {
        protected readonly IAgentProfilePrincipalService agentConnectPrincipalService;

        public FakePrincipalCreator(IAgentProfilePrincipalService agentConnectPrincipalService)
        {
            this.agentConnectPrincipalService = agentConnectPrincipalService;
        }

        public IPrincipal Create(IDictionary<string, AuthClaimsVm> agents, string token)
        {
            if (!agents.ContainsKey(token))
            {
                return null;
            }

            var authVm = agents[token];
            var agentProfileClaims = agentConnectPrincipalService.GetAgentProfileClaims(authVm.MgiDeviceAgentId, authVm.MgiDevicePosNumber, authVm.AgentPassword, authVm.Language, token);

            var claims = new List<Claim>()
                .TryAddClaim(ClaimsNames.AgentPassword, authVm.AgentPassword)
                .TryAddClaim(ClaimsNames.AgentName, authVm.AgentName)
                .TryAddClaim(ClaimsNames.StoreName, authVm.AgentName)
                .TryAddClaim(ClaimsNames.AgentTimeZone, authVm.AgentTimeZone)
                .TryAddClaim(ClaimsNames.AgentTelNo, authVm.AgentTelNo)
                .TryAddClaim(ClaimsNames.PrimaryReceiptLanguage, authVm.PrimaryReceiptLanguage)
                .TryAddClaim(ClaimsNames.SecondaryReceiptLanguage, authVm.SecondaryReceiptLanguage)
                .TryAddClaim(ClaimsNames.Token, authVm.Token)
                .TryAddClaim(ClaimsNames.Sub, authVm.Sub)
                .TryAddClaim(ClaimsNames.MgiAgentLocationId, authVm.MgiAgentLocationId)
                .TryAddClaim(ClaimsNames.MgiUserLastLoginTs, authVm.MgiUserLastLoginTS)
                .TryAddClaim(ClaimsNames.GivenName, authVm.Given_Name)
                .TryAddClaim(ClaimsNames.MgiDevicePosNumber, authVm.MgiDevicePosNumber)
                .TryAddClaim(ClaimsNames.MgiDeviceAgentLocationId, authVm.MgiDeviceAgentId)
                .TryAddClaim(ClaimsNames.UpdatedAt, authVm.Updated_At)
                .TryAddClaim(ClaimsNames.MgiMainOfficeId, authVm.MgiMainOfficeId)
                .TryAddClaim(ClaimsNames.Email, authVm.Email)
                .TryAddClaim(ClaimsNames.Name, authVm.Name)
                .TryAddClaim(ClaimsNames.FamilyName, authVm.Family_Name)
                .TryAddClaim(ClaimsNames.Language, authVm.Language)
                .TryAddClaim(ClaimsNames.MgiDevicePosUnitProfileId, authVm.MgiPosUnitProfileId)
                .TryAddClaim(ClaimsNames.DSL, authVm.DSL)
                .TryAddClaim(ClaimsNames.HostVersionEOLDate, authVm.HostVersionEOLDate)
                .TryAddClaim(ClaimsNames.PlatformVersionEOLDate, authVm.PlatformVersionEOLDate)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxSendAmount, authVm.MgiAppAwMaxSendAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxSendCancellationAmount, authVm.MgiAppAwMaxSendCancellationAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxReceiveCancellationAmount, authVm.MgiAppAwMaxReceiveCancellationAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxAmendAmount, authVm.MgiAppAwMaxAmendAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxReceiveAmount, authVm.MgiAppAwMaxReceiveAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxBillPayAmount, authVm.MgiAppAwMaxBillPayAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxMoneyOrderAmount, authVm.MgiAppAwMaxMoneyOrderAmount)
                .TryAddClaim(ClaimsNames.MgiAppAwMaxVendorPaymentAmount, authVm.MgiAppAwMaxVendorPaymentAmount);

            claims.AddRange(agentProfileClaims);

            var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.FakeAuth);

            var principal = new GenericPrincipal(claimsIdentity, authVm.MgiAppAwRoles.ToArray());

            return principal;
        }
    }
}
