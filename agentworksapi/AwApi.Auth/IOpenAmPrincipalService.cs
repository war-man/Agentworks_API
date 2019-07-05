using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AwApi.Auth
{
    public interface IOpenAmPrincipalService
    {
        Task<List<Claim>> GetUserInfo(string token);
        Task<List<Claim>> GetDeviceInfo(string token);
        Task<List<Claim>> GetUnregisteredDeviceInfo(string token);
    }
}