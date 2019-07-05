using System;

namespace AwApi.ViewModels
{
    [Serializable]
    public class DwRegisterDeviceReqVm
    {
        public string DeviceId { get; set; }
        public string SetupPin { get; set; }
        public string MgiDeviceSession { get; set; }
        public int PosUnitProfileId { get; set; }
        public string ClientSoftwareVersion { get; set; }
        public string PoeType { get; set; }
        public string ChannelType { get; set; }
        public string TargetAudience { get; set; }
    }
}