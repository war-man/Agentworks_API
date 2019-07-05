using System.Threading.Tasks;

namespace MoneyGram.OpenIDM
{
    public interface IOpenIdmRepository
    {
        Task<bool> RegisterDevice(DwRegisterDeviceRequest req);
    }
}