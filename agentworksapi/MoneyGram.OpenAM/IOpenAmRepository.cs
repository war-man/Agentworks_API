using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyGram.OpenAM
{
    public interface IOpenAmRepository
    {
        Task<bool> ValidateToken(string bearerToken);

        Task<Dictionary<string, object>> GetUserInfo(string bearerToken);

        Task<Dictionary<string, object>> ValidateDevice(string deviceId);
        Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId);
    }
}