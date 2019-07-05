using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyGram.Common.Models;
using MoneyGram.OpenAM;

namespace AwApi.Integration
{
    public class OpenAmIntegration : IOpenAmIntegration
    {
        private readonly IOpenAmRepository _repository;

        public OpenAmIntegration(IOpenAmRepository openAmRepository)
        {
            _repository = openAmRepository;
        }

        public Task<bool> ValidateToken(string bearerToken)
        {
            return _repository.ValidateToken(bearerToken);
        }

        public Task<Dictionary<string, object>> GetUserInfo(string bearerToken)
        {
            return _repository.GetUserInfo(bearerToken);
        }

        public Task<Dictionary<string, object>> ValidateDevice(string deviceId)
        {
            return _repository.ValidateDevice(deviceId);
        }

        public Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId)
        {
            return _repository.ValidateUnregisteredDevice(deviceId);
        }

        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                ServiceName = ServiceNames.OpenAM,
                Message = "Not Implemented",
                StatusCode = StatusCode.NotImplemented
            };
        }
    }
}