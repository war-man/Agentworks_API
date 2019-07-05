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
    public class GetProfileReceiverTest
    {
        private ConsumerOperations _consumerOperations { get; set; }
        private string _agentState { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _agentState = AgentLocation.MN;
            _consumerOperations = new ConsumerOperations(new TestRunner());
        }

        private GetProfileReceiverResponse GetProfileReceiver(string profileId, string profileTypeId)
        {
            var getProfileRequest = new INTEGRATION.GetProfileReceiverRequest
            {
                ReceiverProfileID = profileId,
                AgentState = _agentState,
                ReceiverProfileIDType = profileTypeId
            };
            getProfileRequest.PopulateAgentData(getProfileRequest.AgentState);
            var getProfileData = new GetProfileReceiverData(getProfileRequest);
            return _consumerOperations.GetProfileReceiver(getProfileData);
        }

        private CreateOrUpdateProfileReceiverResponse CreateProfile()
        {
            var createRequest = new INTEGRATION.CreateOrUpdateProfileReceiverRequest
            {
                AgentState = _agentState
            };
            createRequest.PopulateAgentData(createRequest.AgentState);
            var createData = new CreateOrUpdateProfileReceiverData(createRequest);
            return _consumerOperations.CreateProfileReceiver(createData);
        }

        [TestMethod]
        public void GetExistingProfileReceiver()
        {
            var createdProfile = CreateProfile().Payload.ConsumerProfileIDs.First();
            var getProfileResponse = GetProfileReceiver(createdProfile.ConsumerProfileID, createdProfile.ConsumerProfileIDType);
            var profileId = _consumerOperations.GetReceiverProfileId(getProfileResponse.Payload.CurrentValues);
            Assert.AreEqual(createdProfile.ConsumerProfileID, profileId);
            Assert.IsTrue(getProfileResponse.Payload.CurrentValues.Count != 0);
            Assert.IsTrue(getProfileResponse.Errors.Count == 0);
        }

        [TestMethod]
        public void GetNotExistingProfileReceiver()
        {
            try
            {
                var getProfileResponse = GetProfileReceiver("9999999A", "TRANSIENT");
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