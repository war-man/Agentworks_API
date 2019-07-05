using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Extensions
{
    public static class AcOperationRequestExtensions
    {
        public static void PopulateAgentData(this AcOperationRequest req, string location, bool isTrainingMode = false)
        {
            if (string.IsNullOrWhiteSpace(req.AgentId) && string.IsNullOrWhiteSpace(req.AgentPos))
            {
                var agent = new Agents(isTrainingMode).GetAgent(location);
                req.AgentId = agent.AgentId;
                req.AgentPos = agent.AgentSequence;
            }
            else
            {
                var agent = new Agents(isTrainingMode).GetAgent(req.AgentId, req.AgentPos);
                req.AgentId = agent.AgentId;
                req.AgentPos = agent.AgentSequence;
            }
        }
    }
}