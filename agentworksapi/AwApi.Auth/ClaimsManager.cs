using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Json;

namespace AwApi.Auth
{
    public class ClaimsManager
    {
        private readonly ICacheManager _cacheManager;
        private string Token { get; }

        public ClaimsManager(ICacheManager cacheManager, string token)
        {
            Roles = new List<string>();
            Claims = new List<Claim>();

            _cacheManager = cacheManager;
            Token = token;
        }

        public List<string> Roles { get; set; }
        public List<Claim> Claims { get; set; }

        public static bool HasClaims()
        {
            try
            {
                var principal = (GenericPrincipal) Thread.CurrentPrincipal;
                return principal?.Claims != null && principal.Claims.Any();
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static string GetClaimValue(string claimType)
        {
            var principal = (GenericPrincipal) Thread.CurrentPrincipal;
            return principal?.Claims?.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }

        /// <summary>
        /// Retrieves claim values for a multi-value claim
        /// </summary>
        /// <param name="claimType">Claim name</param>
        /// <returns>IEnumerable of claim values</returns>
        public static IEnumerable<string> GetClaimValues(string claimType)
        {
            var principal = (GenericPrincipal) Thread.CurrentPrincipal;
            return principal?.Claims?.Where(claim => claim.Type == claimType)?.Select(claim => claim.Value).ToList();
        }

        public static string GetClaimValue(List<Claim> claims, string claimType)
        {
            if(claims != null && claims.Any(c => c.Type == claimType))
            {
                return claims.Find(c => c.Type == claimType).Value;
            }
            return null;
        }

        public static bool GetBooleanClaimValue(List<Claim> claims, string claimType)
        {
            if(claims != null && claims.Any(c => c.Type == claimType))
            {
                return (claims?.FirstOrDefault(claim => claim.Type == claimType)?.Value).ToLower().Equals("true");
            }
            return false;
        }

        public static List<Claim> ParseClaims(Dictionary<string, object> response)
        {
            var claims = new List<Claim>();
            if(response == null)
            {
                return claims;
            }

            foreach(var claim in response)
            {
                if(claim.Value is string)
                {
                    if(!string.IsNullOrWhiteSpace(claim.Value.ToString()))
                    {
                        claims.TryAddClaim(claim.Key, (string)claim.Value);
                    }
                }
                else
                {
                    var claimValues = JsonProcessor.ToStringList(claim);
                    foreach(var claimValue in claimValues)
                    {
                        if (!string.IsNullOrWhiteSpace(claimValue))
                        {
                            claims.TryAddClaim(claim.Key, claimValue);
                        }
                    }
                }
            }

            return claims;
        }
    }
}