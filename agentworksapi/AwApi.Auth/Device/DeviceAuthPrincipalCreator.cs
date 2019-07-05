using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AwApi.ViewModels;

namespace AwApi.Auth.Device
{
    public class DeviceAuthPrincipalCreator : IDeviceAuthPrincipalCreator
    {
        private readonly IAgentPasswordPrincipalService agentPasswordPrincipalService;
        private readonly IOpenAmPrincipalService openAmPrincipalService;

        public DeviceAuthPrincipalCreator(IAgentPasswordPrincipalService agentPasswordPrincipalService, IOpenAmPrincipalService openAmPrincipalService)
        {
            this.agentPasswordPrincipalService = agentPasswordPrincipalService;
            this.openAmPrincipalService = openAmPrincipalService;
        }

        public async Task<IPrincipal> Create(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            var deviceInfoClaims = await openAmPrincipalService.GetDeviceInfo(sessionId);
            var agentPasswordClaims = GetAgentPasswordClaims(sessionId, deviceInfoClaims);

            var claims = deviceInfoClaims.Concat(agentPasswordClaims);
            var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.OpenAmDevice);

            var roles = claims.Where(x => x.Type == ClaimsNames.Roles)
                .Select(x => x.Value)
                .ToArray();

            var principal = new GenericPrincipal(claimsIdentity, roles);

            return principal;
        }

        private List<Claim> GetAgentPasswordClaims(string sessionId, List<Claim> deviceInfoClaims)
        {
            var claims = new List<Claim>();

            var agentId = deviceInfoClaims.FirstOrDefault(x => x.Type == ClaimsNames.MgiAgentLocationId)?.Value;
            var posNumber = deviceInfoClaims.FirstOrDefault(x => x.Type == ClaimsNames.MgiDevicePosNumber)?.Value;

            // If OpenAM provided an AgentID and POS Number, retrieve the profile from AgentConnect
            if (!string.IsNullOrEmpty(agentId) && !string.IsNullOrEmpty(posNumber))
            {
                var agentPassword = agentPasswordPrincipalService.GetAgentPassword(sessionId, agentId, posNumber);

                claims.Add(new Claim(ClaimsNames.AgentPassword, agentPassword));
            }

            return claims;
        }
    }
}
