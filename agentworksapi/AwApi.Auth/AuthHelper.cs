using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AwApi.ViewModels;
using Microsoft.Owin;
using MoneyGram.Common;
using MoneyGram.Common.Localization;

namespace AwApi.Auth
{
    public static class AuthHelper
    {
        public static string GetOtp(IOwinContext owinContext)
        {
            var otp = owinContext.Request.Headers.Get(HttpHeaderNames.Otp);
            return otp ?? string.Empty;
        }

        public static string GetBearerToken(IOwinContext owinContext)
        {
            var authHeader = owinContext.Request.Headers.Get(HttpHeaderNames.Authorization);
            if(string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }
            var splitToken = authHeader.Split(' ');
            if(splitToken.Length != 2 || splitToken[0] != "Bearer")
            {
                return null;
            }
            return splitToken[1];
        }

        public static string GetFakeToken(IOwinContext owinContext)
        {
            var authHeader = owinContext.Request.Headers.Get(HttpHeaderNames.Authorization);
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }
            var splitToken = authHeader.Split(' ');
            if (splitToken.Length != 2 || splitToken[0] != "FakeToken")
            {
                return null;
            }
            return splitToken[1];
        }

        public static string GetApiKeyToken(IOwinContext owinContext)
        {
            var apiKeyParam = owinContext.Request.Query.Where(x => x.Key == "api_key").ToList();
            if(!apiKeyParam.Any())
            {
                return null;
            }
            var apiKey = apiKeyParam.First().Value?.First();
            if(apiKey?.Length == 0)
            {
                return null;
            }
            return apiKey;
        }

        public static string GetSupportKeyToken(IOwinContext owinContext)
        {
            var supportKeyToken = owinContext.Request.Headers.Get(HttpHeaderNames.MgiSupport);
            return string.IsNullOrWhiteSpace(supportKeyToken) ? null : supportKeyToken;
        }

        public static string GetDeviceToken(IOwinContext owinContext)
        {
            var deviceSessionId = owinContext.Request.Headers.Get(HttpHeaderNames.MgiSsoSession);
            return string.IsNullOrWhiteSpace(deviceSessionId) ? null : deviceSessionId;
        }

        public static string GetUnregisteredDeviceToken(IOwinContext owinContext)
        {
            var deviceSessionId = owinContext.Request.Headers.Get(HttpHeaderNames.MgiDeviceSessionUnregistered);
            return string.IsNullOrWhiteSpace(deviceSessionId) ? null : deviceSessionId;
        }

        public static void SetUnauthorizedResponse()
        {
            throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm, "User is not authorized to access this resource.", null, HttpStatusCode.Unauthorized);
        }

        public static bool IsAuthEnabled(AwAuthType authType)
        {
            string appSettingsKey;
            switch(authType)
            {
                case AwAuthType.FakeAuth:
                    appSettingsKey = "FakeAuthEnabled";
                    break;
                case AwAuthType.ApiKeyAuth:
                    appSettingsKey = "ApiKeyAuthEnabled";
                    break;
                case AwAuthType.OpenAmAuth:
                    appSettingsKey = "OpenAmAuthEnabled";
                    break;
                case AwAuthType.OpenAmDeviceAuth:
                    appSettingsKey = "OpenAmDeviceAuthEnabled";
                    break;
                case AwAuthType.OpenAmUnregisteredDeviceAuth:
                    appSettingsKey = "OpenAmUnregisteredDeviceAuthEnabled";
                    break;
                case AwAuthType.SupportAuth:
                    appSettingsKey = "SupportAuthEnabled";
                    break;
                default:
                    return false;
            }
            var authSettingExists = ConfigurationManager.AppSettings.AllKeys.Contains(appSettingsKey);
            return authSettingExists && bool.Parse(ConfigurationManager.AppSettings[appSettingsKey]);
        }

        public static string GetPrincipalCreator()
        {
            try
            {
                var principal = (GenericPrincipal) Thread.CurrentPrincipal;
                var principalTypeClaims = principal.Claims
                    .Where(claim => !claim.Type.Equals(ClaimsNames.PrincipalType))
                    .ToList();

                if(principalTypeClaims.Any() && !string.IsNullOrEmpty(principalTypeClaims[0].Value))
                {
                    return principalTypeClaims[0].Value;
                }
                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static void AddPrincipalTypeIdentity(GenericPrincipal principal, string principalType)
        {
            var principalClaims = new List<Claim>()
                .TryAddClaim(ClaimsNames.PrincipalType, principalType);

            var principalIdentity = new ClaimsIdentity(principalClaims, AuthConstants.PrincipalIdentityName);
            principal.AddIdentity(principalIdentity);
        }

        public static async Task FailOrFallThrough(IOwinContext owinContext, OwinMiddleware next, bool isLastAuthProvider)
        {
            if(isLastAuthProvider)
            {
                AuthHelper.SetUnauthorizedResponse();
                return;
            }
            await next.Invoke(owinContext);
        }

        public static void SetUnauthenticatedResponse()
        {
            throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm, LocalizationKeys.UserUnauthenticated, null, HttpStatusCode.Unauthorized);
        }

        public static void SetNotInRoleResponse()
        {
            throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm, LocalizationKeys.UserNotInRole, null, HttpStatusCode.Forbidden);
        }

        public static void SetForbiddenResponse()
        {
            throw PrincipalExceptionFactory.Create(PrincipalExceptionType.OpenAm, LocalizationKeys.Unauthorized, null, HttpStatusCode.Forbidden);
        }

        public static void SetNonTransactionalDeviceInUse()
        {
            throw PrincipalExceptionFactory.Create(PrincipalExceptionType.NonTransactionalDevice, LocalizationKeys.NonTransactionalDeviceInUse, null, HttpStatusCode.Forbidden);
        }

        public static string GetRemoteIpAddress(IOwinContext owinContext)
        {
            return owinContext.Request.RemoteIpAddress ?? "127.0.0.1";
        }

        public static void AssignPrincipal(IOwinContext owinContext, GenericPrincipal principal)
        {
            owinContext.Request.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        public static bool IsUserAuthenticated()
        {
            try
            {
                var currentPrincipal = (GenericPrincipal) Thread.CurrentPrincipal;
                return currentPrincipal.Identity.IsAuthenticated;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static bool ShouldIgnoreRequest(IOwinContext owinContext)
        {
            return owinContext.Request.Method == "OPTIONS" || !owinContext.Request.Path.Value.Contains("/api");
        }
    }
}