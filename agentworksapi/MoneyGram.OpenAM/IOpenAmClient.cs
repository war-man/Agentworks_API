using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyGram.OpenAM
{
    public interface IOpenAmClient
    {
        Task<bool> ValidateToken(string token);

        Task<Dictionary<string, object>> GetUserInfo(string token);

        Task<Dictionary<string, object>> ValidateDevice(string deviceId);
        Task<Dictionary<string, object>> ValidateUnregisteredDevice(string deviceId);
    }
}