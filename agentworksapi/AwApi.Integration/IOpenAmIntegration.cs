using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyGram.Common.Models;

namespace AwApi.Integration
{
    public interface IOpenAmIntegration
    {
        Task<Dictionary<string, object>> GetUserInfo(string bearerToken);
        Task<bool> ValidateToken(string bearerToken);
        Task<Dictionary<string, object>> ValidateDevice(string deviceId);
        Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId);
        HealthCheckResponse HealthCheck();
    }
}