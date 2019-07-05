using System.Threading.Tasks;

namespace MoneyGram.OpenIDM
{
    public interface IOpenIdmClient
    {
        Task<bool> RegisterDevice(DwRegisterDeviceRequest registerDeviceReq);
    }
}