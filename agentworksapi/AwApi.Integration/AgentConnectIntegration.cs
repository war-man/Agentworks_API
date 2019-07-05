using System.Collections.Generic;
using System.Linq;
using AwApi.Cache;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;
using MoneyGram.Common.Models;

namespace AwApi.Integration
{
    public class AgentConnectIntegration : IAgentConnectIntegration
    {
        public AgentConnectIntegration(IAgentConnect agentConnect, ICacheManager cacheManager)
        {
            agentConnect.ThrowIfNull(nameof(agentConnect));
            cacheManager.ThrowIfNull(nameof(cacheManager));
            _agentConnect = agentConnect;
            _cacheManager = cacheManager;
        }

        private IAgentConnect _agentConnect { get; }
        private ICacheManager _cacheManager { get; }

        public TransactionLookupResponse TransactionLookup(TransactionLookupRequest tranLookupRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.TransactionLookup(agent, tranLookupRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public ConsumerHistoryLookupResponse ConsumerHistoryLookup(ConsumerHistoryLookupRequest consumerHistoryLookupReq)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.ConsumerHistoryLookup(agent, consumerHistoryLookupReq);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public ReceiveValidationResponse ReceiveValidation(ReceiveValidationRequest receiveValidationReq)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.ReceiveValidation(agent, receiveValidationReq);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        /// <summary>
        ///     SEND, RECV Commit AgentConnect call
        /// </summary>
        /// <param name="completeSessionReq">CommitTransactionRequest Model</param>
        /// <returns>CommitTransactionResponse Model</returns>
        public CompleteSessionResponse CompleteSession(CompleteSessionRequest completeSessionReq)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.CompleteSession(agent, completeSessionReq);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        /// <summary>
        ///     MONEY ORDER MoneyOrderTotal AgentConnect Call
        /// </summary>
        /// <param name="moneyOrderTotalRequest">moneyOrderTotalRequest Model</param>
        /// <returns>MoneyOrderTotalResponse Model</returns>
        public MoneyOrderTotalResponse MoneyOrderTotal(MoneyOrderTotalRequest moneyOrderTotalRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.MoneyOrderTotal(agent, moneyOrderTotalRequest);
            if ((response.Payload?.Flags & 1) != 0 || (response.Payload?.Flags & 16) != 0)
            {
                // ignore code table updates.
            }
            else
            {
                CheckFlagsForDataChange(response.Payload?.Flags, agent);
            }
            return response;
        }

        /// <summary>
        ///     MONEY ORDER Compliance AgentConnect Call
        /// </summary>
        /// <param name="complianceTransactionRequest">ComplianceTransactionRequest Model</param>
        /// <returns>ComplianceTransactionResponse Model</returns>
        public ComplianceTransactionResponse ComplianceTransaction(ComplianceTransactionRequest complianceTransactionRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.ComplianceTransaction(agent, complianceTransactionRequest);

            if ((response.Payload?.Flags & 1) != 0 || (response.Payload?.Flags & 16) != 0)
            {
                // ignore code table updates.
            } else
            {
                CheckFlagsForDataChange(response.Payload?.Flags, agent);
            }

            return response;
        }

        /// <summary>
        ///     SEND SendValidation AgentConnect call
        /// </summary>
        /// <param name="sendValidationReq">SendValidationRequest Model</param>
        /// <returns>SendValidationResponse Model</returns>
        public SendValidationResponse SendValidation(SendValidationRequest sendValidationReq)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SendValidation(agent, sendValidationReq);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public ReceiveReversalValidationResponse ReceiveReversalValidation(ReceiveReversalValidationRequest receiveReversalValidationRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.ReceiveReversalValidation(agent, receiveReversalValidationRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public AmendValidationResponse AmendValidation(AmendValidationRequest amendValidationRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.AmendValidation(agent, amendValidationRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SendReversalValidationResponse SendReversalValidation(SendReversalValidationRequest sendReversalValidationRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SendReversalValidation(agent, sendReversalValidationRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetServiceOptionsResponse GetServiceOptions(GetServiceOptionsRequest getServiceOptionsRequest)
        {
            var agent = AuthIntegration.GetAgent();
            return _agentConnect.GetServiceOptions(agent, getServiceOptionsRequest);
        }

        public SearchStagedTransactionsResponse SearchStagedTransactions(SearchStagedTransactionsRequest searchStagedTransactionsRequest)
        {
            var agent = AuthIntegration.GetAgent();

            var response = _agentConnect.SearchStagedTransactions(agent, searchStagedTransactionsRequest);

            return response;
        }

        public GetBankDetailsByLevelResponse GetBankDetailsByLevel(GetBankDetailsByLevelRequest getBankDetailsByLevelRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetBankDetailsByLevel(agent, getBankDetailsByLevelRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetBankDetailsResponse GetBankDetails(GetBankDetailsRequest getBankDetailsRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetBankDetails(agent, getBankDetailsRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetDepositInformationResponse GetDepositInformation(GetDepositInformationRequest getDepositInformationRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetDepositInformation(agent, getDepositInformationRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetDepositBankListResponse GetDepositBankList(GetDepositBankListRequest getDepositBankListRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetDepositBankList(agent, getDepositBankListRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public DepositAnnouncementResponse DepositAnnouncement(DepositAnnouncementRequest depositAnnouncementRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.DepositAnnouncement(agent, depositAnnouncementRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public FeeLookupResponse FeeLookup(FeeLookupRequest feeLookupRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.FeeLookup(agent, feeLookupRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public List<SenderLookupInfo> BillPaymentConsumerLookup(ConsumerHistoryLookupRequest consumerHistoryLookupRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.ConsumerHistoryLookup(agent, consumerHistoryLookupRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response.Payload.SenderInfos.SenderInfo.ToList();
        }

        public BillerSearchResponse BillerSearch(BillerSearchRequest billerSearchRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.BillerSearch(agent, billerSearchRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public BPValidationResponse BPValidation(BPValidationRequest bpValidationRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.BpValidation(agent, bpValidationRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public DwInitialSetupResponse DwInitialSetup(DwInitialSetupRequest dwInitialSetupRequest)
        {
            var agent = AuthIntegration.GetAgent();
            if(dwInitialSetupRequest.DeviceID == null)
            {
                dwInitialSetupRequest.DeviceID = AuthIntegration.GetDeviceId();
            }
            // Map values of DeviceID and Device setup PIN to AgentId and Password
            agent.AgentId = dwInitialSetupRequest.DeviceID;
            agent.AgentSequence = string.Empty;
            agent.AgentPassword = dwInitialSetupRequest.Password;

            var response = _agentConnect.DwInitialSetup(agent, dwInitialSetupRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public DwProfileResponse DwProfile(DwProfileRequest dwProfileRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.DwProfile(agent, dwProfileRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetAllFieldsResponse GetAllFields(GetAllFieldsRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetAllFields(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetEnumerationsResponse GetEnumerations(GetEnumerationsRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetEnumerations(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                ServiceName = ServiceNames.AgentConnect,
                Message = "Not Implemented",
                StatusCode = StatusCode.NotImplemented
            };
        }

        private void CheckFlagsForDataChange(int? flags, Agent agent)
        {
            if(!flags.HasValue)
            {
                return;
            }
            // removed usage of code tables for country, subdivision, currency, and industry.
            // we should no longer react to its changes
            //            if((flags & 1) != 0 || (flags & 16) != 0)
            //            {
            //                // Formatted keys for cache
            //                var currencyInfoKeyFormatted = string.Format(CacheKeys.CURRENCYINFOKEY, agent.Language);
            //                var countriesKeyFormatted = string.Format(CacheKeys.COUNTRYINFOKEY, agent.Language);
            //                var countrySubdivitionKeyFormatted = string.Format(CacheKeys.COUNTRYSUBDIVISIONKEY, agent.Language);
            //                var industryKeyFormatted = string.Format(CacheKeys.INDUSTRYKEY, agent.Language);
            //
            //                // Delete cache for Countries, States, Currencies, Industries.
            //                _cacheManager.Remove(currencyInfoKeyFormatted, CacheRegion.Global);
            //                _cacheManager.Remove(countriesKeyFormatted, CacheRegion.Global);
            //                _cacheManager.Remove(countrySubdivitionKeyFormatted, CacheRegion.Global);
            //                _cacheManager.Remove(industryKeyFormatted, CacheRegion.Global);
            //
            //                // Re-fetch data and cache it - commented out.
            //                // Eager loading penalizes current request with unnecessary performance slow-down.
            //                //_agentConnect.GetCurrencyInfo(agent, new GetCurrencyInfoRequest());
            //                //_agentConnect.GetCountryInfo(agent, new GetCountryInfoRequest());
            //                //_agentConnect.GetCountrySubdivision(agent, new GetCountrySubdivisionRequest());
            //                //_agentConnect.Industry(agent);
            //            }
            if ((flags & 2) != 0)
            {
                //Delete CacheKeys.AUTHCLAIMS, CacheKeys.AGENTPROFILECLAIMS, CacheKeys.AGENTPROFILEKEY from cache
                ClearAgentProfileCache(agent);
            }
        }
        public void ClearAgentProfileCache(Agent agent)
        {
            var token = AuthIntegration.GetToken();
            var authClaimsKey = string.Format(CacheKeys.AUTHCLAIMS, token);
            var agentProfileKey = string.Format(CacheKeys.AGENTPROFILECLAIMS, token);
            var agentProfileKeyFormatted = string.Format(CacheKeys.AGENTPROFILEKEY, agent.AgentId, agent.AgentSequence);
            _cacheManager.Remove(authClaimsKey, CacheRegion.Global);
            _cacheManager.Remove(agentProfileKey, CacheRegion.Global);
            _cacheManager.Remove(agentProfileKeyFormatted, CacheRegion.Global);
        }
        #region LOOKUP

        public GetCountryInfoResponse GetCountryInfo(GetCountryInfoRequest getCountryInfoRequest)
        {
            var agent = AuthIntegration.GetAgent();

            return _agentConnect.GetCountryInfo(agent, getCountryInfoRequest);
        }

        public GetCountrySubdivisionResponse GetCountrySubdivision(GetCountrySubdivisionRequest req)
        {
            var agent = AuthIntegration.GetAgent();

            var acStatesResponse = _agentConnect.GetCountrySubdivision(agent, req);

            return acStatesResponse;
        }

        public GetCurrencyInfoResponse GetCurrencyInfo(GetCurrencyInfoRequest getCurrencyInfoRequest)
        {
            var agent = AuthIntegration.GetAgent();
            return _agentConnect.GetCurrencyInfo(agent, getCurrencyInfoRequest);
        }

        public IndustryResponse Industry(IndustryRequest request)
        {
            var agent = AuthIntegration.GetAgent();

            var industryResponse = _agentConnect.Industry(agent, request);

            return industryResponse;
        }

        public ProfileResponse Profile(ProfileRequest profileRequest)
        {
            var agent = AuthIntegration.GetAgent();
            return _agentConnect.Profile(agent, profileRequest);
        }

        public SaveProfileResponse SaveProfile(SaveProfileRequest saveProfileRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var saveProfileResponse = _agentConnect.SaveProfile(agent, saveProfileRequest);
            ClearAgentProfileCache(agent);
            return saveProfileResponse;
        }

        public GetProfileReceiverResponse GetProfileReceiver(GetProfileReceiverRequest getProfileReceiver)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetProfileReceiver(agent, getProfileReceiver);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetProfileSenderResponse GetProfileSender(GetProfileSenderRequest getProfileSender)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetProfileSender(agent, getProfileSender);            
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(CreateOrUpdateProfileReceiverRequest createOrUpdateProfileReceiverRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.CreateOrUpdateProfileReceiver(agent, createOrUpdateProfileReceiverRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(CreateOrUpdateProfileSenderRequest createOrUpdateProfileSenderRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.CreateOrUpdateProfileSender(agent, createOrUpdateProfileSenderRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SearchConsumerProfilesResponse SearchConsumerProfiles(SearchConsumerProfilesRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SearchConsumerProfiles(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

		public GetProfileConsumerResponse GetProfileConsumer(GetProfileConsumerRequest getProfileConsumerRequest)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetProfileConsumer(agent, getProfileConsumerRequest);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumer(CreateOrUpdateProfileConsumerRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.CreateOrUpdateProfileConsumer(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SaveConsumerProfileImageResponse SaveConsumerProfileImage(SaveConsumerProfileImageRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SaveConsumerProfileImage(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetConsumerProfileTransactionHistoryResponse GetConsumerProfileTransactionHistory(
            GetConsumerProfileTransactionHistoryRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetConsumerProfileTransactionHistory(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SaveConsumerProfileDocumentResponse SaveConsumerProfileDocument(SaveConsumerProfileDocumentRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SaveConsumerProfileDocument(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetConsumerProfileDocumentResponse GetConsumerProfileDocument(GetConsumerProfileDocumentRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetConsumerProfileDocument(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SavePersonalIDImageResponse SavePersonalIDImage(SavePersonalIDImageRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SavePersonalIDImage(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public GetPersonalIDImageResponse GetPersonalIDImage(GetPersonalIDImageRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.GetPersonalIDImage(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        public SaveTransactionDocumentResponse SaveTransactionDocument(SaveTransactionDocumentRequest req)
        {
            var agent = AuthIntegration.GetAgent();
            var response = _agentConnect.SaveTransactionDocument(agent, req);
            CheckFlagsForDataChange(response.Payload?.Flags, agent);
            return response;
        }

        #endregion
    }
}