using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class ProfileOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        /// <summary>
        ///     Constructor. Sets up TestAgentConnectIntegration.
        /// </summary>
        /// <param name="testRunner"></param>
        public ProfileOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        /// <summary>
        ///     Get Profile
        /// </summary>
        /// <param name="agentLocation"></param>
        /// <returns></returns>
        public ProfileResponse Profile(ProfileRequest profileRequest)
        {
            return _acIntegration.Profile(profileRequest);
        }
    }
}