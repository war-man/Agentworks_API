using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations
{
    public interface ITestAgentConnectIntegration
    {
        GetEnumerationsResponse GetEnumerations(string location,GetEnumerationsRequest req);
        GetCountryInfoResponse GetCountryInfo(string location, GetCountryInfoRequest req);
        GetCountrySubdivisionResponse GetCountrySubdivision(string location, GetCountrySubdivisionRequest req);
        CompleteSessionResponse CompleteSession(CompleteSessionRequest req);
        ConsumerHistoryLookupResponse ConsumerHistoryLookup(ConsumerHistoryLookupRequest req);
        ConsumerHistoryLookupResponse ConsumerHistoryLookup(string agentId, string agentPos, bool existing);
        FeeLookupResponse FeeLookup(FeeLookupRequest req);
        GetAllFieldsResponse GetAllFields(GetAllFieldsRequest req);
        SendValidationResponse SendValidation(SendData sendData);
        SendReversalValidationResponse SendReversalValidation(SendReversalData sendReversalData);
        AmendValidationResponse AmendValidation(AmendData amendData);
        TransactionLookupResponse TransactionLookup(TransactionLookupRequest req);
        ReceiveReversalValidationResponse ReceiveReversalValidation(ReceiveReversalData receiveReversalData);

        ReceiveValidationResponse ReceiveValidation(ReceiveData recvData);
        #region BillPay
        BillerSearchResponse BillerSearch(string agentId, string agentPos, BillerSearchRequest req);
        BPValidationResponse BPValidation(BPValidationRequest req);

        #endregion

        CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerData createOrUpdateProfileData);
        GetProfileConsumerResponse GetProfileConsumer(GetProfileConsumerData getProfileConsumerData);
        CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderData createOrUpdateProfileData);
        GetProfileSenderResponse GetProfileSender(GetProfileSenderData getProfileSenderData);
        CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverData createOrUpdateProfileData);
        GetProfileReceiverResponse GetProfileReceiver(GetProfileReceiverData getProfileReceiverData);

        SearchConsumerProfilesResponse SearchConsumerProfiles(
            SearchConsumerProfilesData searchConsumerProfilesData);
    }
}