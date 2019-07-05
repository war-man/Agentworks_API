using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyGram.OpenAM
{
    public abstract class OpenAmDecorator : IOpenAmRepository
    {
        private readonly IOpenAmRepository _openAmRepository;

        public OpenAmDecorator(IOpenAmRepository openAmRepository)
        {
            _openAmRepository = openAmRepository;
        }

        public Task<Dictionary<string, object>> GetUserInfo(string bearerToken)
        {
            return _openAmRepository.GetUserInfo(bearerToken);
        }

        public Task<bool> ValidateToken(string bearerToken)
        {
            return _openAmRepository.ValidateToken(bearerToken);
        }

        public Task<Dictionary<string, object>> ValidateDevice(string deviceId)
        {
            return _openAmRepository.ValidateDevice(deviceId);
        }

        public Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId)
        {
            return _openAmRepository.ValidateUnregisteredDevice(deviceId);
        }
    }
}