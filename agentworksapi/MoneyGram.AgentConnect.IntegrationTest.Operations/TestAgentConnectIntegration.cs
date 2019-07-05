using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations
{
    public class TestAgentConnectIntegration : ITestAgentConnectIntegration
    {
        private TestRunner _testRunner;
        private IAgentConnectConfig _agentConnectConfig;
        private Agents _agents;

        public TestAgentConnectIntegration(TestRunner testRunner)
        {
            _testRunner = testRunner;
            _agentConnectConfig = new AgentConnectConfigCoded();
            _agents = new Agents(testRunner.IsTrainingMode);
        }

        #region LOOKUP
        public GetCountryInfoResponse GetCountryInfo(string location, GetCountryInfoRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetCountryInfo(agent, req);
        }
        public GetCountrySubdivisionResponse GetCountrySubdivision(string location, GetCountrySubdivisionRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetCountrySubdivision(agent, req);
        }

        public GetCurrencyInfoResponse GetCurrencyInfo(string location, GetCurrencyInfoRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetCurrencyInfo(agent, req);
        }

        public IndustryResponse Industry(string location, IndustryRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.Industry(agent, req);
        }

        public GetEnumerationsResponse GetEnumerations(string location, GetEnumerationsRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetEnumerations(agent, req);
        }

        public GetBankDetailsResponse GetBankDetails(string location, GetBankDetailsRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetBankDetails(agent, req);
        }
        public GetBankDetailsByLevelResponse GetBankDetailsByLevel(string location, GetBankDetailsByLevelRequest req)
        {
            var agent = _agents.GetAgent(location);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.GetBankDetailsByLevel(agent, req);
        }

        public ProfileResponse Profile(ProfileRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            return _testRunner.AgentConnect.Profile(agent, req);
        }
        #endregion

        #region BillPay

        public BillerSearchResponse BillerSearch(string agentId, string agentPos, BillerSearchRequest req)
        {
            var agent = _agents.GetAgent(agentId, agentPos);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.BillerSearch(agent, req);
            return response;
        }

        public BPValidationResponse BPValidation(BPValidationRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.BpValidation(agent, req);
            return response;
        }

        #endregion

        #region CONSUMER

        /// <summary>
        /// ConsumerLookup AC call
        /// </summary>
        /// <param name="location">Physical location of the agent - MN, NY, CAN, MEX etc</param>
        /// <param name="existing">Whether the customer query will return existing customers or customer not found</param>
        /// <returns>Domain model hydrated from the AC response of ConsumerLookup</returns>
        public ConsumerHistoryLookupResponse ConsumerHistoryLookup(string agentId, string agentPos, bool existing)
        {
            var agent = _agents.GetAgent(agentId, agentPos);
            var req = existing ? ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestExisting : ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestNonExisting;
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.ConsumerHistoryLookup(agent, req);
            return response;
        }

        public ConsumerHistoryLookupResponse ConsumerHistoryLookup(ConsumerHistoryLookupRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.ConsumerHistoryLookup(agent, req);
            return response;
        }

        public CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerData createOrUpdateProfileData)
        {            
            var agent = _agents.GetAgent(createOrUpdateProfileData.Request.AgentId, createOrUpdateProfileData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(createOrUpdateProfileData.CreateOrUpdateProfileConsumerRequest);
            return _testRunner.AgentConnect.CreateOrUpdateProfileConsumer(agent,
                createOrUpdateProfileData.CreateOrUpdateProfileConsumerRequest);
        }

        public GetProfileConsumerResponse GetProfileConsumer(GetProfileConsumerData getProfileConsumerData)
        {
            var agent = _agents.GetAgent(getProfileConsumerData.Request.AgentId, getProfileConsumerData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(getProfileConsumerData.GetProfileConsumerRequest);
            getProfileConsumerData.GetProfileConsumerRequest.ConsumerProfileID =
                getProfileConsumerData.Request.ConsumerProfileID;
            getProfileConsumerData.GetProfileConsumerRequest.ConsumerProfileIDType =
                getProfileConsumerData.Request.ConsumerProfileIDType;
            return _testRunner.AgentConnect.GetProfileConsumer(agent, getProfileConsumerData.GetProfileConsumerRequest);
        }

        public CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderData createOrUpdateProfileData)
        {
            var agent = _agents.GetAgent(createOrUpdateProfileData.Request.AgentId, createOrUpdateProfileData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(createOrUpdateProfileData.CreateOrUpdateProfileSenderRequest);
            return _testRunner.AgentConnect.CreateOrUpdateProfileSender(agent,
                createOrUpdateProfileData.CreateOrUpdateProfileSenderRequest);
        }

        public GetProfileSenderResponse GetProfileSender(GetProfileSenderData getProfileSenderData)
        {
            var agent = _agents.GetAgent(getProfileSenderData.Request.AgentId, getProfileSenderData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(getProfileSenderData.GetProfileSenderRequest);
            getProfileSenderData.GetProfileSenderRequest.ConsumerProfileID =
                getProfileSenderData.Request.SenderProfileID;
            getProfileSenderData.GetProfileSenderRequest.ConsumerProfileIDType =
                getProfileSenderData.Request.SenderProfileIDType;
            return _testRunner.AgentConnect.GetProfileSender(agent, getProfileSenderData.GetProfileSenderRequest);
        }

        public CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverData createOrUpdateProfileData)
        {
            var agent = _agents.GetAgent(createOrUpdateProfileData.Request.AgentId, createOrUpdateProfileData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(createOrUpdateProfileData.CreateOrUpdateProfileReceiverRequest);
            var response = _testRunner.AgentConnect.CreateOrUpdateProfileReceiver(agent,
                createOrUpdateProfileData.CreateOrUpdateProfileReceiverRequest);
            return response;
        }

        public GetProfileReceiverResponse GetProfileReceiver(GetProfileReceiverData getProfileReceiverData)
        {
            var agent = _agents.GetAgent(getProfileReceiverData.Request.AgentId, getProfileReceiverData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(getProfileReceiverData.GetProfileReceiverRequest);
            getProfileReceiverData.GetProfileReceiverRequest.ConsumerProfileID =
                getProfileReceiverData.Request.ReceiverProfileID;
            getProfileReceiverData.GetProfileReceiverRequest.ConsumerProfileIDType =
                getProfileReceiverData.Request.ReceiverProfileIDType;
            return _testRunner.AgentConnect.GetProfileReceiver(agent, getProfileReceiverData.GetProfileReceiverRequest);
        }

        public SearchConsumerProfilesResponse SearchConsumerProfiles(
            SearchConsumerProfilesData searchConsumerProfilesData)
        {
            var agent = _agents.GetAgent(searchConsumerProfilesData.Request.AgentId,
                searchConsumerProfilesData.Request.AgentPos);
            _agentConnectConfig.DecorateRequest(searchConsumerProfilesData.SearchConsumerProfilesRequest);
            return _testRunner.AgentConnect.SearchConsumerProfiles(agent,
                searchConsumerProfilesData.SearchConsumerProfilesRequest);
        }

        #endregion

        #region SEND

        public FeeLookupResponse FeeLookup(FeeLookupRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.FeeLookup(agent, req);
            return response;
        }

        public SendValidationResponse SendValidation(SendData sendData)
        {
            var agentData = sendData.SendRequest;
            var validationReq = sendData.SendValidationReqs.LastOrDefault();
            var agent = _agents.GetAgent(agentData.AgentId, agentData.AgentPos);
            _agentConnectConfig.DecorateRequest(validationReq);
            var response = _testRunner.AgentConnect.SendValidation(agent, validationReq);
            return response;
        }

        #endregion

        #region Amend

        public AmendValidationResponse AmendValidation(AmendData amendData)
        {
            var agentData = amendData.AmendOperationRequest;
            var validationReq = amendData.ValidationRequests.LastOrDefault();
            var agent = _agents.GetAgent(agentData.AgentId, agentData.AgentPos);
            _agentConnectConfig.DecorateRequest(validationReq);
            var response = _testRunner.AgentConnect.AmendValidation(agent, validationReq);
            return response;
        }

        #endregion

        #region Send Reversal

        public SendReversalValidationResponse SendReversalValidation(SendReversalData sendReversalData)
        {
            var agentData = sendReversalData.SendReversalRequest;
            var validationReq = sendReversalData.ValidationRequests.LastOrDefault();
            var agent = _agents.GetAgent(agentData.AgentId, agentData.AgentPos);
            _agentConnectConfig.DecorateRequest(validationReq);
            var response = _testRunner.AgentConnect.SendReversalValidation(agent, validationReq);
            return response;
        }

        #endregion

        #region Receive Reversal

        public ReceiveReversalValidationResponse ReceiveReversalValidation(ReceiveReversalData receiveReversalData)
        {
            var agent = _agents.GetAgent(receiveReversalData.ReceiveReversalOperationRequest.AgentId, receiveReversalData.ReceiveReversalOperationRequest.AgentPos);
            var validationRequest = receiveReversalData.ValidationRequests.LastOrDefault();
            _agentConnectConfig.DecorateRequest(validationRequest);
            var response = _testRunner.AgentConnect.ReceiveReversalValidation(agent, validationRequest);
            return response;
        }

        #endregion

        #region COMMON

        public CompleteSessionResponse CompleteSession(CompleteSessionRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.CompleteSession(agent, req);
            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Physical location of the agent - MN, NY, CAN, MEX etc</param>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetAllFieldsResponse GetAllFields(GetAllFieldsRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.GetAllFields(agent, req);
            return response;
        }

        #endregion

        #region RECV

        public TransactionLookupResponse TransactionLookup(TransactionLookupRequest req)
        {
            var agent = _agents.GetAgent(req.AgentID, req.AgentSequence);
            _agentConnectConfig.DecorateRequest(req);
            var response = _testRunner.AgentConnect.TransactionLookup(agent, req);
            return response;
        }
        public ReceiveValidationResponse ReceiveValidation(ReceiveData recvData)
        {
            var agent = _agents.GetAgent(recvData.ReceiveRequest.AgentId, recvData.ReceiveRequest.AgentPos);
            var validationRequest = recvData.ReceiveValidationRequests.Last();
            _agentConnectConfig.DecorateRequest(validationRequest);
            var response = _testRunner.AgentConnect.ReceiveValidation(agent, validationRequest);
            return response;
        }

        #endregion
    }
}