using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionRunner.Interfaces;
using TransactionRunner.Models;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Transactions
{
    /// <summary>
    ///     Queue to add transactions that will be run
    /// </summary>
    public class TransactionQueue : ITransactionQueue
    {
        private readonly IMessageBus _messageBus;
        private readonly ITransactionPickerViewModel _transactionPicker;
        private List<Transaction> _transactions;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="messageBus"></param>
        /// <param name="transactionPicker"></param>
        public TransactionQueue(IMessageBus messageBus, ITransactionPickerViewModel transactionPicker)
        {
            _messageBus = messageBus;
            _transactionPicker = transactionPicker;
            _messageBus.Subscribe<RunTransactionsButtonClicked>(RunTransactionButtonSubscriber);
        }

        /// <summary>
        ///     List of transactions to run.
        /// </summary>
        public List<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());

        /// <summary>
        ///     Run all the transactions in FIFO order
        /// </summary>
        /// <returns></returns>
        public async Task Run(TransactionQueueMode mode)
        {
            try
            {
                foreach (var transaction in Transactions)
                {
                    var currentIndex = Transactions.IndexOf(transaction);
                    if (currentIndex == Transactions.Count - 1)
                    {
                        // We are on the last one, so disable the stop button.
                        _messageBus.Publish(new DisableStopButtonEvent());
                    }

                    if (mode == TransactionQueueMode.Import)
                    {
                        _messageBus.Publish(new TransactionBatchProgressEvent((double)(currentIndex + 1) / Transactions.Count));
                    }

                    await transaction.Run(mode);
                    if (StaticTaskCancellation.TaskCancellation.CancelTasks)
                    {
                        break;
                    }
                }
                _messageBus.Publish(new TransactionBatchProgressEvent(1.0));
            }
            catch (Exception ex)
            {
                throw new System.Exception("There was an error.");
            }
            finally
            {
                TransactionCompleted();
            }
        }

        /// <summary>
        ///     Create and add a new transaction to the queue from the provided handler
        /// </summary>
        /// <param name="handler"></param>
        public Guid Add(ITransactionHandler handler)
        {
            Transaction transaction = new Transaction(handler);
            Transactions.Add(transaction);
            return transaction.Guid;
        }

        /// <summary>
        /// Create and add a new transaction to the queue (for imported transaction criteria).
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="transactionParams"></param>
        public Guid Add(ITransactionHandler handler, BaseParams transactionParams)
        {
            Transaction transaction = new Transaction(handler, transactionParams);
            Transactions.Add(transaction);
            return transaction.Guid;
        }

        /// <summary>
        ///     Clears all current transactions
        /// </summary>
        public void Reset()
        {
            Transactions.Clear();
        }

        /// <summary>
        ///     Does the queue contain transactions
        /// </summary>
        /// <returns>true if there are transactions</returns>
        public bool HasTransactions()
        {
            return Transactions.Count > 0;
        }

        private void TransactionCompleted()
        {
            _messageBus.Publish(new TransactionComplete());

            Reset();
        }
        private void RunTransactionButtonSubscriber(RunTransactionsButtonClicked evt)
        {
            var batchNumberVm = StaticBatchNumberVm.BatchNumberViewModel;
            var batchNumber = batchNumberVm.BatchNumber;
            if (!HasTransactions())
            {
                for (int i = 0; i < batchNumber; i++)
                {
                    Add(_transactionPicker.SelectedTransaction);
                }
            }

            Run(TransactionQueueMode.Batch);
        }
    }
}