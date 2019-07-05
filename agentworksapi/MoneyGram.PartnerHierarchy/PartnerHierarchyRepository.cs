using MoneyGram.Common.Extensions;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;

namespace MoneyGram.PartnerHierarchy
{
    public class PartnerHierarchyRepository : IPartnerHierarchyRepository
    {
        private readonly IPartnerHierarchyClient _partnerHierarchyClient;

        public PartnerHierarchyRepository(IPartnerHierarchyClient partnerHierarchyClient)
        {
            partnerHierarchyClient.ThrowIfNull(nameof(partnerHierarchyClient));
            _partnerHierarchyClient = partnerHierarchyClient;
        }

        public PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request)
        {
            var response = _partnerHierarchyClient.GetPartnerHierarchyAgent(request);
            return response.Result;
        }
    }
}
