using System;
using System.Collections.Generic;

namespace AwApi.ViewModels.Event
{
    [Serializable]
    public class EventRequest
    {
        public EventActionType ActionType { get; set; }
        public string Source { get; set; }
        public List<EventRequestPayload> Payload { get; set; }
    }
}