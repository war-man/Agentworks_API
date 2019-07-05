using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class CreateOrUpdateProfileReceiverTest
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
        public void CreateNewReceiverProfile_ProfileCreated()
        {
            var createResponse = CreateProfile();
            Assert.IsFalse(createResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{createResponse.Errors?.Log()}");            
            Assert.IsNotNull(createResponse.Payload.ConsumerProfileIDs);
            Assert.IsTrue(createResponse.Payload.ConsumerProfileIDs.Any() &&
                          !string.IsNullOrWhiteSpace(createResponse.Payload.ConsumerProfileIDs.First()
                              .ConsumerProfileID));
        }

        [TestMethod]
        public void UpdateExistingReceiverProfile_ProfileUpdated()
        {
            var createResponse = CreateProfile();

            #region Get profile

            var getProfileResponse = GetProfileReceiver(createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID, createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileIDType);

            Assert.IsFalse(getProfileResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{getProfileResponse.Errors?.Log()}");
            Assert.IsNotNull(getProfileResponse.Payload, "Payload is null");
            Assert.IsNotNull(getProfileResponse.Payload.CurrentValues, "There are no values related with profile");
            Assert.IsTrue(getProfileResponse.Payload.CurrentValues.Count != 0,
                "There are no values related with profile");
            #endregion

            #region Update profile

            var addressToChange = _consumerOperations.GetCityAddressOfReceiver(getProfileResponse);
            var updateRequest = new INTEGRATION.CreateOrUpdateProfileReceiverRequest
            {
                AgentState = _agentState,
            };
            updateRequest.PopulateAgentData(updateRequest.AgentState);
            var updateData = new CreateOrUpdateProfileReceiverData(updateRequest);
            var values = getProfileResponse.Payload.CurrentValues.Where(x =>
                x.InfoKey != InfoKeyNames.issue_Date && x.InfoKey != InfoKeyNames.last_Modified_Date
                                                     && x.InfoKey != InfoKeyNames.issue_Date.Replace("_", "") &&
                                                     x.InfoKey != InfoKeyNames.last_Modified_Date.Replace("_", "")
            ).ToList();
            var newAddress = _consumerOperations.SetRandomAddressCityOfReceiver(values);
            var generatorCounter = 0;
            while (newAddress.Equals(addressToChange) && generatorCounter < 5)
            {
                newAddress = _consumerOperations.SetRandomAddressCityOfReceiver(values);
                generatorCounter++;
            }

            var createdProfile = createResponse.Payload.ConsumerProfileIDs.First();

            Assert.AreNotEqual(newAddress, addressToChange, "Could not change address to random one");
            var updateResponse = _consumerOperations.UpdateProfileReceiverData(updateData, values,
                createdProfile.ConsumerProfileID, createdProfile.ConsumerProfileIDType);
            Assert.IsFalse(updateResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{updateResponse.Errors?.Log()}");            
            Assert.IsNotNull(updateResponse.Payload.ConsumerProfileIDs);
            Assert.IsTrue(updateResponse.Payload.ConsumerProfileIDs.Any() &&
                          !string.IsNullOrWhiteSpace(updateResponse.Payload.ConsumerProfileIDs.First()
                              .ConsumerProfileID));

            var updatedProfile = GetProfileReceiver(createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID,
                createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileIDType);

            var updatedAddress = _consumerOperations.GetCityAddressOfReceiver(updatedProfile);
            Assert.AreEqual(newAddress, updatedAddress);

            #endregion            
        }

        [TestMethod]
        public void CreateOrUpdateProfileReceiver_FieldsToCollectReturned()
        {
            var request = new INTEGRATION.CreateOrUpdateProfileReceiverRequest
            {
                AgentState = _agentState
            };
            request.PopulateAgentData(request.AgentState);
            var data = new CreateOrUpdateProfileReceiverData(request);
            var response = _consumerOperations.CreateOrUpdateProfileReceiverBaseData(data);

            Assert.IsFalse(response.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            var containsCategory = ContainsCategory(response.Payload.FieldsToCollect, InfoKeyCategories.receiverProfileSaveOrUpdateInfoSet);

            Assert.IsTrue(containsCategory, "Ensure that profile editor limits for create or update profile Receiver are configured properly in the current environment");
        }

        private bool ContainsCategory(List<InfoBase> infos, string categoryKey)
        {
            return infos.OfType<CategoryInfo>()
                .Any(categoryInfo => categoryInfo.InfoKey.ToLower() == categoryKey.ToLower() || ContainsCategory(categoryInfo.Infos, categoryKey));
        }
    }
}