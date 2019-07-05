using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    public class POEAgentLocationMoResponse : BaseServiceMessage
    {
        public List<Agent> AgentList { get; set; }
    }
}
