using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetCountrySubdivisionTests
    {
        private GetCountrySubdivisionOperations _getCountrySubdivisionOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getCountrySubdivisionOperations = new GetCountrySubdivisionOperations(new TestRunner());
        }

        [TestMethod]
        public void GetCountrySubdivision()
        {
            var agentLocation = AgentLocation.NY;

            var getCountrySubdivisionData = new GetCountrySubdivisionData();
            getCountrySubdivisionData.GetCountrySubdivisionReq = new GetCountrySubdivisionRequest
            {
                CachedVersion = null,
                CountryCode = null
            };
            getCountrySubdivisionData.GetCountrySubdivisionResp = _getCountrySubdivisionOperations.GetCountrySubdivision(agentLocation, getCountrySubdivisionData.GetCountrySubdivisionReq);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountrySubdivisionData.GetCountrySubdivisionResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountrySubdivisionData.GetCountrySubdivisionResp.Errors?.Log()}");
            // Should return CountrySubdivisionInfos
            Assert.IsTrue(getCountrySubdivisionData.GetCountrySubdivisionResp.Payload.CountrySubdivisionInfos.Any());

            // Request with CachedVersion set to the version of the previous response
            getCountrySubdivisionData.GetCountrySubdivisionReq.CachedVersion = getCountrySubdivisionData.GetCountrySubdivisionResp.Payload.Version;

            getCountrySubdivisionData.GetCountrySubdivisionResp = _getCountrySubdivisionOperations.GetCountrySubdivision(agentLocation, getCountrySubdivisionData.GetCountrySubdivisionReq);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountrySubdivisionData.GetCountrySubdivisionResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountrySubdivisionData.GetCountrySubdivisionResp.Errors?.Log()}");
            // Should not return CountrySubdivisionInfos
            Assert.IsFalse(getCountrySubdivisionData.GetCountrySubdivisionResp.Payload.CountrySubdivisionInfos.Any());

            var staleVersion = _getCountrySubdivisionOperations.GetStaleVersion(getCountrySubdivisionData.GetCountrySubdivisionResp.Payload.Version);
            // Request with CachedVersion set to the version of the previous response
            getCountrySubdivisionData.GetCountrySubdivisionReq.CachedVersion = staleVersion;

            getCountrySubdivisionData.GetCountrySubdivisionResp = _getCountrySubdivisionOperations.GetCountrySubdivision(agentLocation, getCountrySubdivisionData.GetCountrySubdivisionReq);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(getCountrySubdivisionData.GetCountrySubdivisionResp.Errors.Any(), $"{Environment.NewLine}Errors: {Environment.NewLine}{getCountrySubdivisionData.GetCountrySubdivisionResp.Errors?.Log()}");
            // Should return CountrySubdivisionInfos
            Assert.IsTrue(getCountrySubdivisionData.GetCountrySubdivisionResp.Payload.CountrySubdivisionInfos.Any());
        }
    }
}