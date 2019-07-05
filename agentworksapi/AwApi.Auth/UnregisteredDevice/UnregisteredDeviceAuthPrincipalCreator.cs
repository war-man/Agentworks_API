using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AwApi.ViewModels;

namespace AwApi.Auth.UnregisteredDevice
{
    public class UnregisteredDeviceAuthPrincipalCreator : IUnregisteredDeviceAuthPrincipalCreator
    {
        private readonly IOpenAmPrincipalService openAmPrincipalService;

        public UnregisteredDeviceAuthPrincipalCreator(IOpenAmPrincipalService openAmPrincipalService)
        {
            this.openAmPrincipalService = openAmPrincipalService;
        }

        public async Task<IPrincipal> Create(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            var claims = await openAmPrincipalService.GetUnregisteredDeviceInfo(sessionId);

            var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.OpenAmUnregisteredDevice);

            var roles = claims.Where(x => x.Type == ClaimsNames.Roles)
                .Select(x => x.Value)
                .ToArray();

            var principal = new GenericPrincipal(claimsIdentity, roles);

            return principal;
        }
    }
}
