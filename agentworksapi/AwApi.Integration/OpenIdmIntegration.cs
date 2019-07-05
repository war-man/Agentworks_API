using MoneyGram.Common.Extensions;
using MoneyGram.Common.Models;
using MoneyGram.OpenIDM;

namespace AwApi.Integration
{
    public class OpenIdmIntegration : IOpenIdmIntegration
    {
        private readonly IOpenIdmClient _openIdmClient;

        public OpenIdmIntegration(IOpenIdmClient openIdmClient)
        {
            openIdmClient.ThrowIfNull(nameof(openIdmClient));
            _openIdmClient = openIdmClient;
        }

        public bool RegisterDevice(DwRegisterDeviceRequest registerDeviceReq)
        {
            var response = _openIdmClient.RegisterDevice(registerDeviceReq).Result;
            return response;
        }

        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                ServiceName = ServiceNames.OpenIDM,
                Message = "Not Implemented",
                StatusCode = StatusCode.NotImplemented
            };
        }
    }
}