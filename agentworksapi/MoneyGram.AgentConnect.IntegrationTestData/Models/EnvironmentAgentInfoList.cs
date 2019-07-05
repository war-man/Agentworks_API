using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Models
{
    public class EnvironmentAgentInfoList
    {
        public string Environment { get; set; }
        public List<EnvironmentAgent> Agents { get; set; }
    }
}