using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetCountrySubdivisionOperations
    {
        private ITestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        /// Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public GetCountrySubdivisionOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        /// Get country info by lookup key
        /// </summary>
        /// <param name="getCountrySubdivisionReq"></param>
        /// <returns></returns>
        public GetCountrySubdivisionResponse GetCountrySubdivision(string agentLocation, GetCountrySubdivisionRequest getCountrySubdivisionReq)
        {
            var getCountrySubdivisionResp = _acIntegration.GetCountrySubdivision(agentLocation, getCountrySubdivisionReq);
            return getCountrySubdivisionResp;
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