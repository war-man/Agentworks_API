using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AwApi.ViewModels;
using MoneyGram.Common.Json;

namespace AwApi.Auth.SupportKey
{
    public class SupportAuthPrincipalCreator : ISupportAuthPrincipalCreator
    {
        private readonly IDictionary<string, AuthClaimsVm> supportAuthAgents;

        public SupportAuthPrincipalCreator()
        {
            var jsonFile = Path.Combine(JsonFileHelper.ExecutingDir(), ConfigurationManager.AppSettings["SupportAuthFileName"]);
            if (File.Exists(jsonFile))
            {
                var jsonData = File.ReadAllText(jsonFile);
                this.supportAuthAgents = JsonProcessor.DeserializeObject<Dictionary<string, AuthClaimsVm>>(jsonData);
            }
        }

        public async Task<IPrincipal> Create(string token)
        {
            if (!supportAuthAgents.ContainsKey(token))
            {
                return null;
            }

            var authVm = supportAuthAgents[token];

            var claims = new List<Claim>()
                .TryAddClaim(ClaimsNames.Token, authVm.Token)
                .TryAddClaim(ClaimsNames.Sub, authVm.Sub);

            var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.FakeAuth);

            var principal = new GenericPrincipal(claimsIdentity, authVm.MgiAppAwRoles.ToArray());

            return principal;
        }
    }
}
