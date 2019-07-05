using MoneyGram.PartnerHierarchy.IntegrationTest.Data;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;

namespace MoneyGram.PartnerHierarchy.IntegrationTest.Operations.MoAndLocation
{
    public class MoAndLocationOperations
    {
        private readonly IPartnerHierarchyConfig _config;
        private readonly IPartnerHierarchyClient _client;

        public MoAndLocationOperations(string clientUrl)
        {
            _config = new PartnerHierarchyConfigCoded()
            {
                PartnerHierarchyUrl = clientUrl
            };
            _client = new PartnerHierarchyClient(_config);
        }

        public PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(string mgiMainOfficeId, string mgiDeviceAgentLocationId)
        {
            var response = _client.GetPartnerHierarchyAgent(new PartnerHierarchyAgentRequest()
            {
                LocationId = mgiDeviceAgentLocationId,
                MainofficeId = mgiMainOfficeId
            });
            return response.Result;
        }
    }
}
