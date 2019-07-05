using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public class FeeLookupData : TransactionData
    {
        public FeeLookupRequest FeeLookupReq { get; private set; }

        public FeeLookupResponse FeeLookupResp { get; private set; }

        public void Set(FeeLookupRequest feeLookupRequest)
        {
            FeeLookupReq = feeLookupRequest;
            SetExecutionOrder(nameof(FeeLookupReq));
        }

        public void Set(FeeLookupResponse feeLookupResponse)
        {
            FeeLookupResp = feeLookupResponse;
            SetExecutionOrder(nameof(FeeLookupResp));
        }
    }
}