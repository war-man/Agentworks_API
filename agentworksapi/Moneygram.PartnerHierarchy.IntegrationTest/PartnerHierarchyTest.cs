using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.PartnerHierarchy.IntegrationTest.Data.Repositories;
using MoneyGram.PartnerHierarchy.IntegrationTest.Operations.MoAndLocation;

namespace Moneygram.PartnerHierarchy.IntegrationTest
{
    [TestClass]
    public class PartnerHierarchyTest
    {        
        private MoAndLocationRepository _testDataRepository;
        private MoAndLocationOperations _locationOperations;
        private string _clientUrl;
        private string _environment;

        [TestInitialize]
        public void TestInitialize()
        {            
            _testDataRepository = new MoAndLocationRepository();
            _clientUrl = EnvUrl();
            _locationOperations = new MoAndLocationOperations(_clientUrl);            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        [TestMethod]
        public void TestGetPartnerHierarchyAgent()
        {
            var details = _testDataRepository.MoAndLocations.LocationDetails.Where(x => x.AcEnv == _environment)
                .ToList();
            Assert.IsTrue(details.Count>0, $"Defined test data for environment {_environment} are empty");
            foreach (var detail in _testDataRepository.MoAndLocations.LocationDetails)
            {
                var response = _locationOperations.GetPartnerHierarchyAgent(detail.MgiMainOfficeId, detail.MgiDeviceAgentLocationId);
                var agent = response.GetAgent();
                Assert.IsNotNull(agent);
                Assert.AreEqual(detail.AgentStatus, agent.Status);
                Assert.AreEqual(detail.OracleAccountNumber, agent.OracleAccountNumber);
            }
        }

        private string EnvUrl()
        {
            var testSettings = TestConfig.TestSettings;
            _environment = testSettings.AcEnvironment;
            switch (testSettings.AcEnvironment)
            {
                case AcEnv.D1:
                    return "https://d1coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.D2:
                    return "https://d2coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.D3:
                    return "https://d3coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.D5:
                    return "https://d5coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.Q1:
                    return "https://q1coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.Q2:
                    return "https://q2coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.Q3:
                    return "https://q3coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.Q5:
                    return "https://q5coreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                case AcEnv.External:
                    return "https://extcoreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
                default:
                    return "https://extcoreservices.qacorp.moneygram.com/partnerdata/v1/partnerhierarchy";
            }
        }
    }
}
