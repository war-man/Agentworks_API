using System.Collections.Generic;
using MoneyGram.AgentConnect.UpgradeUtility.Generators;
using MoneyGram.AgentConnect.UpgradeUtility.Generators.AgentConnectRepository;
using MoneyGram.AgentConnect.UpgradeUtility.Generators.IAgentConnect;

namespace MoneyGram.AgentConnect.UpgradeUtility
{
    class Program
    {
        private static List<IGenerator> generators = new List<IGenerator>
        {
            new IAgentConnectGenerator(),
            new AgentConnectRepositoryGenerator(),
            new AgentConnectDecoratorGenerator(),
            new AgentConnectModelsGenerator(),
            new DomainToServiceExtensionsGenerator(),
            new ServiceToDomainExtensionsGenerator()
        };
        
        public static void Main(string[] args)
        {
            foreach (var generator in generators)
            {
                generator.Generate();
            }
        }
    }
}
