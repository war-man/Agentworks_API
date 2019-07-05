using System;
using System.Threading.Tasks;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Provides shared functionality for all transactions
    /// </summary>
    public abstract class BaseHandler : ITransactionHandler
    {
        /// <summary>
        ///     Run the transaction
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<ITransactionResult> Run(object parameters, TransactionQueueMode mode, Guid guid)
        {
            StaticMessageBus.MessageBus.Publish(new TransactionStartedEvent(mode, guid));

            var task = Task.Run(() =>
            {
                try
                {
                    return Transaction(parameters);
                }
                catch (Exception exception)
                {
                    StaticMessageBus.MessageBus.Publish(new TransactionUnrecoverableErrorEvent(mode, guid, exception));
                    return null;
                }

            });
            task.ContinueWith(x => ProcessResults(x, mode, guid));
            return task;
        }

		/// <summary>
		///     Implement this to instantiate parameters.  To be called when Transaction is added to queue
		/// </summary>
		/// <returns>Parameters for this transaction.</returns>
		public abstract BaseParams BuildParams { get; }

		/// <summary>
		///     Display name for this transaction
		/// </summary>
		public abstract string Display { get; }

        /// <summary>
        ///     This sessions's type
        /// </summary>
        public abstract SessionType Type { get; }

        /// <summary>
        ///     This uniquely identifies the transaction name, it may still be send transaction type but its a stagedreceive..
        /// </summary>
        public abstract string TransactionName { get; }

        /// <summary>
        ///     Defines the transaction to be run.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Transaction result</returns>
        public abstract ITransactionResult Transaction(object parameters);

        /// <summary>
        ///     Implement this to handle the results of the transaction
        /// </summary>
        /// <param name="transactionResult"></param>
        /// <param name="mode"></param>
        /// <param name="guid"></param>
        public abstract void ResultHandler(ITransactionResult transactionResult, TransactionQueueMode mode, Guid guid);

        private Task ProcessResults(Task<ITransactionResult> result, TransactionQueueMode mode, Guid guid)
        {
            if(result == null)
            {
                return null;
            }
            return Task.Run(() =>
            {
                try
                {
                    ResultHandler(result.Result, mode, guid);
                }
                catch (Exception exception)
                {
                    StaticMessageBus.MessageBus.Publish(new TransactionUnrecoverableErrorEvent(mode, guid, exception));
                }
            });
        }
    }
}