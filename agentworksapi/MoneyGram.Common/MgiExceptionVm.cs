using System;

namespace MoneyGram.Common
{
    [Serializable]
    public class MgiExceptionVm
    {
        public MgiExceptionType ExceptionType { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public int SubErrorCode { get; set; }
        public string OffendingField { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string DetailString { get; set; }
    }
}