using MoneyGram.PartnerHierarchy.DomainModel.Response;
using System.Threading.Tasks;
using MoneyGram.PartnerHierarchy.DomainModel.Request;

namespace MoneyGram.PartnerHierarchy
{
    public interface IPartnerHierarchyClient
    {
        Task<PartnerHierarchyAgentResponse> GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request);
    }
}
