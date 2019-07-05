using System;
using MoneyGram.Common.Models;

namespace MoneyGram.Common
{
    [Serializable]
    public class DeepHealthCheckException : Exception
    {
        public DeepHealthCheckException()
        {
            ErrorCode = StatusCode.InternalServerError;
            ErrorString = "";
            TimeStamp = DateTime.Now;
        }
        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public DateTime TimeStamp { get; set; }
        public string DetailString { get; set; }
    }
}