using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    public class AgentLocationMoResponse : BaseServiceMessage
    {
        public List<Agent> AgentList { get; set; }
    }
}
