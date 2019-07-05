using System.Linq;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Extensions;

namespace AwApi.Integration
{
    public class AgentConnectTraining: AgentConnectCache
    {
        private const string UseAgentPrinterKey = "USE_AGENT_PRINTER";
        private const string UseOurPaperKey = "USE_OUR_PAPER";

        public AgentConnectTraining(IAgentConnect agentConnect, ICacheManager cacheManager, ITrainingModeRepository trainingModeRepository)
            : base(agentConnect, cacheManager)
        {
            trainingModeRepository.ThrowIfNull(nameof(trainingModeRepository));
            _traingModeRepository = trainingModeRepository;
        }

        private readonly ITrainingModeRepository _traingModeRepository;

        public override CompleteSessionResponse CompleteSession(Agent agent, CompleteSessionRequest completeSessionRequest)
        {
            if (agent.IsInTrainingMode)
            {
                bool shouldReturnThermal = !GetProfileItem(agent, UseAgentPrinterKey);
                return _traingModeRepository.MockCompleteSessionResponse(completeSessionRequest.MgiSessionType, completeSessionRequest.MgiSessionID, shouldReturnThermal);                
            }
            return base.CompleteSession(agent, completeSessionRequest);
        }

        private bool GetProfileItem(Agent agent, string profileItemKey)
        {
            var profile = Profile(agent, new ProfileRequest());
            var profileItem = profile?.Payload?.ProfileItem?.FirstOrDefault(x => x.Key == profileItemKey);
            return profileItem != null && profileItem.Value == "Y";
        }

        public override TransactionLookupResponse TransactionLookup(Agent agent, TransactionLookupRequest transactionLookupRequest)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockTransactionLookupResponse(
                    transactionLookupRequest.ReferenceNumber ?? transactionLookupRequest.ConfirmationNumber, transactionLookupRequest.PurposeOfLookup);
            }
            return base.TransactionLookup(agent, transactionLookupRequest);
        }

        public override ReceiveValidationResponse ReceiveValidation(Agent agent, ReceiveValidationRequest receiveValidationRequest)
        {
            if (agent.IsInTrainingMode)
            {
                bool isOnDt4 = GetProfileItem(agent, UseOurPaperKey);
                return _traingModeRepository.MockReceiveValidationResponse(receiveValidationRequest.MgiSessionID, receiveValidationRequest.FieldValues, isOnDt4);
            }
            return base.ReceiveValidation(agent, receiveValidationRequest);
        }

        public override SearchStagedTransactionsResponse SearchStagedTransactions(Agent agent, SearchStagedTransactionsRequest searchStagedTransactionsRequest)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockSearchStagedTransactionsResponse(searchStagedTransactionsRequest.MgiSessionType.Value);
            }
            return base.SearchStagedTransactions(agent, searchStagedTransactionsRequest);
        }

        public override SendValidationResponse SendValidation(Agent agent, SendValidationRequest request)
        {
            if (agent.IsInTrainingMode && _traingModeRepository.IsStagedTransaction(SessionType.SEND, request.MgiSessionID))
            {
                bool shouldReturnThermal = !GetProfileItem(agent, UseAgentPrinterKey);
                return _traingModeRepository.MockSendValidationResponse(request.MgiSessionID, request.FieldValues, shouldReturnThermal);
            }

            return base.SendValidation(agent, request);
        }

        public override BPValidationResponse BpValidation(Agent agent, BPValidationRequest request)
        {
            if (agent.IsInTrainingMode && _traingModeRepository.IsStagedTransaction(SessionType.BP, request.MgiSessionID))
            {
                bool shouldReturnThermal = !GetProfileItem(agent, UseAgentPrinterKey);
                return _traingModeRepository.MockBillPayValidationResponse(request.MgiSessionID, request.FieldValues, shouldReturnThermal);
            }

            return base.BpValidation(agent, request);
        }

        public override AmendValidationResponse AmendValidation(Agent agent, AmendValidationRequest request)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockAmendValidationResponse(request.MgiSessionID, request.FieldValues);
            }

            return base.AmendValidation(agent, request);
        }

        public override SendReversalValidationResponse SendReversalValidation(Agent agent, SendReversalValidationRequest request)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockSendReversalValidationResponse(request.MgiSessionID, request.FieldValues);
            }

            return base.SendReversalValidation(agent, request);
        }

        public override SearchConsumerProfilesResponse SearchConsumerProfiles(Agent agent, SearchConsumerProfilesRequest request)
        {
            if (agent.IsInTrainingMode)
            {
                
                string criteria = request.FieldValues.SingleOrDefault(x => x.InfoKey == "search_CriteriaName")?.Value;

                if (criteria == SearchProfileCriteria.REF_NUM_DOB_SEARCH) //RECEIVE
                {
                    return _traingModeRepository.MockSearchConsumerProfilesResponse(request.FieldValues);
                }
            }

            return base.SearchConsumerProfiles(agent, request);
        }

        public override CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSender(Agent agent,
            CreateOrUpdateProfileSenderRequest createOrUpdateProfileSenderRequest)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockCreateOrUpdateProfileSender();
            }
            return base.CreateOrUpdateProfileSender(agent, createOrUpdateProfileSenderRequest);
        }

        public override GetProfileReceiverResponse GetProfileReceiver(Agent agent, GetProfileReceiverRequest request)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockGetProfileReceiver(request.MgiSessionID);
            }

            return base.GetProfileReceiver(agent, request);
        }

        public override CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiver(Agent agent, CreateOrUpdateProfileReceiverRequest request)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockCreateOrUpdateProfileReceiver();
            }

            return base.CreateOrUpdateProfileReceiver(agent, request);
        }

        public override SaveTransactionDocumentResponse SaveTransactionDocument(Agent agent, SaveTransactionDocumentRequest saveTransactionDocumentRequest)
        {
            if (agent.IsInTrainingMode)
            {
                return _traingModeRepository.MockSaveTransactionDocument();
            }

            return base.SaveTransactionDocument(agent, saveTransactionDocumentRequest);
        }
    }
}