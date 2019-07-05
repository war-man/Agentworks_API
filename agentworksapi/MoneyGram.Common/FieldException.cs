using System;

namespace MoneyGram.Common
{
    [Serializable]
    public class FieldException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public int SubErrorCode { get; set; }
        public string OffendingField { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string DetailString { get; set; }

        public override string Message
        {
            get
            {
                var errCodeStr = (ErrorCode != 0 ? " - ErrorCode: " + ErrorCode : string.Empty);
                var errString = (!string.IsNullOrEmpty(ErrorString) ? " - ErrorString: " + ErrorString : string.Empty);
                var subErrorCodeStr = (SubErrorCode != 0 ? " - SubErrorCode: " + SubErrorCode : string.Empty);
                var offendingFieldStr = (!string.IsNullOrEmpty(OffendingField) ? " - OffendingField: " + OffendingField : string.Empty);
                var detailStr = (!string.IsNullOrEmpty(DetailString) ? " - DetailString: " + DetailString : string.Empty);
                return $"{base.Message}{errCodeStr}{errString}{subErrorCodeStr}{offendingFieldStr}{detailStr}";
            }
        }
    }
}