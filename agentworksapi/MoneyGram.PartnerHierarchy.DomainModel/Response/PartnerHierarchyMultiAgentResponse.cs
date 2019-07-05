using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyGram.PartnerHierarchy.DomainModel.Response
{
    [Serializable]
    public class PartnerHierarchyMultiAgentResponse : PartnerHierarchyAgentResponse
    {
        public IList<Agent> Agent { get; set; }

        public override Agent GetAgent()
        {
            return Agent?.FirstOrDefault();
        }
    }
}