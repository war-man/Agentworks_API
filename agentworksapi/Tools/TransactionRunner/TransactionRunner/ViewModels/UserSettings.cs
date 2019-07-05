using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TransactionRunner.ImportExport;
using TransactionRunner.ViewModels.Settings;

namespace TransactionRunner.ViewModels
{
    public class UserSettings
    {
        public UserSettings()
        {
            SendSettings = new SendSettings();
            StagedSendSettings = new SendSettings();
            SendReversalSettings = new SendReversalSettings();
            ReceiveSettings = new ReceiveSettings();
            StagedReceiveSettings = new ReceiveSettings();
            BillPaySettings = new BillPaySettings();
            StagedBillPaySettings = new BillPaySettings();
            EnvironmentAgents = new Dictionary<string, List<EnvironmentAgent>>();
        }

        // agent selector settings
        public string Environment { get; set; }

        public string AgentLocation { get; set; }
        public string AgentId { get; set; }

        public string AgentPosId { get; set; }

        // send parameters settings
        public SendSettings SendSettings { get; set; }

        public SendSettings StagedSendSettings { get; set; }

        public ReceiveSettings ReceiveSettings { get; set; }

        public ReceiveSettings StagedReceiveSettings { get; set; }

        public BillPaySettings BillPaySettings { get; set; }

        public BillPaySettings StagedBillPaySettings { get; set; }

        // send reversal parameters
        public SendReversalSettings SendReversalSettings { get; set; }

        public SessionType TransactionType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExportFileFormat FileFormat { get; set; }

        public string LastUsedDirectory { get; set; }

        public Dictionary<string, List<EnvironmentAgent>> EnvironmentAgents { get; set; }

        [JsonIgnore]
        public bool HasAgents => EnvironmentAgents?.Count > 0;

        public bool ContainsEnvironment(string environment)
        {
            return EnvironmentAgents.ContainsKey(environment);
        }

        public bool ContainsAgent(string environment, string agentId, string agentSequence)
        {
            return EnvironmentAgents.ContainsKey(environment)
                   && EnvironmentAgents[environment].Any(x => x.AgentId == agentId && x.AgentSequence == agentSequence);
        }

        public void AddAgent(EnvironmentAgent agent)
        {
            if(EnvironmentAgents.ContainsKey(agent.Environment))
            {
                EnvironmentAgents[agent.Environment].Add(agent);
            }
            else
            {
                EnvironmentAgents.Add(agent.Environment, new List<EnvironmentAgent> {agent});
            }
        }

        public List<EnvironmentAgent> GetAgents(string environment)
        {
            return ContainsEnvironment(environment) ? EnvironmentAgents[environment] : null;
        }

        public EnvironmentAgent GetAgent(string environment, string agentId, string agentSequence)
        {
            if(!ContainsAgent(environment, agentId, agentSequence))
            {
                return null;
            }

            var agent = EnvironmentAgents[environment]
                .First(x => x.AgentId == agentId && x.AgentSequence == agentSequence);
            agent.Environment = environment;

            return agent;
        }

        public void UpdateAgent(EnvironmentAgent oldAgent, EnvironmentAgent newAgent)
        {
            var agent = GetAgent(oldAgent.Environment, oldAgent.AgentId, oldAgent.AgentSequence);

            if(agent == null)
            {
                return;
            }

            agent.Environment = newAgent.Environment;
            agent.AgentCountryIsoCode = newAgent.AgentCountryIsoCode;
            agent.AgentCountry = newAgent.AgentCountry;
            agent.AgentStateCode = newAgent.AgentStateCode;
            agent.AgentState = newAgent.AgentState;
            agent.AgentId = newAgent.AgentId;
            agent.AgentSequence = newAgent.AgentSequence;
            agent.AgentPassword = newAgent.AgentPassword;
            agent.Language = newAgent.Language;
            agent.SendCurrencies = newAgent.SendCurrencies;
        }

        public void DeleteAgent(string environment, string agentId, string agentSequence)
        {
            EnvironmentAgents[environment].RemoveAll(x => x.AgentId == agentId && x.AgentSequence == agentSequence);

            if(EnvironmentAgents[environment].Count == 0)
            {
                EnvironmentAgents.Remove(environment);
            }
        }
    }
}