using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    ///     Provides available environment agents from json.
    /// </summary>
    public class EnvironmentAgentRepository : ITestAgentRepository, ITestAgentSearchable
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "EnvironmentAgents.json");

        public EnvironmentAgentRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            Identities = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<EnvironmentAgent>>>(jsonData)
                : null;

            Identities = Identities ?? new Dictionary<string, List<EnvironmentAgent>>();
        }

        private Dictionary<string, List<EnvironmentAgent>> Identities { get; }

        public T GetAllValues<T>() where T : class
        {
            return Identities
                .Select(x => new EnvironmentAgentInfoList
                {
                    Environment = x.Key,
                    Agents = x.Value
                }).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = Identities[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return Identities.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            Identities.Add(key, value as List<EnvironmentAgent>);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            Identities[key] = value as List<EnvironmentAgent>;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            Identities.Remove(key);

            SaveConfiguration();
        }

        public T Search<T>(string searchTerm) where T : class
        {
            var agentList = GetAgents();

            var agents =
                from agent in agentList
                where ContainsSearchTerm(agent, searchTerm)
                select agent;

            return agents.ToList() as T;
        }

        private void SaveConfiguration()
        {
            var fileContents = JsonProcessor.SerializeObject(Identities, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }

        private List<EnvironmentAgent> GetAgents()
        {
            var agents = new List<EnvironmentAgent>();

            var environments =
                from identity in Identities
                select new EnvironmentAgentInfoList
                {
                    Environment = identity.Key,
                    Agents = identity.Value
                };

            foreach(var environment in environments)
            {
                environment.Agents.ForEach(x => x.Environment = environment.Environment);
                agents.AddRange(environment.Agents);
            }

            return agents;
        }

        private bool ContainsSearchTerm<T>(T obj, string searchTerm)
        {
            foreach(var propertyInfo in obj.GetType().GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(obj, null);

                if(propertyValue is string && propertyValue.ToString().IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return true;
                }
                if(!(propertyValue is List<string>))
                {
                    continue;
                }
                if((propertyValue as List<string>).Any(value => value.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    return true;
                }
            }
            return false;
        }
    }
}