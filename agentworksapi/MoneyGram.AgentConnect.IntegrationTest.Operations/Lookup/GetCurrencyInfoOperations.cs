using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetCurrencyInfoOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        /// <summary>
        /// Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public GetCurrencyInfoOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        /// Get country info by lookup key
        /// </summary>
        /// <param name="lookupKey"></param>
        /// <returns></returns>
        public GetCurrencyInfoResponse GetCurrencyInfo(string location, GetCurrencyInfoRequest req)
        {
            var getCurrencyInfoResp = _acIntegration.GetCurrencyInfo(location, req);
            return getCurrencyInfoResp;
        }

        public string GetStaleVersion(string currentVersion)
        {
            // currentVersion is formatted 20161201050338
            var year = int.Parse(currentVersion.Substring(0, 4));
            var month = int.Parse(currentVersion.Substring(4, 2));
            var date = int.Parse(currentVersion.Substring(6, 2));
            var suffix = currentVersion.Substring(8, 6);

            var versionDate = new DateTime(year, month, date);
            var staleVersionDate = versionDate.AddDays(-1);
            var staleVersion = $"{staleVersionDate.Year}{staleVersionDate.Month:D2}{staleVersionDate.Day:D2}{suffix}";

            return staleVersion;
        }
    }
}