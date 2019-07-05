using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class GetBankDetailsByLevelRequestFactory
    {
        public static GetBankDetailsByLevelRequest BaseRequest(string countryCode, int requestedLevel, long? prevLevelSelection = 0)
        {
            return new GetBankDetailsByLevelRequest
            {
                CountryCode = countryCode,
                HierarchyLevelNumber = requestedLevel,
                PreviousLevelElementNumber = prevLevelSelection
            };
        }
    }
}