using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public class TransactionLookupData : TransactionData
    {
        public TransactionLookupRequest TransactionLookupReq { get; private set; }

        public TransactionLookupResponse TransactionLookupResp { get; private set; }

        public void Set(TransactionLookupRequest transactionLookupRequest)
        {
            TransactionLookupReq = transactionLookupRequest;
            SetExecutionOrder(nameof(TransactionLookupReq));
        }

        public void Set(TransactionLookupResponse transactionLookupResponse)
        {
            TransactionLookupResp = transactionLookupResponse;
            SetExecutionOrder(nameof(TransactionLookupResp));
        }
    }
}