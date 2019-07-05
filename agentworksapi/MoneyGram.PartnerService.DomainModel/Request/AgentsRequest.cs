
namespace MoneyGram.PartnerService.DomainModel.Request
{
    public class AgentsRequest : BaseServiceMessage 
    {
        public decimal AgentId { get; set; }

        public bool AgentIdSpecified { get; set; }

        public string AgentName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string StateCode { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public int HierarchyLevel { get; set; }

        public bool HierarchyLevelSpecified { get; set; }
    }
}
