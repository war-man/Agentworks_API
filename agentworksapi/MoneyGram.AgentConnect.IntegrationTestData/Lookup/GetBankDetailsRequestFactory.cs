using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public static class GetBankDetailsRequestFactory
    {
        public static GetBankDetailsRequest BaseRequest(string countryCode, string bankCode)
        {
            return new GetBankDetailsRequest
            {
                CountryCode = countryCode,
                InfoKey = InfoKeyNames.bankCode,
                Value = bankCode
            };
        }
    }
}