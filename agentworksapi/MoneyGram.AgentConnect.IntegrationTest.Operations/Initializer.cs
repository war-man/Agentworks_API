using AwApi.Integration;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using MoneyGram.AgentConnect;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Cache.InMemory;
using Unity;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations
{
    public static class Initializer
    {
        public static void Initialize()
        {
            var Container = new UnityContainer();
            Container.RegisterType<ICacheManager, InMemoryCacheManager>();
            Container.RegisterType<IAgentConnect, AgentConnectRepository>();
            Container.RegisterType<IAgentConnectProxyFactory, AgentConnectProxyFactory>();
            Container.RegisterType<IAgentConnectConfig, AgentConnectConfigCoded>();
            Container.RegisterType<ITrainingModeRepository, TrainingModeRepository>();
            Container.RegisterType<TestAgentConnectIntegration, TestAgentConnectIntegration>();
            TestRunnerUnityContainer.SetUnityContainerInstance(Container);
            TestRunnerUnityContainer.SetAgentConnectInstance(Container.Resolve<IAgentConnect>());
            TestRunnerUnityContainer.SetTrainingAgentConnect(new AgentConnectTraining(
                Container.Resolve<IAgentConnect>(),
                Container.Resolve<ICacheManager>(),
                Container.Resolve<ITrainingModeRepository>()));
        }
    }
}