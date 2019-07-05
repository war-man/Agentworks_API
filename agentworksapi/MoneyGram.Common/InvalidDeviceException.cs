using System;

namespace MoneyGram.Common
{
    [Serializable]
    public class InvalidDeviceException : Exception
    {
        public InvalidDeviceException()
        {
            ErrorCode = 450;
            ErrorString = "DeviceId does not match with logged-in DeviceId ";
            TimeStamp = DateTime.Now;
        }

        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string DetailString { get; set; }
    }
}