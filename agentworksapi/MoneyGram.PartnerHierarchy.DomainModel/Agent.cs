using System;

namespace MoneyGram.PartnerHierarchy.DomainModel
{
    [Serializable]
    public class Agent
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country{ get; set; }
        public string CurrentLevel { get; set; }
        public string Id { get; set; }
        public string IsRetailCredit { get; set; }
        public string MainOfficeId { get; set; }
        public string MainOfficeOracleAccountNumber { get; set; }
        public string Name { get; set; }
        public string OracleAccountNumber { get; set; }
        public string ParentId { get; set; }
        public string ParentOracleAccountNumber { get; set; }
        public string Phone { get; set; } 
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
    }
}
