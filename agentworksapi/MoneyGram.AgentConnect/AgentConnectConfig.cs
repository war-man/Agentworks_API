using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect
{
    /// <summary>
    /// Implementation IAgentConnectConfig that reads from the ConfigurationManager (app.config).
    /// </summary>
    public class AgentConnectConfig : IAgentConnectConfig
    {
        public string ApiVersion { get; set; }
        public string ClientSoftwareVersion { get; set; }
        public string AgentConnectUrl { get; set; }
        public string PoeType { get; set; }
        public string ChannelType { get; set; }
        public string TargetAudience { get; set; }
        public IList<IEndpointBehavior> EndpoingBehaviors { get; set; }

        public void DecorateRequest(Request acRequest)
        {
            acRequest.ClientSoftwareVersion = this.ClientSoftwareVersion;
            acRequest.PoeType = this.PoeType;
            acRequest.ChannelType = this.ChannelType;
            acRequest.TargetAudience = this.TargetAudience;
            acRequest.TimeStamp = DateTime.Now;
        }
    }
}