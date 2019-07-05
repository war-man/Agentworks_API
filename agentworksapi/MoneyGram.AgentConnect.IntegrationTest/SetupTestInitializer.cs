using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Operations;

namespace MoneyGram.AgentConnect.IntegrationTest
{
    [TestClass]
    public class SetupTestInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Initializer.Initialize();
        }
    }
}