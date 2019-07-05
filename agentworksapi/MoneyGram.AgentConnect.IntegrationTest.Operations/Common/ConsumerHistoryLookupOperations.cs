using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Common
{
    public class ConsumerHistoryLookupOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        public ConsumerHistoryLookupOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        public ConsumerHistoryLookupData ConsumerHistoryLookup(string agentId, string agentPos, bool existing, SessionType tranType)
        {
            var consumerHistoryLookupData = new ConsumerHistoryLookupData();
            var request = existing ? ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestExisting : ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestNonExisting;
            request.AgentID = agentId;
            request.AgentSequence = agentPos;
            consumerHistoryLookupData.Set(request);
            consumerHistoryLookupData.ConsumerHistoryLookupReq.MgiSessionType = tranType;
            consumerHistoryLookupData.Set(_acIntegration.ConsumerHistoryLookup(consumerHistoryLookupData.ConsumerHistoryLookupReq));

            return consumerHistoryLookupData;
        }
    }
}   