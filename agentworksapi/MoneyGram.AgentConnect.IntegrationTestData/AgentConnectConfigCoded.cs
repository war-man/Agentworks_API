using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.AgentConnect.IntegrationTest.Data
{
    /// <summary>
    /// Test implementation of IAgentConnectConfig that returns hardcoded values.
    /// </summary>
    public class AgentConnectConfigCoded : IAgentConnectConfig
    {
        public string ApiVersion => "1705";

        public string ClientSoftwareVersion => "1705";

        public string PoeType => "AGENTWORKS";

        public string ChannelType => "LOCATION";

        public string TargetAudience => "AGENT_FACING";

        public string AgentConnectUrl => EnvUrl();

        public IList<IEndpointBehavior> EndpoingBehaviors => null;

        public void DecorateRequest(Request acRequest)
        {
            acRequest.ClientSoftwareVersion = this.ClientSoftwareVersion;
            acRequest.PoeType = this.PoeType;
            acRequest.ChannelType = this.ChannelType;
            acRequest.TargetAudience = this.TargetAudience;
            acRequest.TimeStamp = DateTime.Now;
        }

        private string EnvUrl()
        {
            var testSettings = TestConfig.TestSettings;
            switch (testSettings.AcEnvironment)
            {
                case AcEnv.D1:
                    return "https://d1ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.D2:
                    return "https://d2ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.D3:
                    return "https://d3ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.D5:
                    return "https://d5ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.Q1:
                    return "https://q1ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.Q2:
                    return "https://q2ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.Q3:
                    return "https://q3ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.Q5:
                    return "https://q5ws.qa.moneygram.com/ac2/services/AgentConnect1705";
                case AcEnv.External:
                    return "https://extws.moneygram.com/extws/services/AgentConnect1705";
                default:
                    return "https://extws.moneygram.com/extws/services/AgentConnect1705";
            }
        }
    }
}