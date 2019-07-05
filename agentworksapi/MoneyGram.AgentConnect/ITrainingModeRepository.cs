using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect
{
    /// <summary>
    /// Interface of Training mode repository
    /// </summary>
    public interface ITrainingModeRepository
    {
        /// <summary>
        /// Mock complete session response depends on transaction type
        /// </summary>
        /// <param name="transactionType">Type of transation</param>
        /// <param name="mgiSessionId">Mgi Session Id</param>
        /// <param name="thermalReceipts">When true, thermal receipts will be returned</param>
        /// <returns>Mocked complete session response</returns>
        CompleteSessionResponse MockCompleteSessionResponse(SessionType transactionType, string mgiSessionId, bool thermalReceipts);

        /// <summary>
        /// Mock transactionlookup response depends on reference number
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <param name="purposeOfLookup"></param>
        /// <returns></returns>
        TransactionLookupResponse MockTransactionLookupResponse(string referenceNumber, string purposeOfLookup);

        /// <summary>
        /// Mocks receive validation response based on mgiSessionId
        /// </summary>
        /// <param name="mgiSessionId">Mgi Session Id</param>
        /// <param name="fieldValues">Values of fields</param>
        /// <param name="agentIsOnDt4">True when agent is on Dt4</param>
        /// <returns>Mocked response from static file</returns>
        ReceiveValidationResponse MockReceiveValidationResponse(string mgiSessionId,
            IList<KeyValuePairType> fieldValues, bool agentIsOnDt4);

        /// <summary>
        /// Mocks search staged transactions response based on transaction type
        /// </summary>
        /// <param name="transactionType">Session type</param>
        /// <returns>Mocked response from static file</returns>
        SearchStagedTransactionsResponse MockSearchStagedTransactionsResponse(SessionType transactionType);

        /// <summary>
        /// Mocks Send validation (for staged transactions)
        /// </summary>
        /// <param name="mgiSessionId">MgiSessionId</param>
        /// <param name="fieldValues">Values of fields</param>
        /// <param name="thermalReceipts">When true, thermal receipts will be returned</param>
        /// <returns></returns>
        SendValidationResponse MockSendValidationResponse(string mgiSessionId, IList<KeyValuePairType> fieldValues, bool thermalReceipts);

        /// <summary>
        /// Mocks Bill Pay validation (for staged transactions)
        /// </summary>
        /// <param name="mgiSessionId">MgiSessionId</param>
        /// <param name="fieldValues">Values of fields</param>
        /// <param name="thermalReceipts">When true, thermal receipts will be returned</param>
        /// <returns></returns>
        BPValidationResponse MockBillPayValidationResponse(string mgiSessionId, IList<KeyValuePairType> fieldValues, bool thermalReceipts);

        /// <summary>
        /// Returns true when session id is handled as staging transaction (Send or Bill Pay)
        /// </summary>
        /// <param name="sessionType">Session type</param>
        /// <param name="mgiSessionId">MgiSessionId from request</param>
        /// <returns></returns>
        bool IsStagedTransaction(SessionType sessionType, string mgiSessionId);

        /// <summary>
        /// Mocks Amend validation
        /// </summary>
        /// <param name="mgiSessionId">MgiSessionId</param>
        /// <param name="fieldValues">Values of fields</param>
        /// <returns></returns>
        AmendValidationResponse MockAmendValidationResponse(string mgiSessionId, List<KeyValuePairType> fieldValues);

        /// <summary>
        /// Mocks Send Reversal validation
        /// </summary>
        /// <param name="mgiSessionId">MgiSessionId</param>
        /// <param name="fieldValues">Values of fields</param>
        /// <returns></returns>
        SendReversalValidationResponse MockSendReversalValidationResponse(string mgiSessionId, List<KeyValuePairType> fieldValues);

        /// <summary>
        /// Mocks Search Consumer Profiles
        /// </summary>
        /// <param name="fieldValues">Values of fields</param>
        /// <returns></returns>
        SearchConsumerProfilesResponse MockSearchConsumerProfilesResponse(List<KeyValuePairType> fieldValues);

        /// <summary>
        /// Create or update profile sender
        /// </summary>
        /// <returns></returns>
        CreateOrUpdateProfileSenderResponse MockCreateOrUpdateProfileSender();

        /// <summary>
        /// Mocks Get Profile Receiver
        /// </summary>
        /// <param name="mgiSessionId">MgiSessionId</param>
        /// <returns></returns>
        GetProfileReceiverResponse MockGetProfileReceiver(string mgiSessionId);

        /// <summary>
        /// Mocks Create Or Update Profile Receiver
        /// </summary>
        /// <returns></returns>
        CreateOrUpdateProfileReceiverResponse MockCreateOrUpdateProfileReceiver();

        /// <summary>
        /// Mocks Create Or Update Profile Receiver
        /// </summary>
        /// <returns></returns>
        SaveTransactionDocumentResponse MockSaveTransactionDocument();
    }
}