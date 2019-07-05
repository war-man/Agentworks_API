using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class IndustryOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        ///     Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public IndustryOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        ///     Get Industries
        /// </summary>
        /// <param name="agentLocation"></param>
        /// <returns></returns>
        public IndustryResponse Industry(string agentLocation)
        {
            var request = new IndustryRequest();
            return _acIntegration.Industry(agentLocation, request);
        }
    }
}