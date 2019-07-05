using System;

namespace MoneyGram.DLS.DomainModel
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

        public AgentHeader Agent { get; set; }
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

        public bool ReadOnlyFlag { get; set; }

        public string FaultHandling { get; set; }

        public string Language { get; set; }

        public bool EchoRequestFlag { get; set; }

        public string SimulatedModeAction { get; set; }

        public bool ReturnErrorsAsException { get; set; }

        public bool RollbackTransaction { get; set; }
    }

    [Serializable]
    public partial class ClientHeader
    {
        public string ClientRequestID { get; set; }

        public string ClientSessionID { get; set; }

        public string ClientName { get; set; }
    }

    [Serializable]
    public partial class AgentHeader
    {
        public string MainOfficeId { get; set; }

        public string LegacyMainOfficeId { get; set; }

        public string AgentId { get; set; }

        public string LegacyAgentId { get; set; }

    }

    [Serializable]
    public enum InvocationMethodCode
    {
        FORFUTUREUSE,
    }
}
