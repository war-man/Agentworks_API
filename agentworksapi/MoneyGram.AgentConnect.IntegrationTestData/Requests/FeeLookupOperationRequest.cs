using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class FeeLookupOperationRequest : AcOperationRequest
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string SendCurr { get; set; }
        public AmountRange AmtRange { get; set; }
        public double Amount { get; set; }
        public ItemChoiceType FeeType { get; set; }
        public string ReceiveCode { get; set; }
        public string ReceiveAgentId { get; set; }
    }
}