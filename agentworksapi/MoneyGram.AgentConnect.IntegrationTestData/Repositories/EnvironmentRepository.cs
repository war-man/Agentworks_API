using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    ///     Provides available environments from json.
    /// </summary>
    public class EnvironmentRepository : ITestAgentRepository
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "Environments.json");

        public EnvironmentRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            Environments = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<AcEnvironment>(jsonData)?.Environments
                    .ToDictionary(x => x, x => x)
                : null;

            Environments = Environments ?? new Dictionary<string, string>();
        }

        private Dictionary<string, string> Environments { get; }

        public T GetAllValues<T>() where T : class
        {
            return Environments.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            return Environments[key] as T;
        }

        public bool ContainsKey(string key)
        {
            return Environments.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            Environments.Add(key, value as string);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            Environments[key] = value as string;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            Environments.Remove(key);

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            var acEnvironment = new AcEnvironment
            {
                Environments = Environments.Select(x => x.Value).ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(acEnvironment, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }
    }
}