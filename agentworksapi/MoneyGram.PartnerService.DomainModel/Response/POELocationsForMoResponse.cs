using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel
{
    public class POELocationsForMoResponse : BaseServiceMessage
    {
        public List<Agent> AgentList { get; set; }
    }
}
