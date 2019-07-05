using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class GetAllFieldsRequestFactory
    {
        public static GetAllFieldsRequest GetAllFieldsRequest(string transactionType, string cachedVersion)
        {
            return new GetAllFieldsRequest
            {
                CachedVersion = cachedVersion,
                TransactionType = transactionType
            };
        }
    }
}