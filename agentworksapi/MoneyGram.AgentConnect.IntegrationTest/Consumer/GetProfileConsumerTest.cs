using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Consumer;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using INTEGRATION = MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Consumer
{
    [TestClass]
    public class GetProfileConsumerTest
    {
        private ConsumerOperations _consumerOperations { get;set; }
        private string _agentState { get;set; }

        [TestInitialize]
        public void TestSetup()
        {
            _agentState = AgentLocation.MN;
            _consumerOperations = new ConsumerOperations(new TestRunner());
        }

        private GetProfileConsumerResponse GetProfileConsumer(string profileId, string profileTypeId)
        {
            var getProfileRequest = new INTEGRATION.GetProfileConsumerRequest()
            {
                ConsumerProfileID = profileId,
                AgentState = _agentState,
                ConsumerProfileIDType = profileTypeId
            };
            getProfileRequest.PopulateAgentData(getProfileRequest.AgentState);
            var getProfileData = new GetProfileConsumerData(getProfileRequest);
            return _consumerOperations.GetProfileConsumer(getProfileData);
        }

        private CreateOrUpdateProfileConsumerResponse CreateProfile()
        {
            var createRequest = new INTEGRATION.CreateOrUpdateProfileConsumerRequest()
            {
                AgentState = _agentState
            };
            createRequest.PopulateAgentData(createRequest.AgentState);
            var createData = new CreateOrUpdateProfileConsumerData(createRequest);
            return _consumerOperations.CreateProfileConsumer(createData);
        }

        [TestMethod]
        public void GetExistingProfileConsumer()
        {
            var createdProfile = CreateProfile().Payload.ConsumerProfileIDs.First();
            var getProfileResponse = GetProfileConsumer(createdProfile.ConsumerProfileID, createdProfile.ConsumerProfileIDType);
            var profileId = _consumerOperations.GetConsumerProfileId(getProfileResponse.Payload.CurrentValues);
            Assert.AreEqual(createdProfile.ConsumerProfileID, profileId);
            Assert.IsTrue(getProfileResponse.Payload.CurrentValues.Count != 0);
            Assert.IsTrue(getProfileResponse.Errors.Count == 0);
        }

        [TestMethod]
        public void GetNotExistingProfileConsumer()
        {            
            try
            {
                var getProfileResponse = GetProfileConsumer("9999999A", "TRANSIENT");
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
