using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetBankDetailsTest
    {
        private GetBankDetailsOperations _getBankDetailsOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getBankDetailsOperations = new GetBankDetailsOperations(new TestRunner());
        }

        [TestMethod]
        public void GetBankDetails()
        {
            var getBankDetailsResp = _getBankDetailsOperations.GetBankDetails(AgentLocation.MN, Country.India, BankIdentifier.ABHY.ToString());

            // ASSERT ALL THE THINGS
        }
    }
}