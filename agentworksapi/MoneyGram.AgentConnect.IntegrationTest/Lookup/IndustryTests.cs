using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class IndustryTests
    {
        private IndustryOperations _industryOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _industryOperations = new IndustryOperations(new TestRunner());
        }

        [TestMethod]
        public void Industry()
        {
            var industryData = new IndustryData();

            var agentLocation = AgentLocation.NY;
            industryData.IndustryResp = _industryOperations.Industry(agentLocation);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(industryData.IndustryResp.Errors.Any(),
                TestLogHelper.BusinessErrorsMsg(industryData.IndustryResp));
            Assert.IsTrue(industryData.IndustryResp.Payload.IndustryInfoList.Any(),
                TestLogHelper.AssertFailedMsg("Industry did not return any IndustryInfos."));
        }
    }
}