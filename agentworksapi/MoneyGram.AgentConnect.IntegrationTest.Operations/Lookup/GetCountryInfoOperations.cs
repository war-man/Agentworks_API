using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetCountryInfoOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        /// Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public GetCountryInfoOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        /// Get country info by lookup key
        /// </summary>
        /// <param name="agentLocation"></param>
        /// <param name="lookupKey"></param>
        /// <param name="cachedVersion"></param>
        /// <returns></returns>
        public GetCountryInfoResponse GetCountryInfo(string agentLocation, string lookupKey, string cachedVersion = null)
        {
            var request = new GetCountryInfoRequest
            {
                CachedVersion = cachedVersion,
                LookupKey = lookupKey
            };
            return _acIntegration.GetCountryInfo(agentLocation, request);
        }

        public string GetStaleVersion(string currentVersion)
        {
            // currentVersion is formatted 1.1.20170109
            var prefix = currentVersion.Substring(0, 4);
            var year = int.Parse(currentVersion.Substring(4, 4));
            var month = int.Parse(currentVersion.Substring(8, 2));
            var date = int.Parse(currentVersion.Substring(10, 2));

            var versionDate = new DateTime(year, month, date);
            var staleVersionDate = versionDate.AddDays(-1);
            var staleVersion = $"{prefix}{staleVersionDate.Year}{staleVersionDate.Month:D2}{staleVersionDate.Day:D2}";

            return staleVersion;
        }
    }
}