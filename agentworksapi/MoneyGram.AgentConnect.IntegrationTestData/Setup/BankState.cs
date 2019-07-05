using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public class BankState
    {
        public HierarchyLevelInfo StateInfo { get; set; }
        public List<BankCity> BankCities { get; set; }

    }
}