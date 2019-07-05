using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public class BankDetail
    {
        public HierarchyLevelInfo BankInfo { get; set; }
        public List<BankState> BankStates { get; set; }

    }
}