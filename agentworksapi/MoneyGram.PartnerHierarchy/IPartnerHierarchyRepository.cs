using MoneyGram.PartnerHierarchy.DomainModel.Response;
using MoneyGram.PartnerHierarchy.DomainModel.Request;

namespace MoneyGram.PartnerHierarchy
{
    public interface IPartnerHierarchyRepository
    {
        PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request);
    }
}
