using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    public class LocationsForMoResponse : BaseServiceMessage
    {
        public List<Agent> AgentList { get; set; }
    }
}
