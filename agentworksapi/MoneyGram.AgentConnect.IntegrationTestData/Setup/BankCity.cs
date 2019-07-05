using MoneyGram.AgentConnect.DomainModel.Transaction;
using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public class BankCity
    {
        public HierarchyLevelInfo CityInfo { get; set; }
        public List<HierarchyLevelInfo> BankBranches { get; set; }
    }
}