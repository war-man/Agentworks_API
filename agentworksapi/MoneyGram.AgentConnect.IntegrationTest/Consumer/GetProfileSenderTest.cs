using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Consumer;
using INTEGRATION = MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Consumer
{
    [TestClass]
    public class GetProfileSenderTest
    {
        private ConsumerOperations _consumerOperations { get; set; }
        private string _agentState { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _agentState = AgentLocation.MN;
            _consumerOperations = new ConsumerOperations(new TestRunner());
        }

        private GetProfileSenderResponse GetProfileSender(string profileId, string profileTypeId)
        {
            var getProfileRequest = new INTEGRATION.GetProfileSenderRequest
            {
                SenderProfileID = profileId,
                AgentState = _agentState,
                SenderProfileIDType = profileTypeId
            };
            getProfileRequest.PopulateAgentData(getProfileRequest.AgentState);
            var getProfileData = new GetProfileSenderData(getProfileRequest);
            return _consumerOperations.GetProfileSender(getProfileData);
        }

        private CreateOrUpdateProfileSenderResponse CreateProfile()
        {
            var createRequest = new INTEGRATION.CreateOrUpdateProfileSenderRequest
            {
                AgentState = _agentState
            };
            createRequest.PopulateAgentData(createRequest.AgentState);
            var createData = new CreateOrUpdateProfileSenderData(createRequest);
            return _consumerOperations.CreateProfileSender(createData);
        }

        [TestMethod]
        public void GetExistingProfileSender()
        {
            var createdProfile = CreateProfile().Payload.ConsumerProfileIDs.First();
            var getProfileResponse = GetProfileSender(createdProfile.ConsumerProfileID, createdProfile.ConsumerProfileIDType);
            var profileId = _consumerOperations.GetSenderProfileId(getProfileResponse.Payload.CurrentValues);
            Assert.AreEqual(createdProfile.ConsumerProfileID, profileId);
            Assert.IsTrue(getProfileResponse.Payload.CurrentValues.Count != 0);
            Assert.IsTrue(getProfileResponse.Errors.Count == 0);
        }

        [TestMethod]
        public void GetNotExistingProfileSender()
        {
            try
            {
                var getProfileResponse = GetProfileSender("9999999A", "TRANSIENT");
                Assert.IsTrue(getProfileResponse.Errors.Count > 0);
            }
            catch (AgentConnectException e)
            {
                Assert.IsNotNull(e);
                Assert.AreEqual(e.ErrorCode, 3413);
            }
        }
    }
}