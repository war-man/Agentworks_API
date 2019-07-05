using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Common
{
    public class FeeLookupOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        public FeeLookupOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        public FeeLookupData FeeLookupForSend(SendData sendData)
        {
            var feeLookupData = new FeeLookupData();
            var feeRequest = FeeLookupRequestFactory.FeeLookupRequestForSend(sendData);
            feeRequest.AgentID = sendData.SendRequest.AgentId;
            feeRequest.AgentSequence = sendData.SendRequest.AgentPos;
            feeLookupData.Set(feeRequest);
            feeLookupData.Set(_acIntegration.FeeLookup(feeLookupData.FeeLookupReq));

            return feeLookupData;
        }

        public FeeLookupData FeeLookupForBpEp(string fromLocation, string toCountry, string toState, string sendCurr, AmountRange amtRange, ItemChoiceType1 itemChoiceType, string receiveCode, string receiveAgentId, string agentId, string agentPos)
        {
            var feeLookupData = new FeeLookupData();
            var feeLookupReq = FeeLookupRequestFactory.FeeLookupForBpEp(toCountry, toState, sendCurr, amtRange, itemChoiceType, receiveCode, receiveAgentId, agentId, agentPos);
            feeLookupData.Set(feeLookupReq);
            feeLookupData.Set(_acIntegration.FeeLookup(feeLookupData.FeeLookupReq));

            return feeLookupData;
        }
    }
}