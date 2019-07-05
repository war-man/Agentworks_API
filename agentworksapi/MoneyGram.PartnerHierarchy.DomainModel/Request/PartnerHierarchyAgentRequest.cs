using System;

namespace MoneyGram.PartnerHierarchy.DomainModel.Request
{
    [Serializable]
    public sealed class PartnerHierarchyAgentRequest
    {
        public string LocationId { get; set; }

        public string MainofficeId { get; set; }
    }
}
