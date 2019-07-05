using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class BillPayOperationRequest : AcOperationRequest
    {
        public TestBiller Biller { get; set; }
        public AmountRange AmtRange { get; set; }
        public double Amount { get; set; }
        public string ThirdPartyType { get; set; }
        public bool Staging { get; set; }
    }
}