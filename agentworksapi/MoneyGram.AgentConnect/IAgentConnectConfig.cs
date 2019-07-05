using System.Collections.Generic;
using System.ServiceModel.Description;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect
{
    /// <summary>
    /// Defines dependency injected configuration interface for configuration driven AgentConnect settings.
    /// </summary>
    public interface IAgentConnectConfig
    {
        string ApiVersion { get; }
        string ClientSoftwareVersion { get; }
        string AgentConnectUrl { get; }
        string PoeType { get; }
        string ChannelType { get; }
        string TargetAudience { get; }
        IList<IEndpointBehavior> EndpoingBehaviors { get; }
        void DecorateRequest(Request acRequest);
    }
}