using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using MoneyGram.Common.Json;

namespace AwApi.Auth.Fake
{
    public class FakeAuthPrincipalCreator : FakePrincipalCreator, IFakeAuthPrincipalCreator
    {
        private readonly IDictionary<string, AuthClaimsVm> fakeAuthAgents;

        public FakeAuthPrincipalCreator(IAgentProfilePrincipalService agentConnectPrincipalService):
            base(agentConnectPrincipalService)
        {
            var jsonFile = Path.Combine(JsonFileHelper.ExecutingDir(), ConfigurationManager.AppSettings["FakeAuthAgentsFileName"]);
            if (File.Exists(jsonFile))
            {
                var jsonData = File.ReadAllText(jsonFile);
                this.fakeAuthAgents = JsonProcessor.DeserializeObject<Dictionary<string, FakeAuthInfoVm>>(jsonData)
                        .ToDictionary(x => x.Key, x => x.Value.ToAuthClaimsVm());
            }
        }

        public async Task<IPrincipal> Create(string fakeToken)
        {
            return base.Create(fakeAuthAgents, fakeToken);
        }
    }
}
