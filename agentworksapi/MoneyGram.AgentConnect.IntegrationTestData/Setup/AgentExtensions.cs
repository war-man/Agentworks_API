using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class AgentExtensions
    {
        /// <summary>
        ///     Converts an EnvironmentAgent to an Agent.
        /// </summary>
        /// <param name="environmentAgent"></param>
        /// <param name="isTrainingMode"></param>
        /// <returns></returns>
        public static Agent ToAgent(this EnvironmentAgent environmentAgent, bool isTrainingMode)
        {
            return new Agent
            {
                AgentId = environmentAgent.AgentId,
                AgentSequence = environmentAgent.AgentSequence,
                AgentPassword = environmentAgent.AgentPassword,
                Language = environmentAgent.Language,
                IsInTrainingMode = isTrainingMode
            };
        }
    }
}