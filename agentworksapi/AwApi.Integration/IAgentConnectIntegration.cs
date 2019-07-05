using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Models;

namespace AwApi.Integration
{
    public interface IAgentConnectIntegration
    {
        HealthCheckResponse HealthCheck();

        // COMMON calls
        CompleteSessionResponse CompleteSession(CompleteSessionRequest completeSessionReq);
        TransactionLookupResponse TransactionLookup(TransactionLookupRequest tranLookupRequest);
        SearchStagedTransactionsResponse SearchStagedTransactions(SearchStagedTransactionsRequest searchStagedTransactionsRequest);
        FeeLookupResponse FeeLookup(FeeLookupRequest feeLookupRequest);

        //RECV specific calls
        ConsumerHistoryLookupResponse ConsumerHistoryLookup(ConsumerHistoryLookupRequest consumerHistoryLookupReq);
        ReceiveValidationResponse ReceiveValidation(ReceiveValidationRequest receiveValidationReq);
        ReceiveReversalValidationResponse ReceiveReversalValidation(ReceiveReversalValidationRequest receiveReversalValidationRequest);

        //SEND specific calls
        SendValidationResponse SendValidation(SendValidationRequest sendValidationRequest);
        AmendValidationResponse AmendValidation(AmendValidationRequest amendValidationRequest);
        SendReversalValidationResponse SendReversalValidation(SendReversalValidationRequest sendReversalValidationRequest);
        GetBankDetailsByLevelResponse GetBankDetailsByLevel(GetBankDetailsByLevelRequest getBankDetailsByLevelRequest);
        GetBankDetailsResponse GetBankDetails(GetBankDetailsRequest getBankDetailsRequest);
        GetServiceOptionsResponse GetServiceOptions(GetServiceOptionsRequest getServiceOptionsRequest);

        // BILL PAY specific calls
        List<SenderLookupInfo> BillPaymentConsumerLookup(ConsumerHistoryLookupRequest consumerHistoryLookupRequest);
        BillerSearchResponse BillerSearch(BillerSearchRequest billerSearchRequest);
        BPValidationResponse BPValidation(BPValidationRequest bpValidationRequest);

        // Internal only calls
        DwInitialSetupResponse DwInitialSetup(DwInitialSetupRequest dwInitialSetupRequest);
        DwProfileResponse DwProfile(DwProfileRequest dwProfileRequest);

        // Deposit calls
        GetDepositInformationResponse GetDepositInformation(GetDepositInformationRequest getDepositInformationRequest);
        GetDepositBankListResponse GetDepositBankList(GetDepositBankListRequest getDepositBankListRequest);
        DepositAnnouncementResponse DepositAnnouncement(DepositAnnouncementRequest depositAnnouncementRequest);

        // Lookup calls
        GetCountryInfoResponse GetCountryInfo(GetCountryInfoRequest getCountryInfoRequest);
        GetCountrySubdivisionResponse GetCountrySubdivision(GetCountrySubdivisionRequest req);
        GetCurrencyInfoResponse GetCurrencyInfo(GetCurrencyInfoRequest getCurrencyInfoRequest);
        IndustryResponse Industry(IndustryRequest industryRequest);
        GetAllFieldsResponse GetAllFields(GetAllFieldsRequest req);
        GetEnumerationsResponse GetEnumerations(GetEnumerationsRequest req);

        // Profile
        ProfileResponse Profile(ProfileRequest profileRequest);
        SaveProfileResponse SaveProfile(SaveProfileRequest saveProfileRequest);

        // Image
        SavePersonalIDImageResponse SavePersonalIDImage(SavePersonalIDImageRequest savePersonalIDImageRequest);
        GetPersonalIDImageResponse GetPersonalIDImage(GetPersonalIDImageRequest getPersonalIDImageRequest);

        // Transaction Documents
        SaveTransactionDocumentResponse SaveTransactionDocument(SaveTransactionDocumentRequest saveTransactionDocumentRequest);

        // MoneyOrder
        ComplianceTransactionResponse ComplianceTransaction(ComplianceTransactionRequest request);
        MoneyOrderTotalResponse MoneyOrderTotal(MoneyOrderTotalRequest request);

        //Consumer
        GetProfileReceiverResponse GetProfileReceiver(GetProfileReceiverRequest getProfileReceiver);
        GetProfileSenderResponse GetProfileSender(GetProfileSenderRequest getProfileSender);
        CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverRequest createOrUpdateProfileReceiverRequest);
        CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderRequest createOrUpdateProfileSenderRequest);
        GetProfileConsumerResponse GetProfileConsumer(GetProfileConsumerRequest req);
        CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerRequest req);
        SaveConsumerProfileImageResponse SaveConsumerProfileImage(SaveConsumerProfileImageRequest req);
        SearchConsumerProfilesResponse SearchConsumerProfiles(SearchConsumerProfilesRequest req);
        GetConsumerProfileTransactionHistoryResponse GetConsumerProfileTransactionHistory(GetConsumerProfileTransactionHistoryRequest req);
        SaveConsumerProfileDocumentResponse SaveConsumerProfileDocument(SaveConsumerProfileDocumentRequest req);
        GetConsumerProfileDocumentResponse GetConsumerProfileDocument(GetConsumerProfileDocumentRequest req);
    }
}