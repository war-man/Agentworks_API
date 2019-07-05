using System;

namespace MoneyGram.PartnerHierarchy.DomainModel.Response
{
    [Serializable]
    public abstract class PartnerHierarchyAgentResponse
    {
        public abstract Agent GetAgent();
    }
}