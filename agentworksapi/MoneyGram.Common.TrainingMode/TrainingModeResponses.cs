namespace MoneyGram.Common.TrainingMode
{
    /// <summary>
    /// Contains response file names for each of flow steps. All values are optional.
    /// </summary>
    public class TrainingModeResponses
    {
        /// <summary>
        /// Complete Session operation for Send
        /// </summary>
        public string SendCompleteSession { get; set; }

        /// <summary>
        /// Complete Session operation for Send (with thermal receipt)
        /// </summary>
        public string SendCompleteSessionThermal { get; set; }

        /// <summary>
        /// Create or update profile sender
        /// </summary>
        public string SendCreateOrUpdateProfileSender { get; set; }             

        /// <summary>
        /// Complete Session operation for Bill Pay
        /// </summary>
        public string BillPayCompleteSession { get; set; }

        /// <summary>
        /// Complete Session operation for Bill Pay (with thermal receipt)
        /// </summary>
        public string BillPayCompleteSessionThermal { get; set; }

        #region Receive
        /// <summary>
        /// Transaction Lookup for Receive
        /// </summary>
        public string ReceiveTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for Receive
        /// </summary>
        public string ReceiveValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for Receive
        /// </summary>
        public string ReceiveValidationNonEmpty { get; set; }

        /// <summary>
        /// Complete Session operation for Receive
        /// </summary>
        public string ReceiveCompleteSession { get; set; }

        /// <summary>
        /// Complete Session operation for Receive (with thermal receipt)
        /// </summary>
        public string ReceiveCompleteSessionThermal { get; set; }

        /// <summary>
        /// Search Consumer Profile For Receive
        /// </summary>
        public string ReceiveSearchConsumerProfile { get; set; }

        /// <summary>
        /// Get Receiver Profile
        /// </summary>
        public string ReceiveGetProfileReceiver { get; set; }

        /// <summary>
        /// Create Or Update Receiver Profile
        /// </summary>
        public string ReceiveCreateOrUpdateProfileReceiver { get; set; }

        #endregion

        #region Staged Send
        /// <summary>
        /// Transaction lookup for staged send
        /// </summary>
        public string StagedSendTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for staged send
        /// </summary>
        public string StagedSendValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for staged send
        /// </summary>
        public string StagedSendValidationNonempty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for staged send (with thermal receipts)
        /// </summary>
        public string StagedSendValidationNonemptyThermal { get; set; }

        /// <summary>
        /// Search staged transactions operation for send
        /// </summary>
        public string StagedSendSearchTransactions { get; set; }
        #endregion

        /// <summary>
        /// Search staged transactions operation for bill pay
        /// </summary>
        public string StagedBillPaySearchTransactions { get; set; }

        #region Staged Bill Pay
        /// <summary>
        /// Transaction lookup for staged bill pay
        /// </summary>
        public string StagedBillPayTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for staged bill pay
        /// </summary>
        public string StagedBillPayValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for staged bill pay
        /// </summary>
        public string StagedBillPayValidationNonempty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for staged bill pay (with thermal receipts)
        /// </summary>
        public string StagedBillPayValidationNonemptyThermal { get; set; }
        #endregion

        #region Staged Receive
        /// <summary>
        /// Transaction Lookup for Staged Receive
        /// </summary>
        public string StagedReceiveTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for Staged Receive
        /// </summary>
        public string StagedReceiveValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for Staged Receive on Dt4
        /// </summary>
        public string StagedReceiveValidationEmptyDt4 { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for Staged Receive
        /// </summary>
        public string StagedReceiveValidationNonEmpty { get; set; }

        /// <summary>
        /// Complete Session operation for Staged Receive
        /// </summary>
        public string StagedReceiveCompleteSession { get; set; }

        /// <summary>
        /// Complete Session operation for Staged Receive (with thermal receipt)
        /// </summary>
        public string StagedReceiveCompleteSessionThermal { get; set; }
        #endregion

        #region Amend
        /// <summary>
        /// Transaction Lookup for Amend
        /// </summary>
        public string AmendTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for Amend
        /// </summary>
        public string AmendValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for Amend
        /// </summary>
        public string AmendValidationNonempty { get; set; }

        /// <summary>
        /// Complete Session operation for Amend
        /// </summary>
        public string AmendCompleteSession { get; set; }
        #endregion

        #region Send Reversal

        /// <summary>
        /// Transaction Lookup for Send Reversal
        /// </summary>
        public string SendReversalTransactionLookup { get; set; }

        /// <summary>
        /// Validation (with empty FieldValues) for Send Reversal
        /// </summary>
        public string SendReversalValidationEmpty { get; set; }

        /// <summary>
        /// Validation (with nonempty FieldValues) for Send Reversal
        /// </summary>
        public string SendReversalValidationNonempty { get; set; }

        /// <summary>
        /// Complete Session operation for Send Reversal
        /// </summary>
        public string SendReversalCompleteSession { get; set; }
        #endregion

        /// <summary>
        /// Status Transaction Lookup response (for Amend and Send Reversal)
        /// </summary>
        /// <returns></returns>
        public string StatusTransactionLookup { get; set; }

        #region Report

        /// <summary>
        /// Daily Transaction Activity Report (for mt transaction)
        /// </summary>
        public string ReportDailyTransactionActivityMtTransaction { get; set; }

        /// <summary>
        /// Daily Transaction Activity Report (for bill pay transaction)
        /// </summary>
        public string ReportDailyTransactionActivityBillPayTransaction { get; set; }

        #endregion

        /// <summary>
        /// Save transaction document response mock (for all transaction types)
        /// </summary>
        public string SaveTransactionDocument { get; set; }
    }
}