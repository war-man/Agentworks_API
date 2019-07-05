using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class GetEnumerationsRequestFactory
    {
        public static GetEnumerationsRequest GetEnumerationsRequest(string enumerationName)
        {
            return new GetEnumerationsRequest
            {
                EnumerationName = enumerationName
            };
        }
    }
}