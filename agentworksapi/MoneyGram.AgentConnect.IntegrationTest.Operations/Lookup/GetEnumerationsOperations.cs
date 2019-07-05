using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetEnumerationsOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        public GetEnumerationsOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);

        }

        /// <summary>
        /// Gets enumerations from AC.
        /// </summary>
        /// <param name="fromLocation">Agent Location</param>
        /// <param name="enumerationName">If empty, returns all enumeration lists in the response.</param>
        /// <returns></returns>
        public GetEnumerationsData GetEnumerations(string fromLocation, string enumerationName)
        {
            var getEnumerationsData = new GetEnumerationsData();
            getEnumerationsData.GetEnumerationsReq = GetEnumerationsRequestFactory.GetEnumerationsRequest(enumerationName);
            getEnumerationsData.GetEnumerationsResp = _acIntegration.GetEnumerations(fromLocation, getEnumerationsData.GetEnumerationsReq);
            return getEnumerationsData;
        }
    }
}