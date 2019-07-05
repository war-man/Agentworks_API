using System;
using System.Threading.Tasks;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using TransactionRunner.Transactions;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     Interface for handling transactions, whatever they are
    /// </summary>
    public interface ITransactionHandler
    {
        /// <summary>
        ///     Run the transaction.  Implemented in baseHandler
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<ITransactionResult> Run(object parameters, TransactionQueueMode mode, Guid guid);

        /// <summary>
        ///     Defines the transaction to be run.  Put the long sync code here.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Transaction result</returns>
        ITransactionResult Transaction(object parameters);

        /// <summary>
        ///     Implement this to instantiate parameters.  To be called when Transaction is added to queue
        /// </summary>
        /// <returns>parameters for this transaction.</returns>
        BaseParams BuildParams { get; }

		/// <summary>
		///     Display name for this transaction
		/// </summary>
		string Display { get; }
        
        /// <summary>
        ///     This sessions's type
        /// </summary>
        SessionType Type { get; }

        /// <summary>
        /// Name of transaction for export file
        /// </summary>
        string TransactionName { get; }
    }
}