using System;

namespace AwLogAnalyzer.Models
{
    public class Record
    {
        public DateTime DateStamp { get; set; }
        public string ApiName { get; set; }
        public int TotalElapsedMilliseconds { get; set; }
        public int ElapsedMilliseconds { get; set; }
        public string ApplicationName { get; set; }
        public string ServerIp { get; set; }
        public string UserName { get; set; }
        public string Method { get; set; }
        public string transactionId { get; set; }
        public bool IsBeginRequest { get; set; }
        public bool IsEndRequest { get; set; }
        public bool IsSoap { get; set; }
        public string SoapCall { get; set; }
        public string MethodAction { get; set; }
    }
}