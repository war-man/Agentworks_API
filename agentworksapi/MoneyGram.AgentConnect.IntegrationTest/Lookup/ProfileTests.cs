using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class ProfileTests
    {
        private ProfileOperations _profileOperations { get; set; }
        private Agents _agents { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();
            _profileOperations = new ProfileOperations(testRunner);
            _agents = new Agents(testRunner.IsTrainingMode);
        }

        [TestMethod]
        public void Profile()
        {
            var profileData = new ProfileData();
            profileData.ProfileReq = ProfileRequestFactory.GenerateBaseProfileRequest();

            var agentLocation = AgentLocation.NY;
            var agent = _agents.GetAgent(agentLocation);
            profileData.ProfileReq.AgentID = agent.AgentId;
            profileData.ProfileReq.AgentSequence = agent.AgentSequence;
            profileData.ProfileResp = _profileOperations.Profile(profileData.ProfileReq);

            // ASSERT
            // Should not return errors
            Assert.IsFalse(profileData.ProfileResp.Errors.Any(),
                TestLogHelper.BusinessErrorsMsg(profileData.ProfileResp));
            // Should return ProductProfileItems
            Assert.IsTrue(profileData.ProfileResp.Payload.ProductProfileItem.Any(),
                TestLogHelper.AssertFailedMsg("Profile did not return any ProductProfileItems."));
            // Should return ProfileItems
            Assert.IsTrue(profileData.ProfileResp.Payload.ProfileItem.Any(),
                TestLogHelper.AssertFailedMsg("Profile did not return any ProfileItems."));
        }
    }
}