using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Localization;

namespace AwApi.Auth
{
    public class OpenAmPrincipalService : IOpenAmPrincipalService
    {
        private readonly IOpenAmIntegration openAmIntegration;
        private readonly ICacheManager cacheManager;

        public OpenAmPrincipalService(IOpenAmIntegration openAmIntegration, ICacheManager cacheManager)
        {
            this.openAmIntegration = openAmIntegration;
            this.cacheManager = cacheManager;
        }

        public async Task<List<Claim>> GetUserInfo(string token)
        {
            var camsClaimsKey = string.Format(AuthCacheKeys.CAMSCLAIMS, token);
            var cachedCamsClaims = cacheManager.Contains<CachedObjectResponseContainer<List<Tuple<string, string>>>>(camsClaimsKey,
                CacheRegion.Global);
            // Check if claims are missing first
            if (cachedCamsClaims.Exists)
            {
                var missingClaimsInCache = ClaimsInventoryHelper.CheckCamsClaims(cachedCamsClaims.CachedObj.DataObject.ToClaims());
                if (missingClaimsInCache.Any())
                {
                    // We have incomplete data. Invalidate cache
                    cachedCamsClaims.Exists = false;
                    cacheManager.Remove(camsClaimsKey, CacheRegion.Global);
                }
                else
                {
                    return cachedCamsClaims.CachedObj.DataObject.ToClaims();
                }
            }

            Dictionary<string, object> userInfo = null;
            try
            {
                userInfo = await openAmIntegration.GetUserInfo(token);
            }
            catch (Exception)
            {
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.SystemUnavailable, null);
            }

            var camsClaims = ClaimsManager.ParseClaims(userInfo);

            var missingClaims = ClaimsInventoryHelper.CheckCamsClaims(camsClaims);
            if (missingClaims.Any())
            {
                // Throw exception
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.InvalidUserProfile, missingClaims);
            }

            var camsClaimsCacheContainer = CacheAheadHelper.PopulateCacheMetadata<List<Tuple<string, string>>>(camsClaims.ToTupleList(), CachePolicies.FourHours);
            cacheManager.Save(camsClaimsKey, camsClaimsCacheContainer, CacheRegion.Global,
                CachePolicies.FourHours);

            return camsClaims;
        }

        public async Task<List<Claim>> GetDeviceInfo(string token)
        {
            Dictionary<string, object> deviceInfo;

            try
            {
                deviceInfo = await openAmIntegration.ValidateDevice(token);
            }
            catch (Exception)
            {
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.SystemUnavailable, null);
            }

            var camsClaims = ClaimsManager.ParseClaims(deviceInfo);
            var codedClaims = new List<Claim>()
                .TryAddClaim(ClaimsNames.Language, "en-us")                 // set language to en-us...it is not provided
                .TryAddClaim(ClaimsNames.Roles, MgiAwRole.Dt4Device)        // set role to Dt4Device
                .TryAddClaim(ClaimsNames.DSL, DeviceSecurityLevel.Fifty);   // set DSL to fifty                

            var claims = camsClaims.Concat(codedClaims).ToList();

            var missingClaims = ClaimsInventoryHelper.CheckCamsDeviceClaims(claims);
            if (missingClaims.Any())
            {
                // Throw exception
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.InvalidUserProfile, missingClaims);
            }

            return claims;
        }

        public async Task<List<Claim>> GetUnregisteredDeviceInfo(string token)
        {
            Dictionary<string, object> deviceInfo;

            try
            {
                deviceInfo = await openAmIntegration.ValidateUnregisteredDevice(token);
            }
            catch (Exception)
            {
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.SystemUnavailable, null);
            }

            var camsClaims = ClaimsManager.ParseClaims(deviceInfo);
            var codedClaims = new List<Claim>()
               .TryAddClaim(ClaimsNames.Language, "en-us")                          // set language to en-us...it is not provided
               .TryAddClaim(ClaimsNames.Roles, MgiAwRole.Dt4UnregisteredDevice)     // set role to Dt4UnregisteredDevice
               .TryAddClaim(ClaimsNames.DSL, DeviceSecurityLevel.Fifty);            // set DSL to fifty                

            var claims = camsClaims.Concat(codedClaims).ToList();

            var missingClaims = ClaimsInventoryHelper.CheckCamsUnregisteredDeviceClaims(camsClaims);
            if (missingClaims.Any())
            {
                // Throw exception
                throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm,
                    LocalizationKeys.InvalidUserProfile, missingClaims);
            }

            return claims;
        }
    }
}
