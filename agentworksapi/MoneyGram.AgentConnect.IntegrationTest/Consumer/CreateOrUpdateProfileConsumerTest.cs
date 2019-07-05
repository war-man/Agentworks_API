using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class CreateOrUpdateProfileConsumerTest
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
        public void CreateNewConsumerProfile()
        {
            var createResponse = CreateProfile();
            Assert.IsFalse(createResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{createResponse.Errors?.Log()}");            
            Assert.IsNotNull(createResponse.Payload.ConsumerProfileIDs);
            Assert.IsTrue(createResponse.Payload.ConsumerProfileIDs.Any() &&
                          !string.IsNullOrWhiteSpace(createResponse.Payload.ConsumerProfileIDs.First()
                              .ConsumerProfileID));
        }

        [TestMethod]
        public void UpdateExistingConsumerProfile()
        {
            var createResponse = CreateProfile();

            #region Get profile
            
            var getProfileResponse = GetProfileConsumer(createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID, createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileIDType);
            
            Assert.IsFalse(getProfileResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{getProfileResponse.Errors?.Log()}");
            Assert.IsNotNull(getProfileResponse.Payload,"Payload is null");
            Assert.IsNotNull(getProfileResponse.Payload.CurrentValues, "There are no values related with profile");
            Assert.IsTrue(getProfileResponse.Payload.CurrentValues.Count != 0,
                "There are no values related with profile");
            #endregion

            #region Update profile

            var cityToChange = _consumerOperations.GetCityAddressOfConsumer(getProfileResponse);
            var updateRequest = new INTEGRATION.CreateOrUpdateProfileConsumerRequest()
            {
                AgentState = _agentState,
            };
            updateRequest.PopulateAgentData(updateRequest.AgentState);
            var updateData = new CreateOrUpdateProfileConsumerData(updateRequest);
            var values = getProfileResponse.Payload.CurrentValues.Where(x =>
                x.InfoKey != InfoKeyNames.issue_Date && x.InfoKey != InfoKeyNames.last_Modified_Date
                                                     && x.InfoKey != InfoKeyNames.issue_Date.Replace("_", "") &&
                                                     x.InfoKey != InfoKeyNames.last_Modified_Date.Replace("_", "")
            ).ToList();
            var newCity = _consumerOperations.SetRandomAddressCityOfConsumer(values);
            var generatorCounter = 0;
            while (newCity.Equals(cityToChange) && generatorCounter < 5)
            {
                newCity = _consumerOperations.SetRandomAddressCityOfConsumer(values);
                generatorCounter++;
            }

            Assert.AreNotEqual(cityToChange, newCity, "Could not generate random city value");

            var createdProfile = createResponse.Payload.ConsumerProfileIDs.First();
            var updateResponse = _consumerOperations.UpdateProfileConsumerData(updateData, values,
                createdProfile.ConsumerProfileID, createdProfile.ConsumerProfileIDType);
            Assert.IsFalse(updateResponse.Errors.Any(),
                $" {Environment.NewLine}Errors: {Environment.NewLine}{updateResponse.Errors?.Log()}");            
            Assert.IsNotNull(updateResponse.Payload.ConsumerProfileIDs);
            Assert.AreEqual(updateResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID,
                createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID);

            var updatedProfile = GetProfileConsumer(createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID,
                createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileIDType);

            var updatedCity = _consumerOperations.GetCityAddressOfConsumer(updatedProfile);
            Assert.AreEqual(newCity, updatedCity);
            #endregion            
        }

        [TestMethod]
        public void CreateOrUpdateProfileConsumer_ValidateShouldReturnFieldsToCollect()
        {
            var request = new INTEGRATION.CreateOrUpdateProfileConsumerRequest()
            {
                AgentState = _agentState
            };
            request.PopulateAgentData(request.AgentState);
            var data = new CreateOrUpdateProfileConsumerData(request);
            var response = _consumerOperations.CreateOrUpdateProfileConsumerBaseData(data);
            
            Assert.IsFalse(response.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            var containsFirstIdCategory = ContainsCategory(response.Payload.FieldsToCollect,
                InfoKeyCategories.profileSaveOrUpdateInfoSet);
            var containsSecondIdCategory = ContainsCategory(response.Payload.FieldsToCollect,
                InfoKeyCategories.consumerNotificationPlusUpdateinfoSet);

            Assert.IsTrue(containsFirstIdCategory && containsSecondIdCategory,
                "Ensure that profile editor limits for create or update profile consumer are configured properly in the current environment");
        }

        private bool ContainsCategory(List<InfoBase> infos, string categoryKey)
        {
            return infos.OfType<CategoryInfo>()
                .Any(categoryInfo => categoryInfo.InfoKey.ToLower() == categoryKey.ToLower() ||
                                     ContainsCategory(categoryInfo.Infos, categoryKey));
        }
    }
}
