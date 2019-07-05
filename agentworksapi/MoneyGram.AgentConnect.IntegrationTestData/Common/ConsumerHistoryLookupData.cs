using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public class ConsumerHistoryLookupData : TransactionData
    {
        public ConsumerHistoryLookupRequest ConsumerHistoryLookupReq { get; private set; }

        public ConsumerHistoryLookupResponse ConsumerHistoryLookupResp { get; private set; }

        public void Set(ConsumerHistoryLookupRequest consumerHistoryLookupRequest)
        {
            ConsumerHistoryLookupReq = consumerHistoryLookupRequest;
            SetExecutionOrder(nameof(ConsumerHistoryLookupReq));
        }

        public void Set(ConsumerHistoryLookupResponse consumerHistoryLookupResponse)
        {
            ConsumerHistoryLookupResp = consumerHistoryLookupResponse;
            SetExecutionOrder(nameof(ConsumerHistoryLookupResp));
        }
    }
}