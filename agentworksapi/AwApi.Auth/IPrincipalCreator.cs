using System.Security.Principal;
using System.Threading.Tasks;

namespace AwApi.Auth
{
    public interface IPrincipalCreator
    {
        Task<IPrincipal> Create(string sessionId);
    }
}
