using System.Collections.Generic;
using MoneyGram.Common.Extensions;
using System.Threading.Tasks;

namespace MoneyGram.OpenAM
{
    public class OpenAmRepository : IOpenAmRepository
    {
        private readonly IOpenAmClient _openAmClient;

        public OpenAmRepository(IOpenAmClient openAmClient)
        {
            openAmClient.ThrowIfNull(nameof(openAmClient));
            _openAmClient = openAmClient;
        }

        public Task<bool> ValidateToken(string bearerToken)
        {
            bearerToken.ThrowIfNullOrEmpty(nameof(bearerToken));

            var isTokenValid = _openAmClient.ValidateToken(bearerToken);

            return isTokenValid;
        }

        public Task<Dictionary<string, object>> GetUserInfo(string bearerToken)
        {
            bearerToken.ThrowIfNull(nameof(bearerToken));

            var userInfo = _openAmClient.GetUserInfo(bearerToken);

            return userInfo;
        }

        public Task<Dictionary<string, object>> ValidateDevice(string deviceId)
        {
            deviceId.ThrowIfNull(nameof(deviceId));

            var deviceInfo = _openAmClient.ValidateDevice(deviceId);

            return deviceInfo;
        }

        public Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId)
        {
            deviceId.ThrowIfNull(nameof(deviceId));

            var deviceInfo = _openAmClient.ValidateUnregisteredDevice(deviceId);

            return deviceInfo;
        }
    }
}