using System;

namespace AwApi.ViewModels
{
    [Serializable]
    public class Device
    {
        public string DeviceSecurityLevel { get; set; }
        public string PlatformVersionEOLDate { get; set; }
        public string HostVersionEOLDate { get; set; }        

        public string OracleAccountNumber { get; set; }
        public string AgentStatus { get; set; }
        public bool IsRetailCredit { get; set; }
    }
}