using TransactionRunner.Interfaces;

namespace TransactionRunner.Transactions
{
    public class TransactionResult : ITransactionResult
    {
        public object Result { get; set; }
    }
}