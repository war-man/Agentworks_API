using System;
using System.Runtime.Serialization;

namespace MoneyGram.AgentConnect.DomainModel.Exceptions
{
    public class AgentConnectException : Exception
    {
        public AgentConnectException()
        {
            // Add implementation.
        }

        public AgentConnectException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public AgentConnectException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected AgentConnectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }

        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public int SubErrorCode { get; set; }
        public string OffendingField { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string DetailString { get; set; }
    }
}