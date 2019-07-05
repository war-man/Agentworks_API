using MoneyGram.AgentConnect;
using Unity;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations
{
    public static class TestRunnerUnityContainer
    {
        private static UnityContainer _container { get; set; }
        private static IAgentConnect _agentConnect { get; set; }

        private static IAgentConnect _trainingAgentConnect { get; set; }

        public static UnityContainer GetUnityContainerInstance()
        {
            return _container;
        }
        public static IAgentConnect GetAgentConnectInstance(bool isTrainingMode = false)
        {
            return isTrainingMode? _trainingAgentConnect : _agentConnect;
        }
        public static void SetAgentConnectInstance(IAgentConnect agentConnect)
        {
            _agentConnect = agentConnect;
        }
        public static void SetUnityContainerInstance(UnityContainer container)
        {
            _container = container;
        }

        public static void SetTrainingAgentConnect(IAgentConnect agentConnect)
        {
            _trainingAgentConnect = agentConnect;
        }
    }
}