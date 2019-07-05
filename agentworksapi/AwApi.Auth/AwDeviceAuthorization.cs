using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Collections.Generic;
using AwApi.ViewModels;

namespace AwApi.Auth
{
    public class AwDeviceAuthorization : AuthorizeAttribute
    {
        public AwDeviceAuthorization(params string[] deviceSecurityLevels) : base()
        {
            var rolesList = new List<string>(deviceSecurityLevels);

            Roles += string.Join(",", rolesList.Select(x => DeviceSecurityLevel.GetApplicationDefinedDevices(x)));
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var deviceSecurityLevel = ClaimsManager.GetClaimValue(ClaimsNames.DSL);

            var authenticated = AuthHelper.IsUserAuthenticated();
            var isDeviceAuthorized = (string.IsNullOrEmpty(Roles) ? true : Roles.Split(',').Contains(deviceSecurityLevel));

            if (!authenticated)
            {
                AuthHelper.SetUnauthenticatedResponse();
            }

            if (!isDeviceAuthorized)
            {
                AuthHelper.SetNonTransactionalDeviceInUse();
            }

            return (authenticated && isDeviceAuthorized);
        }
    }
}