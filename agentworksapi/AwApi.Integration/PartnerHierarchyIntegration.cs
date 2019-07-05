using MoneyGram.PartnerHierarchy;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;

namespace AwApi.Integration
{
    public class PartnerHierarchyIntegration : IPartnerHierarchyIntegration
    {
        private readonly IPartnerHierarchyRepository _partnerHierarchyRepository;

        public PartnerHierarchyIntegration(IPartnerHierarchyRepository partnerHierarchyRepository)
        {
            _partnerHierarchyRepository = partnerHierarchyRepository;
        }
        public PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request)
        {
            return _partnerHierarchyRepository.GetPartnerHierarchyAgent(request);            
        }
    }
}
