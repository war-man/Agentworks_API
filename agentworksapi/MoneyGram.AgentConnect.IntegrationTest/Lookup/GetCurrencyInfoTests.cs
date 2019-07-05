using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetCurrencyInfoTests
    {
        private GetCurrencyInfoOperations _getCurrencyInfoOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getCurrencyInfoOperations = new GetCurrencyInfoOperations(new TestRunner());
        }

        private GetCurrencyInfoData GetCurrencyInfo(string agentLocation, string version)
        {
            // TEST WITHOUT VERSION
            var getCurrencyInfoData = new GetCurrencyInfoData();
            getCurrencyInfoData.GetCurrencyInfoReq = new GetCurrencyInfoRequest
            {
                Version = version
            };
            getCurrencyInfoData.GetCurrencyInfoResp = _getCurrencyInfoOperations.GetCurrencyInfo(agentLocation, getCurrencyInfoData.GetCurrencyInfoReq);
            return getCurrencyInfoData;
        }

        [TestMethod]
        public void GetCurrencyInfo_NoVersion()
        {
            var getCurrencyInfoData = GetCurrencyInfo(AgentLocation.NY, string.Empty);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCurrencyInfoData.GetCurrencyInfoResp.Errors.Any(),
                TestLogHelper.BusinessErrorsMsg(getCurrencyInfoData.GetCurrencyInfoResp));
            // Should return CurrencyInfos
            Assert.IsTrue(getCurrencyInfoData.GetCurrencyInfoResp.Payload.CurrencyInfos.Any(),
                TestLogHelper.AssertFailedMsg("GetCurrencyInfo requested without version did not return CurrencyInfos, should return all Currencies"));
        }

        [TestMethod]
        public void GetCurrencyInfo_WithCurrentVersion()
        {
            var getCurrencyInfoData = GetCurrencyInfo(AgentLocation.NY, string.Empty);
            getCurrencyInfoData = GetCurrencyInfo(AgentLocation.NY, getCurrencyInfoData.GetCurrencyInfoResp.Payload.Version);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCurrencyInfoData.GetCurrencyInfoResp.Errors.Any(),
                TestLogHelper.BusinessErrorsMsg(getCurrencyInfoData.GetCurrencyInfoResp));
            // Should not return CurrencyInfos
            Assert.IsFalse(getCurrencyInfoData.GetCurrencyInfoResp.Payload.CurrencyInfos.Any(),
                TestLogHelper.AssertFailedMsg("GetCurrencyInfo requested with current version returned CurrencyInfos, should return none"));
        }

        [TestMethod]
        public void GetCurrencyInfo_WithStaleVersion()
        {
            var getCurrencyInfoData = GetCurrencyInfo(AgentLocation.NY, string.Empty);
            var staleVersion = _getCurrencyInfoOperations.GetStaleVersion(getCurrencyInfoData.GetCurrencyInfoResp.Payload.Version);
            getCurrencyInfoData = GetCurrencyInfo(AgentLocation.NY, staleVersion);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCurrencyInfoData.GetCurrencyInfoResp.Errors.Any(),
                TestLogHelper.BusinessErrorsMsg(getCurrencyInfoData.GetCurrencyInfoResp));
            // Should return CurrencyInfos
            Assert.IsTrue(getCurrencyInfoData.GetCurrencyInfoResp.Payload.CurrencyInfos.Any(),
                TestLogHelper.AssertFailedMsg("GetCurrencyInfo requested with stale version did not return CurrencyInfos, should return all CurrencyInfos"));
        }
    }
}