using System;
using System.Threading.Tasks;
using TransactionRunner.Interfaces;

namespace TransactionRunner.Transactions
{
    /// <summary>
    ///     Class for a single transaction.  Uses handler to instantiate parameters for this running of a transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Identifier of transaction
        /// </summary>
        public Guid Guid { get; }

        private readonly object _params;

        private Task<ITransactionResult> _task;

        private Transaction()
        {
            Guid = Guid.NewGuid();
        }

        /// <summary>
        ///     Builds a new Transaction out of the given TransactionHandler.  Also builds params based on handler's BuildParams
        ///     function
        /// </summary>
        /// <param name="handler"></param>
        public Transaction(ITransactionHandler handler) : this()
        {
            Handler = handler;
            _params = handler.BuildParams;
        }

        /// <summary>
        /// Builds a new Transaction for handler and explicitely passed transactionParams (for imported transaction criteria)
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="transactionParams"></param>
        public Transaction(ITransactionHandler handler, BaseParams transactionParams) : this()
        {
            Handler = handler;
            _params = transactionParams;
        }

        /// <summary>
        ///     The handler for the transaction
        /// </summary>
        public ITransactionHandler Handler { get; }

        /// <summary>
        ///     Run the current transaction
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>Task that can be awaited for results of the transaction.</returns>
        public Task<ITransactionResult> Run(TransactionQueueMode mode)
        {
            _task = Handler.Run(_params, mode, Guid);

            return _task;
        }
    }
}