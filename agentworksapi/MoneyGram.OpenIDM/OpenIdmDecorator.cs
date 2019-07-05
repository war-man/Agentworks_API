using System.Threading.Tasks;

namespace MoneyGram.OpenIDM
{
    public abstract class OpenIdmDecorator : IOpenIdmRepository
    {
        private readonly IOpenIdmRepository _openIdmRepository;

        public OpenIdmDecorator(IOpenIdmRepository openIdmRepository)
        {
            _openIdmRepository = openIdmRepository;
        }

        public Task<bool> RegisterDevice(DwRegisterDeviceRequest req)
        {
            return _openIdmRepository.RegisterDevice(req);
        }
    }
}