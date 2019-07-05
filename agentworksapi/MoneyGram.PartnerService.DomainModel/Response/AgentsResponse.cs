using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel.Response
{
    public class AgentsResponse : BaseServiceMessage 
    {
        public List<Agent> AgentList;
    }
}
