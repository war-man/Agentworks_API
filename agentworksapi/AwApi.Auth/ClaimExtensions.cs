using System.Collections.Generic;
using System.Security.Claims;

namespace AwApi.Auth
{
    public static class ClaimExtensions
    {
        public static List<Claim> TryAddClaim(this List<Claim> claims, string claimName, string claimValue, string valueType = ClaimValueTypes.String)
        {
            if (claims == null || string.IsNullOrEmpty(claimName) || claimValue == null)
            {
                return claims;
            }

            claims.Add(new Claim(claimName, claimValue, valueType));

            return claims;
        }
    }
}