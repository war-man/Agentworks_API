using System;

namespace MoneyGram.PartnerHierarchy.DomainModel.Response
{
    [Serializable]
    public class PartnerHierarchySingleAgentResponse : PartnerHierarchyAgentResponse
    {
        public Agent Agent { get; set; }

        public override Agent GetAgent()
        {
            return Agent;
        }
    }
}