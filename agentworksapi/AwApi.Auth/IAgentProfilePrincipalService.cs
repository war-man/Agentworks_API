using System.Collections.Generic;
using System.Security.Claims;

namespace AwApi.Auth
{
    public interface IAgentProfilePrincipalService
    {
        List<Claim> GetAgentProfileClaims(string agentId, string posNumber, string password, string language, string sessionId);
    }
}
