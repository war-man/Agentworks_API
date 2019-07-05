using TransactionRunner.Messaging;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Transactions
{
    internal class StaticTransactionQueue
    {
        private static ITransactionQueue _transactionQueue;

        public static ITransactionQueue TransactionQueue
        {
            get
            {
                return _transactionQueue ?? (_transactionQueue = new TransactionQueue(StaticMessageBus.MessageBus,
                           StaticTransactionPickerVm.TransactionPickerViewModel));
            }
            private set { _transactionQueue = value; }
        }
    }
}