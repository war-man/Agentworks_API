using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetBankDetailsByLevelOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        /// Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public GetBankDetailsByLevelOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        /// Get bank details based on country code and level
        /// </summary>
        /// <param name="fromLocation"></param>
        /// <param name="countryCode"></param>
        /// <param name="requestedLevel"></param>
        /// <param name="prevLevelSelection"></param>
        /// <returns></returns>
        private GetBankDetailsByLevelResponse GetBankDetailsByLevel(string fromLocation, string countryCode, int requestedLevel, long prevLevelSelection = 0)
        {
            var request = GetBankDetailsByLevelRequestFactory.BaseRequest(countryCode, requestedLevel, prevLevelSelection);
            return _acIntegration.GetBankDetailsByLevel(fromLocation, request);
        }

        public GetBankDetailsByLevelResponse GetBanks(string fromLocation, string countryCode)
        {
            return GetBankDetailsByLevel(fromLocation, countryCode, BankLevel.Bank);
        }

        public GetBankDetailsByLevelResponse GetStateDetails(string fromLocation, string countryCode, long bankElementNumber)
        {
            return GetBankDetailsByLevel(fromLocation, countryCode, BankLevel.State, bankElementNumber);
        }

        public GetBankDetailsByLevelResponse GetCityDetails(string fromLocation, string countryCode, long cityElementNumber)
        {
            return GetBankDetailsByLevel(fromLocation, countryCode, BankLevel.City, cityElementNumber);
        }

        public GetBankDetailsByLevelResponse GetBranchDetails(string fromLocation, string countryCode, long branchElementNumber)
        {
            return GetBankDetailsByLevel(fromLocation, countryCode, BankLevel.Branch, branchElementNumber);
        }

        public string GetIFSC(string fromLocation, string countryCode, long branchElementNumber)
        {
            var response = GetBankDetailsByLevel(fromLocation, countryCode, BankLevel.Branch, branchElementNumber);
            var ifscAttribute = response.Payload?.HierarchyLevelInfos?[0].Attributes.Find(x => x.AttributeLabel == "IFSC").AttributeValue; ;

            return (ifscAttribute);
        }

    }
}