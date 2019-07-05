using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public class Agents
    {
        private readonly bool _isTrainingMode = false;

        public Agents(bool isTrainingMode)
        {
            _isTrainingMode = isTrainingMode;
        }


        public Agent GetAgent(string agentLocation)
        {
            var agentData = GetAgentData();

            return agentData.Any(x => x.AgentStateCode == agentLocation)
                ? agentData.First(x => x.AgentStateCode == agentLocation).ToAgent(_isTrainingMode)
                : null;
        }

        public Agent GetAgent(string agentId, string agentPos)
        {
            var agentData = GetAgentData();

            return agentData.Any(x => x.AgentId == agentId && x.AgentSequence == agentPos)
                ? agentData.First(x => x.AgentId == agentId && x.AgentSequence == agentPos).ToAgent(_isTrainingMode)
                : null;
        }

        private List<EnvironmentAgent> GetAgentData()
        {
            var testSettings = TestConfig.TestSettings;
            var acEnvironment = testSettings.AcEnvironment.ToString();
            var repo = new EnvironmentAgentRepository();

            return repo.ContainsKey(acEnvironment)
                ? repo.GetValue<List<EnvironmentAgent>>(acEnvironment)
                : new List<EnvironmentAgent>();
        }
    }
}