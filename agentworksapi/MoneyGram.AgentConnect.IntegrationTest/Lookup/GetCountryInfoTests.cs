using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetCountryInfoTests
    {
        private GetCountryInfoOperations _getCountryInfoOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getCountryInfoOperations = new GetCountryInfoOperations(new TestRunner());
        }

        [TestMethod]
        public void GetCountryInfo_SendActive()
        {
            // Request without cached version
            var getCountryInfoData = new GetCountryInfoData();
            getCountryInfoData.SendActiveCountriesResp = _getCountryInfoOperations.GetCountryInfo(AgentLocation.MN, CountryLookupKey.SendActive);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountryInfoData.SendActiveCountriesResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountryInfoData.SendActiveCountriesResp.Errors?.Log()}");
            // Should return CountryInfos
            Assert.IsTrue(getCountryInfoData.SendActiveCountriesResp.Payload.CountryInfos.Any());

            // Request with cachedVersion set to the version of the previous response
            getCountryInfoData.SendActiveCountriesResp = _getCountryInfoOperations.GetCountryInfo(AgentLocation.MN, CountryLookupKey.SendActive,
                getCountryInfoData.SendActiveCountriesResp.Payload.Version);
            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountryInfoData.SendActiveCountriesResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountryInfoData.SendActiveCountriesResp.Errors?.Log()}");
            // Should contain no CountryInfos
            Assert.IsFalse(getCountryInfoData.SendActiveCountriesResp.Payload.CountryInfos.Any());

            var staleVersion = _getCountryInfoOperations.GetStaleVersion(getCountryInfoData.SendActiveCountriesResp.Payload.Version);

            // Request with cachedVersion set to the version of the previous response minus a day
            getCountryInfoData.SendActiveCountriesResp = _getCountryInfoOperations.GetCountryInfo(AgentLocation.MN, CountryLookupKey.SendActive, staleVersion);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountryInfoData.SendActiveCountriesResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountryInfoData.SendActiveCountriesResp.Errors?.Log()}");
            // Should return CountryInfos
            Assert.IsTrue(getCountryInfoData.SendActiveCountriesResp.Payload.CountryInfos.Any());
        }
    }
}