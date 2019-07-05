using System;
using System.Runtime.Serialization;

namespace MoneyGram.DLS.DomainModel.Exceptions
{
    public partial class RelatedError
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }

    public class DLSException : Exception
    {

        public enum ErrorHandlingCode
        {
            RetryNow,
            RetryLater,
            ReturnError
        }

        public enum ErrorCategoryCode
        {
            ClientIntegrationError,
            UserError,
            ServiceSystemError
        }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
        public ErrorHandlingCode ErrorHandlingCodes;
        private bool ErrorHandlingCodeFieldSpecified;
        private ErrorCategoryCode ErrorCategoryCodes;
        private bool ErrorCategoryCodeFieldSpecified;

        public DateTime? TimeStamp { get; set; }


        public DLSException()
            : base()
        {
            // Add implementation.
        }

        public DLSException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public DLSException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected DLSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}