using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Consumer;
using INTEGRATION = MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Consumer
{
    [TestClass]
    public class SearchConsumerProfilesTest
    {
        private ConsumerOperations _consumerOperations { get;set; }
        private string _agentState { get; set; }

        private List<string> _searchCriteriaList;

        [TestInitialize]
        public void TestSetup()
        {
            _consumerOperations = new ConsumerOperations(new TestRunner());
            _agentState = AgentLocation.MN;
            _searchCriteriaList = new List<string>()
            {
                SearchProfileCriteria.NAME_AC_NUM_SEARCH,
                SearchProfileCriteria.NAME_DOB_SEARCH,
                SearchProfileCriteria.NAME_PHONE_SEARCH,
                SearchProfileCriteria.PLUS_NUM_SEARCH,
                SearchProfileCriteria.REF_NUM_DOB_SEARCH
            };
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

        [TestMethod]
        public void SearchConsumerProfile_ValidateShouldReturnFieldsToCollect()
        {
            var searchRequest = new SearchConsumerProfileRequest()
            {
                AgentState = _agentState
            };
            var exceptionList = new List<string>();
            foreach (var searchCriteria in _searchCriteriaList)
            {
                searchRequest.PopulateAgentData(searchRequest.AgentState);
                var searchData = new SearchConsumerProfilesData(searchRequest);
                try
                {
                    var searchResponse =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, null);
                    Assert.IsFalse(searchResponse.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponse.Errors?.Log()}");
                    Assert.IsTrue(searchResponse.Payload.FieldsToCollect != null &&
                                  searchResponse.Payload.FieldsToCollect.Count != 0);
                    Assert.IsTrue(searchResponse.Payload.ConsumerProfileSearchInfos?.Count == 0,
                        $"There are search results for criteria {searchCriteria.ToString()}");
                }
                catch (Exception ex)
                {
                    exceptionList.Add(
                        $"Failed search for criteria {searchCriteria.ToString()}. Exception: {ex.Message}");
                }
            }
            Assert.IsTrue(exceptionList.Count == 0, string.Join("; ", exceptionList.ToArray()));
        }

        [TestMethod]
        public void SearchConsumerProfile_IncorectDataShouldReturnEmptyList()
        {
            var searchRequest = new SearchConsumerProfileRequest()
            {
                AgentState = _agentState
            };
            var exceptionList = new List<string>();
            foreach (var searchCriteria in _searchCriteriaList)
            {
                searchRequest.PopulateAgentData(searchRequest.AgentState);
                var searchData = new SearchConsumerProfilesData(searchRequest);
                try
                {
                    var searchResponse =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, null);                    
                    Assert.IsFalse(searchResponse.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponse.Errors?.Log()}");
                    Assert.IsTrue(searchResponse.Payload.FieldsToCollect != null &&
                                  searchResponse.Payload.FieldsToCollect.Count != 0);
                    Assert.IsTrue(searchResponse.Payload.ConsumerProfileSearchInfos?.Count == 0,
                        $"There are search results for criteria {searchCriteria.ToString()}");


                    var searchValues =
                        _consumerOperations.GetIncorectSearchValues(searchResponse.Payload.FieldsToCollect);
                    var searchResponseWithEmptyResults =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, searchValues);
                    Assert.IsFalse(searchResponseWithEmptyResults.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponseWithEmptyResults.Errors?.Log()}");
                    Assert.IsTrue(searchResponseWithEmptyResults.Payload.FieldsToCollect != null &&
                                  searchResponseWithEmptyResults.Payload.FieldsToCollect.Count == 0);
                    Assert.IsTrue(searchResponseWithEmptyResults.Payload.ConsumerProfileSearchInfos?.Count == 0,
                        $"There are search results for criteria {searchCriteria.ToString()}");
                }
                catch (Exception ex)
                {
                    exceptionList.Add(
                        $"Failed search for criteria {searchCriteria.ToString()}. Exception: {ex.Message}");
                }
            }
            Assert.IsTrue(exceptionList.Count == 0, string.Join("; ", exceptionList.ToArray()));
        }

        [TestMethod]
        public void SearchExistingConsumerProfiles()
        {
            var createRequest = new INTEGRATION.CreateOrUpdateProfileConsumerRequest()
            {
                AgentState = _agentState
            };
            createRequest.PopulateAgentData(createRequest.AgentState);
            var createData = new CreateOrUpdateProfileConsumerData(createRequest);
            var createResponse = _consumerOperations.CreateProfileConsumer(createData);

            Assert.IsFalse(createResponse.Errors.Any(),
                $" {Environment.NewLine}Errors: {Environment.NewLine}{createResponse.Errors?.Log()}");
            
            var searchRequest = new INTEGRATION.SearchConsumerProfileRequest()
            {
                AgentState = _agentState
            };

            var exceptionList = new List<string>();
            foreach (var searchCriteria in _searchCriteriaList)
            {
                searchRequest.PopulateAgentData(searchRequest.AgentState);
                var searchData = new SearchConsumerProfilesData(searchRequest);
                var currentProfile = GetProfileConsumer(
                    createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileID,
                    createResponse.Payload.ConsumerProfileIDs.First().ConsumerProfileIDType);
                try
                {
                   var searchResponse =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, null);
                    Assert.IsFalse(searchResponse.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponse.Errors?.Log()}");
                    Assert.IsTrue(searchResponse.Payload.FieldsToCollect != null &&
                                  searchResponse.Payload.FieldsToCollect.Count != 0);
                    Assert.IsTrue(searchResponse.Payload.ConsumerProfileSearchInfos?.Count == 0,
                        $"There are not search results for criteria {searchCriteria.ToString()}");

                    var valuesToBeSearched =
                        _consumerOperations.GetCorectSearchValues(searchResponse.Payload.FieldsToCollect,
                            currentProfile.Payload.CurrentValues);
                    var searchResponseWithResult =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, valuesToBeSearched);
                    Assert.IsFalse(searchResponseWithResult.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponseWithResult.Errors?.Log()}");
                    Assert.IsTrue(searchResponseWithResult.Payload.FieldsToCollect != null &&
                                  searchResponseWithResult.Payload.FieldsToCollect.Count == 0, "There are fields to collect");
                    Assert.IsTrue(
                        searchResponseWithResult.Payload.ConsumerProfileSearchInfos != null &&
                        searchResponseWithResult.Payload.ConsumerProfileSearchInfos.Count != 0,
                        $"There are not search results for criteria {searchCriteria.ToString()}");
                    Assert.IsTrue(searchResponseWithResult.Payload.ConsumerProfileSearchInfos.FirstOrDefault(x =>
                                      x.ConsumerProfileID == createResponse.Payload.ConsumerProfileIDs.First()
                                          .ConsumerProfileID) != null, "Couldn't find profile in search results");
                }
                catch (Exception ex)
                {
                    exceptionList.Add(
                        $"Failed search for criteria {searchCriteria.ToString()}. Exception: {ex.Message}");
                }
            }

            Assert.IsTrue(exceptionList.Count == 0, string.Join("; ", exceptionList.ToArray()));
        }

        [TestMethod]
        public void SearchNotExistingConsumerProfile()
        {
            
            var searchRequest = new SearchConsumerProfileRequest()
            {
                AgentState = _agentState
            };

            var exceptionList = new List<string>();
            foreach (var searchCriteria in _searchCriteriaList)
            {
                searchRequest.PopulateAgentData(searchRequest.AgentState);
                var searchData = new SearchConsumerProfilesData(searchRequest);                
                try
                {
                    var searchResponse =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, null);
                    Assert.IsFalse(searchResponse.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponse.Errors?.Log()}");
                    Assert.IsTrue(searchResponse.Payload.FieldsToCollect != null &&
                                  searchResponse.Payload.FieldsToCollect.Count != 0);
                    Assert.IsTrue(searchResponse.Payload.ConsumerProfileSearchInfos?.Count == 0,
                        $"There are search results for criteria {searchCriteria.ToString()}");

                    var valuesToBeSearched =
                        _consumerOperations.GetNotExistingSearchValues(searchResponse.Payload.FieldsToCollect);
                    var searchResponseWithResult =
                        _consumerOperations.SearchConsumerProfiles(searchData, searchCriteria, valuesToBeSearched);
                    Assert.IsFalse(searchResponseWithResult.Errors.Any(),
                        $" {Environment.NewLine}Errors: {Environment.NewLine}{searchResponseWithResult.Errors?.Log()}");
                    Assert.IsTrue(searchResponseWithResult.Payload.FieldsToCollect != null &&
                                  searchResponseWithResult.Payload.FieldsToCollect.Count == 0);
                    Assert.IsTrue(
                        searchResponseWithResult.Payload.ConsumerProfileSearchInfos == null ||
                        searchResponseWithResult.Payload.ConsumerProfileSearchInfos.Count == 0,
                        $"There are search results for criteria {searchCriteria.ToString()}");                    
                }
                catch (Exception ex)
                {
                    exceptionList.Add(
                        $"Failed search for criteria {searchCriteria.ToString()}. Exception: {ex.Message}");
                }
            }

            Assert.IsTrue(exceptionList.Count == 0, string.Join("; ", exceptionList.ToArray()));
        }
    }
}
