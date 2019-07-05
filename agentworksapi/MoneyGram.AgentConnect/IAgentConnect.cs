//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Threading.Tasks;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect
{
    public interface IAgentConnect 
    {
		AmendValidationResponse AmendValidation(Agent agent, AmendValidationRequest amendValidationRequest);
		Task<AmendValidationResponse> AmendValidationAsync(Agent agent, AmendValidationRequest amendValidationRequest);
		BillerSearchResponse BillerSearch(Agent agent, BillerSearchRequest billerSearchRequest);
		Task<BillerSearchResponse> BillerSearchAsync(Agent agent, BillerSearchRequest billerSearchRequest);
		BillPaymentDetailReportResponse BillPaymentDetailReport(Agent agent, BillPaymentDetailReportRequest billPaymentDetailReportRequest);
		Task<BillPaymentDetailReportResponse> BillPaymentDetailReportAsync(Agent agent, BillPaymentDetailReportRequest billPaymentDetailReportRequest);
		BillPaymentSummaryReportResponse BillPaymentSummaryReport(Agent agent, BillPaymentSummaryReportRequest billPaymentSummaryReportRequest);
		Task<BillPaymentSummaryReportResponse> BillPaymentSummaryReportAsync(Agent agent, BillPaymentSummaryReportRequest billPaymentSummaryReportRequest);
		BPValidationResponse BpValidation(Agent agent, BPValidationRequest bPValidationRequest);
		Task<BPValidationResponse> BpValidationAsync(Agent agent, BPValidationRequest bPValidationRequest);
		CheckInResponse CheckIn(Agent agent, CheckInRequest checkInRequest);
		Task<CheckInResponse> CheckInAsync(Agent agent, CheckInRequest checkInRequest);
		CityListResponse CityList(Agent agent, CityListRequest cityListRequest);
		Task<CityListResponse> CityListAsync(Agent agent, CityListRequest cityListRequest);
		CompleteSessionResponse CompleteSession(Agent agent, CompleteSessionRequest completeSessionRequest);
		Task<CompleteSessionResponse> CompleteSessionAsync(Agent agent, CompleteSessionRequest completeSessionRequest);
		ComplianceTransactionResponse ComplianceTransaction(Agent agent, ComplianceTransactionRequest complianceTransactionRequest);
		Task<ComplianceTransactionResponse> ComplianceTransactionAsync(Agent agent, ComplianceTransactionRequest complianceTransactionRequest);
		ConfirmTokenResponse ConfirmToken(Agent agent, ConfirmTokenRequest confirmTokenRequest);
		Task<ConfirmTokenResponse> ConfirmTokenAsync(Agent agent, ConfirmTokenRequest confirmTokenRequest);
		ConsumerHistoryLookupResponse ConsumerHistoryLookup(Agent agent, ConsumerHistoryLookupRequest consumerHistoryLookupRequest);
		Task<ConsumerHistoryLookupResponse> ConsumerHistoryLookupAsync(Agent agent, ConsumerHistoryLookupRequest consumerHistoryLookupRequest);
		CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumer(Agent agent, CreateOrUpdateProfileConsumerRequest createOrUpdateProfileConsumerRequest);
		Task<CreateOrUpdateProfileConsumerResponse> CreateOrUpdateProfileConsumerAsync(Agent agent, CreateOrUpdateProfileConsumerRequest createOrUpdateProfileConsumerRequest);
		CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(Agent agent, CreateOrUpdateProfileReceiverRequest createOrUpdateProfileReceiverRequest);
		Task<CreateOrUpdateProfileReceiverResponse> CreateOrUpdateProfileReceiverAsync(Agent agent, CreateOrUpdateProfileReceiverRequest createOrUpdateProfileReceiverRequest);
		CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(Agent agent, CreateOrUpdateProfileSenderRequest createOrUpdateProfileSenderRequest);
		Task<CreateOrUpdateProfileSenderResponse> CreateOrUpdateProfileSenderAsync(Agent agent, CreateOrUpdateProfileSenderRequest createOrUpdateProfileSenderRequest);
		DepositAnnouncementResponse DepositAnnouncement(Agent agent, DepositAnnouncementRequest depositAnnouncementRequest);
		Task<DepositAnnouncementResponse> DepositAnnouncementAsync(Agent agent, DepositAnnouncementRequest depositAnnouncementRequest);
		DirectoryOfAgentsByAreaCodePrefixResponse DirectoryOfAgentsByAreaCodePrefix(Agent agent, DirectoryOfAgentsByAreaCodePrefixRequest directoryOfAgentsByAreaCodePrefixRequest);
		Task<DirectoryOfAgentsByAreaCodePrefixResponse> DirectoryOfAgentsByAreaCodePrefixAsync(Agent agent, DirectoryOfAgentsByAreaCodePrefixRequest directoryOfAgentsByAreaCodePrefixRequest);
		DirectoryOfAgentsByCityResponse DirectoryOfAgentsByCity(Agent agent, DirectoryOfAgentsByCityRequest directoryOfAgentsByCityRequest);
		Task<DirectoryOfAgentsByCityResponse> DirectoryOfAgentsByCityAsync(Agent agent, DirectoryOfAgentsByCityRequest directoryOfAgentsByCityRequest);
		DirectoryOfAgentsByZipResponse DirectoryOfAgentsByZip(Agent agent, DirectoryOfAgentsByZipRequest directoryOfAgentsByZipRequest);
		Task<DirectoryOfAgentsByZipResponse> DirectoryOfAgentsByZipAsync(Agent agent, DirectoryOfAgentsByZipRequest directoryOfAgentsByZipRequest);
		DisclosureTextDetailsResponse DisclosureTextDetails(Agent agent, DisclosureTextDetailsRequest disclosureTextDetailsRequest);
		Task<DisclosureTextDetailsResponse> DisclosureTextDetailsAsync(Agent agent, DisclosureTextDetailsRequest disclosureTextDetailsRequest);
		DoddFrankStateRegulatorInfoResponse DoddFrankStateRegulatorInfo(Agent agent, DoddFrankStateRegulatorInfoRequest doddFrankStateRegulatorInfoRequest);
		Task<DoddFrankStateRegulatorInfoResponse> DoddFrankStateRegulatorInfoAsync(Agent agent, DoddFrankStateRegulatorInfoRequest doddFrankStateRegulatorInfoRequest);
		DwInitialSetupResponse DwInitialSetup(Agent agent, DwInitialSetupRequest dwInitialSetupRequest);
		Task<DwInitialSetupResponse> DwInitialSetupAsync(Agent agent, DwInitialSetupRequest dwInitialSetupRequest);
		DwPasswordResponse DwPassword(Agent agent, DwPasswordRequest dwPasswordRequest);
		Task<DwPasswordResponse> DwPasswordAsync(Agent agent, DwPasswordRequest dwPasswordRequest);
		DwProfileResponse DwProfile(Agent agent, DwProfileRequest dwProfileRequest);
		Task<DwProfileResponse> DwProfileAsync(Agent agent, DwProfileRequest dwProfileRequest);
		FeeLookupResponse FeeLookup(Agent agent, FeeLookupRequest feeLookupRequest);
		Task<FeeLookupResponse> FeeLookupAsync(Agent agent, FeeLookupRequest feeLookupRequest);
		FeeLookupBySendCountryResponse FeeLookupBySendCountry(Agent agent, FeeLookupBySendCountryRequest feeLookupBySendCountryRequest);
		Task<FeeLookupBySendCountryResponse> FeeLookupBySendCountryAsync(Agent agent, FeeLookupBySendCountryRequest feeLookupBySendCountryRequest);
		GetAllErrorsResponse GetAllErrors(Agent agent, GetAllErrorsRequest getAllErrorsRequest);
		Task<GetAllErrorsResponse> GetAllErrorsAsync(Agent agent, GetAllErrorsRequest getAllErrorsRequest);
		GetAllFieldsResponse GetAllFields(Agent agent, GetAllFieldsRequest getAllFieldsRequest);
		Task<GetAllFieldsResponse> GetAllFieldsAsync(Agent agent, GetAllFieldsRequest getAllFieldsRequest);
		GetBankDetailsResponse GetBankDetails(Agent agent, GetBankDetailsRequest getBankDetailsRequest);
		Task<GetBankDetailsResponse> GetBankDetailsAsync(Agent agent, GetBankDetailsRequest getBankDetailsRequest);
		GetBankDetailsByLevelResponse GetBankDetailsByLevel(Agent agent, GetBankDetailsByLevelRequest getBankDetailsByLevelRequest);
		Task<GetBankDetailsByLevelResponse> GetBankDetailsByLevelAsync(Agent agent, GetBankDetailsByLevelRequest getBankDetailsByLevelRequest);
		GetBroadcastMessagesResponse GetBroadcastMessages(Agent agent, GetBroadcastMessagesRequest getBroadcastMessagesRequest);
		Task<GetBroadcastMessagesResponse> GetBroadcastMessagesAsync(Agent agent, GetBroadcastMessagesRequest getBroadcastMessagesRequest);
		GetConsumerProfileDocumentResponse GetConsumerProfileDocument(Agent agent, GetConsumerProfileDocumentRequest getConsumerProfileDocumentRequest);
		Task<GetConsumerProfileDocumentResponse> GetConsumerProfileDocumentAsync(Agent agent, GetConsumerProfileDocumentRequest getConsumerProfileDocumentRequest);
		GetConsumerProfileTransactionHistoryResponse GetConsumerProfileTransactionHistory(Agent agent, GetConsumerProfileTransactionHistoryRequest getConsumerProfileTransactionHistoryRequest);
		Task<GetConsumerProfileTransactionHistoryResponse> GetConsumerProfileTransactionHistoryAsync(Agent agent, GetConsumerProfileTransactionHistoryRequest getConsumerProfileTransactionHistoryRequest);
		GetCountryInfoResponse GetCountryInfo(Agent agent, GetCountryInfoRequest getCountryInfoRequest);
		Task<GetCountryInfoResponse> GetCountryInfoAsync(Agent agent, GetCountryInfoRequest getCountryInfoRequest);
		GetCountrySubdivisionResponse GetCountrySubdivision(Agent agent, GetCountrySubdivisionRequest getCountrySubdivisionRequest);
		Task<GetCountrySubdivisionResponse> GetCountrySubdivisionAsync(Agent agent, GetCountrySubdivisionRequest getCountrySubdivisionRequest);
		GetCurrencyInfoResponse GetCurrencyInfo(Agent agent, GetCurrencyInfoRequest getCurrencyInfoRequest);
		Task<GetCurrencyInfoResponse> GetCurrencyInfoAsync(Agent agent, GetCurrencyInfoRequest getCurrencyInfoRequest);
		GetDebugDataResponse GetDebugData(Agent agent, GetDebugDataRequest getDebugDataRequest);
		Task<GetDebugDataResponse> GetDebugDataAsync(Agent agent, GetDebugDataRequest getDebugDataRequest);
		GetDepositBankListResponse GetDepositBankList(Agent agent, GetDepositBankListRequest getDepositBankListRequest);
		Task<GetDepositBankListResponse> GetDepositBankListAsync(Agent agent, GetDepositBankListRequest getDepositBankListRequest);
		GetDepositInformationResponse GetDepositInformation(Agent agent, GetDepositInformationRequest getDepositInformationRequest);
		Task<GetDepositInformationResponse> GetDepositInformationAsync(Agent agent, GetDepositInformationRequest getDepositInformationRequest);
		GetEnumerationsResponse GetEnumerations(Agent agent, GetEnumerationsRequest getEnumerationsRequest);
		Task<GetEnumerationsResponse> GetEnumerationsAsync(Agent agent, GetEnumerationsRequest getEnumerationsRequest);
		GetPersonalIDImageResponse GetPersonalIDImage(Agent agent, GetPersonalIDImageRequest getPersonalIDImageRequest);
		Task<GetPersonalIDImageResponse> GetPersonalIDImageAsync(Agent agent, GetPersonalIDImageRequest getPersonalIDImageRequest);
		GetProfileConsumerResponse GetProfileConsumer(Agent agent, GetProfileConsumerRequest getProfileConsumerRequest);
		Task<GetProfileConsumerResponse> GetProfileConsumerAsync(Agent agent, GetProfileConsumerRequest getProfileConsumerRequest);
		GetProfileReceiverResponse GetProfileReceiver(Agent agent, GetProfileReceiverRequest getProfileReceiverRequest);
		Task<GetProfileReceiverResponse> GetProfileReceiverAsync(Agent agent, GetProfileReceiverRequest getProfileReceiverRequest);
		GetProfileSenderResponse GetProfileSender(Agent agent, GetProfileSenderRequest getProfileSenderRequest);
		Task<GetProfileSenderResponse> GetProfileSenderAsync(Agent agent, GetProfileSenderRequest getProfileSenderRequest);
		GetReceiptForReprintResponse GetReceiptForReprint(Agent agent, GetReceiptForReprintRequest getReceiptForReprintRequest);
		Task<GetReceiptForReprintResponse> GetReceiptForReprintAsync(Agent agent, GetReceiptForReprintRequest getReceiptForReprintRequest);
		GetServiceOptionsResponse GetServiceOptions(Agent agent, GetServiceOptionsRequest getServiceOptionsRequest);
		Task<GetServiceOptionsResponse> GetServiceOptionsAsync(Agent agent, GetServiceOptionsRequest getServiceOptionsRequest);
		GetUCPByConsumerAttributesResponse GetUCPByConsumerAttributes(Agent agent, GetUCPByConsumerAttributesRequest getUCPByConsumerAttributesRequest);
		Task<GetUCPByConsumerAttributesResponse> GetUCPByConsumerAttributesAsync(Agent agent, GetUCPByConsumerAttributesRequest getUCPByConsumerAttributesRequest);
		IndustryResponse Industry(Agent agent, IndustryRequest industryRequest);
		Task<IndustryResponse> IndustryAsync(Agent agent, IndustryRequest industryRequest);
		InitialSetupResponse InitialSetup(Agent agent, InitialSetupRequest initialSetupRequest);
		Task<InitialSetupResponse> InitialSetupAsync(Agent agent, InitialSetupRequest initialSetupRequest);
		InjectedInstructionResponse InjectedInstruction(Agent agent, InjectedInstructionRequest injectedInstructionRequest);
		Task<InjectedInstructionResponse> InjectedInstructionAsync(Agent agent, InjectedInstructionRequest injectedInstructionRequest);
		MoneyGramReceiveDetailReportResponse MoneyGramReceiveDetailReport(Agent agent, MoneyGramReceiveDetailReportRequest moneyGramReceiveDetailReportRequest);
		Task<MoneyGramReceiveDetailReportResponse> MoneyGramReceiveDetailReportAsync(Agent agent, MoneyGramReceiveDetailReportRequest moneyGramReceiveDetailReportRequest);
		MoneyGramReceiveSummaryReportResponse MoneyGramReceiveSummaryReport(Agent agent, MoneyGramReceiveSummaryReportRequest moneyGramReceiveSummaryReportRequest);
		Task<MoneyGramReceiveSummaryReportResponse> MoneyGramReceiveSummaryReportAsync(Agent agent, MoneyGramReceiveSummaryReportRequest moneyGramReceiveSummaryReportRequest);
		MoneyGramSendDetailReportResponse MoneyGramSendDetailReport(Agent agent, MoneyGramSendDetailReportRequest moneyGramSendDetailReportRequest);
		Task<MoneyGramSendDetailReportResponse> MoneyGramSendDetailReportAsync(Agent agent, MoneyGramSendDetailReportRequest moneyGramSendDetailReportRequest);
		MoneyGramSendDetailReportWithTaxResponse MoneyGramSendDetailReportWithTax(Agent agent, MoneyGramSendDetailReportWithTaxRequest moneyGramSendDetailReportWithTaxRequest);
		Task<MoneyGramSendDetailReportWithTaxResponse> MoneyGramSendDetailReportWithTaxAsync(Agent agent, MoneyGramSendDetailReportWithTaxRequest moneyGramSendDetailReportWithTaxRequest);
		MoneyGramSendSummaryReportResponse MoneyGramSendSummaryReport(Agent agent, MoneyGramSendSummaryReportRequest moneyGramSendSummaryReportRequest);
		Task<MoneyGramSendSummaryReportResponse> MoneyGramSendSummaryReportAsync(Agent agent, MoneyGramSendSummaryReportRequest moneyGramSendSummaryReportRequest);
		MoneyOrderTotalResponse MoneyOrderTotal(Agent agent, MoneyOrderTotalRequest moneyOrderTotalRequest);
		Task<MoneyOrderTotalResponse> MoneyOrderTotalAsync(Agent agent, MoneyOrderTotalRequest moneyOrderTotalRequest);
		OpenOTPLoginResponse OpenOTPLogin(Agent agent, OpenOTPLoginRequest openOTPLoginRequest);
		Task<OpenOTPLoginResponse> OpenOTPLoginAsync(Agent agent, OpenOTPLoginRequest openOTPLoginRequest);
		ProfileResponse Profile(Agent agent, ProfileRequest profileRequest);
		Task<ProfileResponse> ProfileAsync(Agent agent, ProfileRequest profileRequest);
		ProfileChangeResponse ProfileChange(Agent agent, ProfileChangeRequest profileChangeRequest);
		Task<ProfileChangeResponse> ProfileChangeAsync(Agent agent, ProfileChangeRequest profileChangeRequest);
		PromotionLookupByCodeResponse PromotionLookupByCode(Agent agent, PromotionLookupByCodeRequest promotionLookupByCodeRequest);
		Task<PromotionLookupByCodeResponse> PromotionLookupByCodeAsync(Agent agent, PromotionLookupByCodeRequest promotionLookupByCodeRequest);
		ReceiptsFormatDetailsResponse ReceiptsFormatDetails(Agent agent, ReceiptsFormatDetailsRequest receiptsFormatDetailsRequest);
		Task<ReceiptsFormatDetailsResponse> ReceiptsFormatDetailsAsync(Agent agent, ReceiptsFormatDetailsRequest receiptsFormatDetailsRequest);
		ReceiveReversalValidationResponse ReceiveReversalValidation(Agent agent, ReceiveReversalValidationRequest receiveReversalValidationRequest);
		Task<ReceiveReversalValidationResponse> ReceiveReversalValidationAsync(Agent agent, ReceiveReversalValidationRequest receiveReversalValidationRequest);
		ReceiveValidationResponse ReceiveValidation(Agent agent, ReceiveValidationRequest receiveValidationRequest);
		Task<ReceiveValidationResponse> ReceiveValidationAsync(Agent agent, ReceiveValidationRequest receiveValidationRequest);
		RegisterHardTokenResponse RegisterHardToken(Agent agent, RegisterHardTokenRequest registerHardTokenRequest);
		Task<RegisterHardTokenResponse> RegisterHardTokenAsync(Agent agent, RegisterHardTokenRequest registerHardTokenRequest);
		SaveConsumerProfileDocumentResponse SaveConsumerProfileDocument(Agent agent, SaveConsumerProfileDocumentRequest saveConsumerProfileDocumentRequest);
		Task<SaveConsumerProfileDocumentResponse> SaveConsumerProfileDocumentAsync(Agent agent, SaveConsumerProfileDocumentRequest saveConsumerProfileDocumentRequest);
		SaveConsumerProfileImageResponse SaveConsumerProfileImage(Agent agent, SaveConsumerProfileImageRequest saveConsumerProfileImageRequest);
		Task<SaveConsumerProfileImageResponse> SaveConsumerProfileImageAsync(Agent agent, SaveConsumerProfileImageRequest saveConsumerProfileImageRequest);
		SaveDebugDataResponse SaveDebugData(Agent agent, SaveDebugDataRequest saveDebugDataRequest);
		Task<SaveDebugDataResponse> SaveDebugDataAsync(Agent agent, SaveDebugDataRequest saveDebugDataRequest);
		SavePersonalIDImageResponse SavePersonalIDImage(Agent agent, SavePersonalIDImageRequest savePersonalIDImageRequest);
		Task<SavePersonalIDImageResponse> SavePersonalIDImageAsync(Agent agent, SavePersonalIDImageRequest savePersonalIDImageRequest);
		SaveProfileResponse SaveProfile(Agent agent, SaveProfileRequest saveProfileRequest);
		Task<SaveProfileResponse> SaveProfileAsync(Agent agent, SaveProfileRequest saveProfileRequest);
		SaveSubagentsResponse SaveSubagents(Agent agent, SaveSubagentsRequest saveSubagentsRequest);
		Task<SaveSubagentsResponse> SaveSubagentsAsync(Agent agent, SaveSubagentsRequest saveSubagentsRequest);
		SaveTransactionDocumentResponse SaveTransactionDocument(Agent agent, SaveTransactionDocumentRequest saveTransactionDocumentRequest);
		Task<SaveTransactionDocumentResponse> SaveTransactionDocumentAsync(Agent agent, SaveTransactionDocumentRequest saveTransactionDocumentRequest);
		SearchConsumerProfilesResponse SearchConsumerProfiles(Agent agent, SearchConsumerProfilesRequest searchConsumerProfilesRequest);
		Task<SearchConsumerProfilesResponse> SearchConsumerProfilesAsync(Agent agent, SearchConsumerProfilesRequest searchConsumerProfilesRequest);
		SearchStagedTransactionsResponse SearchStagedTransactions(Agent agent, SearchStagedTransactionsRequest searchStagedTransactionsRequest);
		Task<SearchStagedTransactionsResponse> SearchStagedTransactionsAsync(Agent agent, SearchStagedTransactionsRequest searchStagedTransactionsRequest);
		SendReversalValidationResponse SendReversalValidation(Agent agent, SendReversalValidationRequest sendReversalValidationRequest);
		Task<SendReversalValidationResponse> SendReversalValidationAsync(Agent agent, SendReversalValidationRequest sendReversalValidationRequest);
		SendValidationResponse SendValidation(Agent agent, SendValidationRequest sendValidationRequest);
		Task<SendValidationResponse> SendValidationAsync(Agent agent, SendValidationRequest sendValidationRequest);
		SubagentsResponse Subagents(Agent agent, SubagentsRequest subagentsRequest);
		Task<SubagentsResponse> SubagentsAsync(Agent agent, SubagentsRequest subagentsRequest);
		TransactionLookupResponse TransactionLookup(Agent agent, TransactionLookupRequest transactionLookupRequest);
		Task<TransactionLookupResponse> TransactionLookupAsync(Agent agent, TransactionLookupRequest transactionLookupRequest);
		TranslationsResponse Translations(Agent agent, TranslationsRequest translationsRequest);
		Task<TranslationsResponse> TranslationsAsync(Agent agent, TranslationsRequest translationsRequest);
		VariableReceiptInfoResponse VariableReceiptInfo(Agent agent, VariableReceiptInfoRequest variableReceiptInfoRequest);
		Task<VariableReceiptInfoResponse> VariableReceiptInfoAsync(Agent agent, VariableReceiptInfoRequest variableReceiptInfoRequest);
		VersionManifestResponse VersionManifest(Agent agent, VersionManifestRequest versionManifestRequest);
		Task<VersionManifestResponse> VersionManifestAsync(Agent agent, VersionManifestRequest versionManifestRequest);
	}
}