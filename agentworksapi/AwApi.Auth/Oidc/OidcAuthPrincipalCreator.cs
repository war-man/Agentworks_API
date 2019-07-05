using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AwApi.Integration;
using AwApi.ViewModels;

namespace AwApi.Auth.Oidc
{
    public class OidcAuthPrincipalCreator : IOidcAuthPrincipalCreator
    {
        private readonly IAgentProfilePrincipalService agentProfilePrincipalService;
        private readonly IAgentPasswordPrincipalService agentPasswordPrincipalService;
        private readonly IOpenAmIntegration openAmIntegration;
        private readonly IOpenAmPrincipalService openAmPrincipalService;

        public OidcAuthPrincipalCreator(IAgentProfilePrincipalService agentProfilePrincipalService, 
            IAgentPasswordPrincipalService agentPasswordPrincipalService, 
            IOpenAmIntegration openAmIntegration,
            IOpenAmPrincipalService openAmPrincipalService)
        {
            this.agentProfilePrincipalService = agentProfilePrincipalService;
            this.agentPasswordPrincipalService = agentPasswordPrincipalService;
            this.openAmIntegration = openAmIntegration;
            this.openAmPrincipalService = openAmPrincipalService;
        }

        public async Task<IPrincipal> Create(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            // Check Token Validity
            var isTokenValid = await openAmIntegration.ValidateToken(sessionId);
            if (isTokenValid == false)
            {
                return null;
            }

            var userInfoClaims = await openAmPrincipalService.GetUserInfo(sessionId);
            var agentProfileClaims = GetAgentProfileClaims(sessionId, userInfoClaims);

            var claims = userInfoClaims.Concat(agentProfileClaims);
            var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.OpenAm);

            var roles = claims.Where(x => x.Type == ClaimsNames.Roles)
                .Select(x => x.Value)
                .ToArray();

            var principal = new GenericPrincipal(claimsIdentity, roles);

            return principal;
        }

        private List<Claim> GetAgentProfileClaims(string sessionId, List<Claim> userInfoClaims)
        {
            var claims = new List<Claim>();

            var agentId = userInfoClaims.FirstOrDefault(x => x.Type == ClaimsNames.MgiAgentLocationId)?.Value;
            var posNumber = userInfoClaims.FirstOrDefault(x => x.Type == ClaimsNames.MgiDevicePosNumber)?.Value;
            var language = userInfoClaims.FirstOrDefault(x => x.Type == ClaimsNames.Language)?.Value;

            // If OpenAM provided an AgentID and POS Number, retrieve the profile from AgentConnect
            if (!string.IsNullOrEmpty(agentId) && !string.IsNullOrEmpty(posNumber))
            {
                var agentPassword = agentPasswordPrincipalService.GetAgentPassword(sessionId, agentId, posNumber);
                var agentProfileClaims = agentProfilePrincipalService.GetAgentProfileClaims(agentId, posNumber, agentPassword, language, sessionId);

                claims.Add(new Claim(ClaimsNames.AgentPassword, agentPassword));
                claims.AddRange(agentProfileClaims);
            }

            return claims;
        }
    }
}
