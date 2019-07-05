using System;

namespace MoneyGram.Common
{
    [Serializable]
    public class TransactionalLimitsException : Exception
    {
        public TransactionalLimitsException()
        {
            ErrorCode = 403;
            ErrorString = "Cannot complete the transaction as the amount exceeds the user transaction thresholds.";
            TimeStamp = DateTime.Now;
        }

        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string DetailString { get; set; }
    }
}