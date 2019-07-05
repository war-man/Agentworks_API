using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class SendRequest : AcOperationRequest
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string SendCurr { get; set; }
        public AmountRange AmtRange { get; set; }
        public double Amount { get; set; }
        public ItemChoiceType1 FeeType { get; set; }
        public string ServiceOption { get; set; }
        public string ThirdPartyType { get; set; }
    }
}