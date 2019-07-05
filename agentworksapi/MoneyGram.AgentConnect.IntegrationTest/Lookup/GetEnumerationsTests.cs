using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetEnumerationsTests
    {
        private GetEnumerationsOperations _getEnumerationsOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getEnumerationsOperations = new GetEnumerationsOperations(new TestRunner());
        }

        [TestMethod]
        public void GetEnumerations()
        {
            var getEnumerationsData = _getEnumerationsOperations.GetEnumerations(AgentLocation.MN, string.Empty);

            Assert.IsFalse(getEnumerationsData.GetEnumerationsResp.Errors.Any(), $"Errors: {Environment.NewLine}{getEnumerationsData.GetEnumerationsResp.Errors?.Log()}");
        }

        [TestMethod]
        public void GetEnumerationsForOccupation()
        {
            var getEnumerationData = _getEnumerationsOperations.GetEnumerations(AgentLocation.MN, EnumerationNames.OCCUPATION);

            Assert.IsFalse(getEnumerationData.GetEnumerationsResp.Errors.Any(), $"Errors: {Environment.NewLine}{getEnumerationData.GetEnumerationsResp.Errors?.Log()}");
            Assert.IsTrue(getEnumerationData.GetEnumerationsResp.Payload.Enumerations.Any(), "GetEnumerations for OCCUPATION returned empty");
        }

    }
}
