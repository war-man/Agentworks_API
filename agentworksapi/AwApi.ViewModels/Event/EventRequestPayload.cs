using System;

namespace AwApi.ViewModels.Event
{
    [Serializable]
    public class EventRequestPayload
    {
        public string Timestamp { get; set; }
        public string Severity { get; set; }
        public string SoftwareComponent { get; set; }
        public string Method { get; set; }
        public string User { get; set; }
        public string Data { get; set; }
    }
}