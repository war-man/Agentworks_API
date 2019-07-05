using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetBankDetailsOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        /// Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public GetBankDetailsOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        public GetBankDetailsResponse GetBankDetails(string fromLocation, string countryCode, string bankCode)
        {
            var request = GetBankDetailsRequestFactory.BaseRequest(countryCode, bankCode);
            return _acIntegration.GetBankDetails(fromLocation, request);
        }
    }
}