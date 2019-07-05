using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionRunner.Interfaces;

namespace TransactionRunner.Transactions
{
    /// <summary>
    ///     Transaction Queue
    /// </summary>
    public interface ITransactionQueue
    {
        /// <summary>
        ///     Run the current queue.
        /// </summary>
        /// <returns></returns>
        Task Run(TransactionQueueMode mode);

        /// <summary>
        ///     Add a new Transaction to the queue based on handler provided.
        /// </summary>
        /// <param name="t"></param>
        Guid Add(ITransactionHandler t);

        /// <summary>
        ///     Clear current transactions
        /// </summary>
        void Reset();

        /// <summary>
        ///     Does the queue contain transactions
        /// </summary>
        /// <returns>true if there are transactions</returns>
        bool HasTransactions();

        /// <summary>
        ///     The transactions in the queue
        /// </summary>
        List<Transaction> Transactions { get; }

        /// <summary>
        /// Create and add a new transaction to the queue (for imported transaction criteria).
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="transactionParams"></param>
        Guid Add(ITransactionHandler handler, BaseParams transactionParams);
    }
}