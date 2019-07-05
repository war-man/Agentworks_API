using MoneyGram.Common.Extensions;
using MoneyGram.PartnerHierarchy;
using MoneyGram.PartnerHierarchy.DomainModel.Request;
using MoneyGram.PartnerHierarchy.DomainModel.Response;

namespace AwApi.Integration
{
    public abstract class PartnerHierarchyDecorator: IPartnerHierarchyRepository
    {
        public PartnerHierarchyDecorator(IPartnerHierarchyRepository partnerHierarchy)
        {
            partnerHierarchy.ThrowIfNull(nameof(partnerHierarchy));

            PartnerHierarchy = partnerHierarchy;
        }

        protected IPartnerHierarchyRepository PartnerHierarchy { get; }

        public virtual PartnerHierarchyAgentResponse GetPartnerHierarchyAgent(PartnerHierarchyAgentRequest request)
        {
            return PartnerHierarchy.GetPartnerHierarchyAgent(request);
        }
    }
}
