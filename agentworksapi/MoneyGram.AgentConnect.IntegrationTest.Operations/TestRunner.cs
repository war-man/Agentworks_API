using System.Net;
using MoneyGram.AgentConnect;
using Unity;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations
{
    public class TestRunner
    {
        private readonly IUnityContainer Container;
        public IAgentConnect AgentConnect;

        public bool IsTrainingMode { get; }

        public TestRunner(bool isTrainingMode = false)
        {
            IsTrainingMode = isTrainingMode;
            Container = TestRunnerUnityContainer.GetUnityContainerInstance();
            AgentConnect = TestRunnerUnityContainer.GetAgentConnectInstance(IsTrainingMode);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
    }
} 