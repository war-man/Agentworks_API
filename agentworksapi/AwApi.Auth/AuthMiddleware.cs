using AwApi.ViewModels;
using Microsoft.Owin;
using NLog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AwApi.Auth.ApiKey;
using AwApi.Auth.Device;
using AwApi.Auth.Fake;
using AwApi.Auth.Oidc;
using AwApi.Auth.SupportKey;
using AwApi.Auth.UnregisteredDevice;

namespace AwApi.Auth
{
    public class AuthMiddleware : IAuthMiddleware
    {
        private readonly IOidcAuthPrincipalCreator oidcAuthPrincipalCreator;
        private readonly IDeviceAuthPrincipalCreator deviceAuthPrincipalCreator;
        private readonly IUnregisteredDeviceAuthPrincipalCreator unregisteredDeviceAuthPrincipalCreator;
        private readonly IFakeAuthPrincipalCreator fakeAuthPrincipalCreator;
        private readonly IApiKeyPrincipalCreator apiKeyPrincipalCreator;
        private readonly ISupportAuthPrincipalCreator supportAuthPrincipalCreator;
        private readonly bool fakeAuthEnabled;
        private readonly bool apiKeyAuthEnabled;

        public AuthMiddleware(IOidcAuthPrincipalCreator oidcAuthPrincipalCreator,
            IDeviceAuthPrincipalCreator deviceAuthPrincipalCreator,
            IUnregisteredDeviceAuthPrincipalCreator unregisteredDeviceAuthPrincipalCreator,
            IFakeAuthPrincipalCreator fakeAuthPrincipalCreator, 
            IApiKeyPrincipalCreator apiKeyPrincipalCreator, 
            ISupportAuthPrincipalCreator supportAuthPrincipalCreator)
        {
            this.oidcAuthPrincipalCreator = oidcAuthPrincipalCreator;
            this.deviceAuthPrincipalCreator = deviceAuthPrincipalCreator;
            this.unregisteredDeviceAuthPrincipalCreator = unregisteredDeviceAuthPrincipalCreator;
            this.fakeAuthPrincipalCreator = fakeAuthPrincipalCreator;
            this.apiKeyPrincipalCreator = apiKeyPrincipalCreator;
            this.supportAuthPrincipalCreator = supportAuthPrincipalCreator;

            fakeAuthEnabled = AuthHelper.IsAuthEnabled(AwAuthType.FakeAuth);
            apiKeyAuthEnabled = AuthHelper.IsAuthEnabled(AwAuthType.ApiKeyAuth);
        }

        public async Task Invoke(IOwinContext context, Func<Task> next)
        {
            if (AuthHelper.ShouldIgnoreRequest(context))
            {
                await next();
                return;
            }

            var principal = await CreatePrincipal(context);

            if (principal == null)
            {
                AuthHelper.SetUnauthorizedResponse();
                return;
            }

            context.Request.User = principal;

            // Set log properties
            MappedDiagnosticsContext.Set("username", ((ClaimsPrincipal)principal).Claims.FirstOrDefault(x => x.Type == ClaimsNames.Sub)?.Value);
            MappedDiagnosticsContext.Set("posnum", ((ClaimsPrincipal)principal).Claims.FirstOrDefault(x => x.Type == ClaimsNames.MgiDevicePosNumber)?.Value);
            MappedDiagnosticsContext.Set("unitprofileid", ((ClaimsPrincipal)principal).Claims.FirstOrDefault(x => x.Type == ClaimsNames.MgiDevicePosUnitProfileId)?.Value);
            MappedDiagnosticsContext.Set("agentid", ((ClaimsPrincipal)principal).Claims.FirstOrDefault(x => x.Type == ClaimsNames.MgiAgentLocationId)?.Value);
            await next();
        }

        private async Task<IPrincipal> CreatePrincipal(IOwinContext context)
        {
            var fakeToken = AuthHelper.GetFakeToken(context);
            var bearerToken = AuthHelper.GetBearerToken(context);
            var apiKey = AuthHelper.GetApiKeyToken(context);
            var deviceToken = AuthHelper.GetDeviceToken(context);
            var unregisteredDeviceToken = AuthHelper.GetUnregisteredDeviceToken(context);
            var supportKey = AuthHelper.GetSupportKeyToken(context);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                return await oidcAuthPrincipalCreator.Create(bearerToken);
            }
            else if (!string.IsNullOrEmpty(deviceToken))
            {
                return await deviceAuthPrincipalCreator.Create(deviceToken);
            }
            else if (!string.IsNullOrEmpty(unregisteredDeviceToken))
            {
                return await unregisteredDeviceAuthPrincipalCreator.Create(unregisteredDeviceToken);
            }
            else if (!string.IsNullOrEmpty(supportKey))
            {
                return await supportAuthPrincipalCreator.Create(supportKey);
            }
            else if (!string.IsNullOrEmpty(fakeToken) && fakeAuthEnabled)
            {
                return await fakeAuthPrincipalCreator.Create(fakeToken);
            }
            else if (!string.IsNullOrEmpty(apiKey) && apiKeyAuthEnabled)
            {
                return await apiKeyPrincipalCreator.Create(apiKey);
            }

            return null;
        }
    }
}
