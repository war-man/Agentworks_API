using MoneyGram.Common.Models;
using MoneyGram.OpenIDM;

namespace AwApi.Integration
{
    public interface IOpenIdmIntegration
    {
        HealthCheckResponse HealthCheck();
        bool RegisterDevice(DwRegisterDeviceRequest registerDeviceReq);
    }
}