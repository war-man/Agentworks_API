using System.Threading.Tasks;
using MoneyGram.Common.Extensions;

namespace MoneyGram.OpenIDM
{
    public class OpenIdmRepository : IOpenIdmRepository
    {
        private readonly IOpenIdmClient _openIdmClient;

        public OpenIdmRepository(IOpenIdmClient openIdmClient)
        {
            openIdmClient.ThrowIfNull(nameof(openIdmClient));
            _openIdmClient = openIdmClient;
        }

        public Task<bool> RegisterDevice(DwRegisterDeviceRequest req)
        {
            var isTokenValid = _openIdmClient.RegisterDevice(req);
            return isTokenValid;
        }
    }
}