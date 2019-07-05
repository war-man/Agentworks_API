using System;
using System.Runtime.Serialization;

namespace MoneyGram.PartnerService.DomainModel.Exceptions
{
    public partial class RelatedError
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }

    public class PartnerServiceException : Exception
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


        public PartnerServiceException()
            : base()
        {
            // Add implementation.
        }

        public PartnerServiceException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public PartnerServiceException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected PartnerServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}