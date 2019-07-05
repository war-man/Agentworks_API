using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using MoneyGram.Common.Json;

namespace AwApi.Auth.ApiKey
{
    public class ApiKeyPrincipalCreator : FakePrincipalCreator, IApiKeyPrincipalCreator
    {
        private readonly IDictionary<string, AuthClaimsVm> apiKeyAgents;

        public ApiKeyPrincipalCreator(IAgentProfilePrincipalService agentConnectPrincipalService):
            base(agentConnectPrincipalService)
        {
            var jsonFile = Path.Combine(JsonFileHelper.ExecutingDir(), ConfigurationManager.AppSettings["apiKeysFileName"]);
            if (File.Exists(jsonFile))
            {
                var jsonData = File.ReadAllText(jsonFile);
                this.apiKeyAgents = JsonProcessor.DeserializeObject<Dictionary<string, FakeAuthInfoVm>>(jsonData)
                        .ToDictionary(x => x.Key, x => x.Value.ToAuthClaimsVm());
            }
        }

        public async Task<IPrincipal> Create(string apiKey)
        {
            return base.Create(apiKeyAgents, apiKey);
        }
    }
}
