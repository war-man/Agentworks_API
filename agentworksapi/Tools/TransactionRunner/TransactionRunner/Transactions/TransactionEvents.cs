using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using TransactionRunner.Interfaces;

namespace TransactionRunner.Transactions
{
	/// <summary>
	///     "Run" button clicked.
	/// </summary>
	public class RunTransactionsButtonClicked
	{

	}

	/// <summary>
	///     Used to display intermediate results - individual transactions
	/// </summary>
	public class TransactionResultsReceivedMessage
	{
	    private string _referenceNumber;

	    /// <summary>
	    /// Creates new instance of message
	    /// </summary>
	    /// <param name="mode"></param>
	    /// <param name="guid"></param>
	    public TransactionResultsReceivedMessage(TransactionQueueMode mode, Guid guid)
	    {
	        Mode = mode;
	        Guid = guid;
	    }

	    public TransactionQueueMode Mode { get; }

	    public Guid Guid { get; }

        /// <summary>
        ///     The transaction result.
        /// </summary>
        public ITransactionResult Result { get; set; }

		/// <summary>
		///     The output message that displays the details for the transaction.
		/// </summary>
		public string DetailsString { get; set; }

	    /// <summary>
	    ///     The reference number of the transaction.
	    /// </summary>
	    public string ReferenceNumber
	    {
	        get => string.IsNullOrEmpty(_referenceNumber) ? "ERROR" : _referenceNumber;
	        set => _referenceNumber = value;
	    }

        /// <summary>
        /// True when transaction resulted with an error
        /// </summary>
	    public bool IsError => string.IsNullOrEmpty(_referenceNumber);
	}

    /// <summary>
    /// Event raised when transaction in batch is started
    /// </summary>
    public class TransactionStartedEvent
    {
        /// <summary>
        /// Creates new event instance
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        public TransactionStartedEvent(TransactionQueueMode mode, Guid guid)
        {
            Mode = mode;
            Guid = guid;
        }

        /// <summary>
        /// Transaction queue mode
        /// </summary>
        public TransactionQueueMode Mode { get; }

        /// <summary>
        /// Transaction identifier
        /// </summary>
        public Guid Guid { get; }
    }

    /// <summary>
    /// Event raised when there is exception in transaction runner logic
    /// </summary>
    public class TransactionUnrecoverableErrorEvent
    {
        /// <summary>
        /// Creates event instance
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        /// <param name="exception"></param>
        public TransactionUnrecoverableErrorEvent(TransactionQueueMode mode, Guid guid, Exception exception)
        {
            Mode = mode;
            Guid = guid;
            Exception = exception;
        }

        /// <summary>
        /// Transaction queue mode
        /// </summary>
        public TransactionQueueMode Mode { get; }

        /// <summary>
        /// Transaction identifier
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Caught exception
        /// </summary>
        public Exception Exception { get; }
    }

    /// <summary>
    ///     Send after all individual transactions have completed.
    /// </summary>
    public class TransactionComplete
	{

	}

	/// <summary>
	///     Agent has changed, so update parameters view model if the agent is not valid.
	/// </summary>
	public class AgentChangedEvent
	{
		/// <summary>
		///     If the selected agent is valid or not.
		/// </summary>
		public bool AgentIsValid;
	}

    /// <summary>
    ///     The agent location has changed, alert any subscribing event to filter dropdowns accordingly.
    /// </summary>
    public class AgentLocationChangedEvent
    {

    }

    /// <summary>
    ///     Product has changeed (Send, Send Reversal, Amend, etc.).
    /// </summary>
    public class ProductChangeEvent
	{
		/// <summary>
		///     The type of the product.
		/// </summary>
		public SessionType ProductType;
        public string TransactionName;
	}

	/// <summary>
	///     User has added/edited/deleted agents, so update agent selection parameters.
	/// </summary>
	public class AgentManagementChangedEvent
	{
		/// <summary>
		/// 
		/// </summary>
		public IAgentSelectorViewModel AgentSelectorViewModel;

	};
	public class EnvironmentChangeEvent
	{
		public string SelectedEnvironment;

	}
	/// <summary>
	///		User has added/edited/deleted receiving agents, so update agent selection parameters.
	/// </summary>
	public class ReceiveAgentManagementChangedEvent
	{

	}

	public class SendStateChangedEvent
	{
		/// <summary>
		/// Sends a message when the send state is changed for those pieces listening for it
		/// </summary>
		public SubdivisionInfo SelectedCountrySubdivision;
	}
    
    /// <summary>
    /// Event to report progress of transaction batch
    /// </summary>
    public class TransactionBatchProgressEvent
    {
        /// <summary>
        /// Constructs new event instance
        /// </summary>
        /// <param name="progress"></param>
        public TransactionBatchProgressEvent(double progress)
        {
            Progress = progress;
        }

        /// <summary>
        /// Current progress value Range from 0.0 to 1.0
        /// </summary>
        public double Progress { get; private set; }
    }
}