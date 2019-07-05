using System;
using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.EntityMapper.CustomExtensions;
using MoneyGram.Common.TrainingMode;

namespace MoneyGram.AgentConnect
{
    /// <summary>
    /// Repository for TrainingMode    
    /// </summary>
    public class TrainingModeRepository: ITrainingModeRepository
    {
        /// <summary>
        /// Mock complete session response depends on transaction type
        /// </summary>
        /// <param name="transactionType">Type of transation</param>
        /// <param name="mgiSessionId"></param>
        /// <param name="thermalReceipts">When true, thermal receipts will be returned</param>
        /// <returns>Mocked complete session response</returns>
        public CompleteSessionResponse MockCompleteSessionResponse(SessionType transactionType, string mgiSessionId, bool thermalReceipts)
        {
            Func<TrainingModeResponses, string> getFileName = null;
            switch (transactionType)
            {
                case SessionType.SEND:
                    getFileName = thermalReceipts
                        ? (Func<TrainingModeResponses, string>)(r => r.SendCompleteSessionThermal)
                        : (r => r.SendCompleteSession);
                    break;
                case SessionType.BP:
                    getFileName = thermalReceipts
                        ? (Func<TrainingModeResponses, string>)(r => r.BillPayCompleteSessionThermal)
                        : (r => r.BillPayCompleteSession);
                    break;
                case SessionType.RCV:
                    getFileName = thermalReceipts
                        ? (Func<TrainingModeResponses, string>)(r => IsStagedTransaction(SessionType.RCV, mgiSessionId) ? r.StagedReceiveCompleteSessionThermal : r.ReceiveCompleteSessionThermal)
                        : (r => IsStagedTransaction(SessionType.RCV, mgiSessionId) ? r.StagedReceiveCompleteSession : r.ReceiveCompleteSession);
                    break;
                case SessionType.AMD:
                    getFileName = r => r.AmendCompleteSession;
                    break;
                case SessionType.SREV:
                    getFileName = r => r.SendReversalCompleteSession;
                    break;
            }

            return TrainingModeConfiguration.GetResponse<CompleteSessionResponse>(transactionType.ToTrainingModeSessionType(), mgiSessionId, getFileName);
        }

        /// <summary>
        /// Mock transactionlookup response depends on reference number
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <param name="purposeOfLookup"></param>
        /// <returns></returns>
        public TransactionLookupResponse MockTransactionLookupResponse(string referenceNumber, string purposeOfLookup)
        {
            SessionType sessionType = SessionType.AMD;

            Func<TrainingModeResponses, string> getFileName = null;

            TransactionLookupResponse response = null;
            switch (purposeOfLookup)
            {
                case PurposeOfLookup.Receive:
                    sessionType = SessionType.RCV;
                    getFileName = x => x.ReceiveTransactionLookup;
                    break;
                case PurposeOfLookup.ReceiveCompletion:
                    sessionType = SessionType.RCV;
                    getFileName = x => x.StagedReceiveTransactionLookup;
                    break;
                case PurposeOfLookup.SendCompletion:
                    sessionType = SessionType.SEND;
                    getFileName = x => x.StagedSendTransactionLookup;
                    break;
                case PurposeOfLookup.BillPayCompletion:
                    sessionType = SessionType.BP;
                    getFileName = x => x.StagedBillPayTransactionLookup;
                    break;
                case PurposeOfLookup.Amend:
                    sessionType = SessionType.AMD;
                    getFileName = x => x.AmendTransactionLookup;
                    break;
                case PurposeOfLookup.SendReversal:
                    sessionType = SessionType.SREV;
                    getFileName = x => x.SendReversalTransactionLookup;
                    break;
                case PurposeOfLookup.Status:
                    response = TrainingModeConfiguration.GetTransactionLookupStatusResponse<TransactionLookupResponse>(referenceNumber, x => x.StatusTransactionLookup);
                    break;
            }

            if (response == null)
            {
                response = TrainingModeConfiguration.GetResponse<TransactionLookupResponse>(sessionType.ToTrainingModeSessionType(), referenceNumber, getFileName);
            }
            
            var sendDate = response.Payload.CurrentValues.FirstOrDefault(x => x.InfoKey == "dateTimeSent");
            if (sendDate != null)
            {
                int.TryParse(sendDate.Value, out int daysAgo); //try to parse value from mock file. If it's int and parses it's added to current date. If parsing fails it is default of 0.
                sendDate.Value = DateTime.Now.AddDays(daysAgo).ToString("yyyy-MM-ddTHH:mm:ss.ffffzzz");
            }
            return response;
        }

        public ReceiveValidationResponse MockReceiveValidationResponse(string mgiSessionId,
            IList<KeyValuePairType> fieldValues, bool agentIsOnDt4)
        {
            const string receiverDoBKey = "receiver_DOB";

            Func<TrainingModeResponses, string> getFileName;
            if (IsStagedTransaction(SessionType.RCV, mgiSessionId))
            {
                getFileName = responses => fieldValues.Any(x => x.InfoKey != receiverDoBKey) ?
                    responses.StagedReceiveValidationNonEmpty : 
                    (agentIsOnDt4 ? responses.StagedReceiveValidationEmptyDt4 : responses.StagedReceiveValidationEmpty);
            }
            else
            {
                getFileName = responses => fieldValues.Any(x => x.InfoKey != receiverDoBKey) ? responses.ReceiveValidationNonEmpty : responses.ReceiveValidationEmpty;
            }

            return TrainingModeConfiguration.GetResponse<ReceiveValidationResponse>(SessionType.RCV.ToTrainingModeSessionType(), mgiSessionId, getFileName);
        }

        public SearchStagedTransactionsResponse MockSearchStagedTransactionsResponse(SessionType transactionType)
        {
            if (transactionType == SessionType.SEND)
            {
                return TrainingModeConfiguration.GetResponse<SearchStagedTransactionsResponse>(transactionType.ToTrainingModeSessionType(), null, x => x.StagedSendSearchTransactions);
            }
            if (transactionType == SessionType.BP)
            {
                return TrainingModeConfiguration.GetResponse<SearchStagedTransactionsResponse>(transactionType.ToTrainingModeSessionType(), null, x => x.StagedBillPaySearchTransactions);
            }
            throw new ArgumentException("Unsupported transaction type");
        }

        public SendValidationResponse MockSendValidationResponse(string mgiSessionId, IList<KeyValuePairType> fieldValues, bool thermalReceipts)
        {
            if (!fieldValues.Any())
            {
                return TrainingModeConfiguration.GetResponse<SendValidationResponse>(SessionType.SEND.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedSendValidationEmpty);
            }

            if (thermalReceipts)
            {
                return TrainingModeConfiguration.GetResponse<SendValidationResponse>(SessionType.SEND.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedSendValidationNonemptyThermal);
            }

            return TrainingModeConfiguration.GetResponse<SendValidationResponse>(SessionType.SEND.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedSendValidationNonempty);
        }

        public BPValidationResponse MockBillPayValidationResponse(string mgiSessionId, IList<KeyValuePairType> fieldValues, bool thermalReceipts)
        {
            if (!fieldValues.Any())
            {
                return TrainingModeConfiguration.GetResponse<BPValidationResponse>(SessionType.BP.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedBillPayValidationEmpty);
            }

            if (thermalReceipts)
            {
                return TrainingModeConfiguration.GetResponse<BPValidationResponse>(SessionType.BP.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedBillPayValidationNonemptyThermal);
            }

            return TrainingModeConfiguration.GetResponse<BPValidationResponse>(SessionType.BP.ToTrainingModeSessionType(), mgiSessionId, x => x.StagedBillPayValidationNonempty);
        }

        public bool IsStagedTransaction(SessionType sessionType, string mgiSessionId)
        {
            return TrainingModeConfiguration.IsStagedTransaction(sessionType.ToTrainingModeSessionType(), mgiSessionId);
        }

        public AmendValidationResponse MockAmendValidationResponse(string mgiSessionId, List<KeyValuePairType> fieldValues)
        {
            if (!fieldValues.Any())
            {
                return TrainingModeConfiguration.GetResponse<AmendValidationResponse>(SessionType.AMD.ToTrainingModeSessionType(), mgiSessionId, x => x.AmendValidationEmpty);
            }

            return TrainingModeConfiguration.GetResponse<AmendValidationResponse>(SessionType.AMD.ToTrainingModeSessionType(), mgiSessionId, x => x.AmendValidationNonempty);
        }

        public SendReversalValidationResponse MockSendReversalValidationResponse(string mgiSessionId, List<KeyValuePairType> fieldValues)
        {
            if (!fieldValues.Any())
            {
                return TrainingModeConfiguration.GetResponse<SendReversalValidationResponse>(SessionType.SREV.ToTrainingModeSessionType(), mgiSessionId, x => x.SendReversalValidationEmpty);
            }

            return TrainingModeConfiguration.GetResponse<SendReversalValidationResponse>(SessionType.SREV.ToTrainingModeSessionType(), mgiSessionId, x => x.SendReversalValidationNonempty);
        }

        public SearchConsumerProfilesResponse MockSearchConsumerProfilesResponse(List<KeyValuePairType> fieldValues)
        {
            string criteria = fieldValues.SingleOrDefault(x => x.InfoKey == "search_CriteriaName")?.Value;

            if (criteria == SearchProfileCriteria.REF_NUM_DOB_SEARCH) //RECEIVE
            {
                string refNo = fieldValues.Single(x => x.InfoKey == "consumer_ReferenceNumber").Value;
                return TrainingModeConfiguration.GetResponse<SearchConsumerProfilesResponse>(SessionType.RCV.ToTrainingModeSessionType(), refNo, x => x.ReceiveSearchConsumerProfile);
            }

            //returning empty consumer list
            return TrainingModeConfiguration.GetResponse<SearchConsumerProfilesResponse>(SessionType.RCV.ToTrainingModeSessionType(), null, x => x.ReceiveSearchConsumerProfile);
        }

        /// <summary>
        /// Mock Create Consumer Profile for Sender
        /// </summary>
        /// <returns></returns>
        public CreateOrUpdateProfileSenderResponse MockCreateOrUpdateProfileSender()
        {
            var resp = TrainingModeConfiguration.GetResponse<CreateOrUpdateProfileSenderResponse>(Common.TrainingMode.Enums.SessionType.SEND, null, x => x.SendCreateOrUpdateProfileSender);
            return resp;
        }

        public GetProfileReceiverResponse MockGetProfileReceiver(string mgiSessionId)
        {
            return TrainingModeConfiguration.GetResponse<GetProfileReceiverResponse>(SessionType.RCV.ToTrainingModeSessionType(), mgiSessionId, x => x.ReceiveGetProfileReceiver);
        }

        public CreateOrUpdateProfileReceiverResponse MockCreateOrUpdateProfileReceiver()
        {
            return TrainingModeConfiguration.GetResponse<CreateOrUpdateProfileReceiverResponse>(SessionType.RCV.ToTrainingModeSessionType(), null, x => x.ReceiveCreateOrUpdateProfileReceiver);
        }

        public SaveTransactionDocumentResponse MockSaveTransactionDocument()
        {
            return TrainingModeConfiguration.GetResponse<SaveTransactionDocumentResponse>(null, null, x => x.SaveTransactionDocument);
        }
    }
}