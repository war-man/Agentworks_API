using System;

namespace MoneyGram.PartnerService.DomainModel
{
    [Serializable]
    public class BaseServiceMessage
    {
        public Header header { get; set; }
    }

    [Serializable]
    public partial class Header
    {
        public SecurityHeader Security { get; set; }

        public RoutingContextHeader RoutingContext { get; set; }

        public ProcessingInstruction ProcessingInstruction { get; set; }

        public ClientHeader ClientHeader { get; set; }

    }

    [Serializable]
    public partial class SecurityHeader
    {
        public string UserID { get; set; }
    }

    [Serializable]
    public partial class RoutingContextHeader
    {
    }

    [Serializable]
    public partial class ProcessingInstruction
    {
        public string Action { get; set; }

        public InvocationMethodCode InvocationMethod { get; set; }

        public bool InvocationMethodFieldSpecified;

        public bool ReadOnlyFlag { get; set; }

        public bool ReadOnlyFlagFieldSpecified;

        public string FaultHandling { get; set; }

        public string Language { get; set; }

        public bool EchoRequestFlag { get; set; }

        public bool EchoRequestFlagFieldSpecified;

        public string SimulatedModeAction { get; set; }

        public bool ReturnErrorsAsException { get; set; }

        public bool ReturnErrorsAsExceptionFieldSpecified;

        public bool RollbackTransaction { get; set; }
    }

    [Serializable]
    public partial class ClientHeader
    {
        public string ClientRequestID { get; set; }

        public string ClientSessionID { get; set; }

    }

    [Serializable]
    public enum InvocationMethodCode
    {
        FORFUTUREUSE,
    }
}
