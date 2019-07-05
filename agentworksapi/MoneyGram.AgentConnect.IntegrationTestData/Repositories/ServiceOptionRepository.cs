using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    ///     Reads available service options from json.
    /// </summary>
    public class ServiceOptionRepository : ITestAgentRepository
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "ServiceOptions.json");

        public ServiceOptionRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            ServiceOptions = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<ServiceOption>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.Code, x => x)
                : null;

            ServiceOptions = ServiceOptions ?? new Dictionary<string, ServiceOption>();
        }

        private Dictionary<string, ServiceOption> ServiceOptions { get; }

        public T GetAllValues<T>() where T : class
        {
            return ServiceOptions.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = ServiceOptions[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return ServiceOptions.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            ServiceOptions.Add(key, value as ServiceOption);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            ServiceOptions[key] = value as ServiceOption;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            ServiceOptions.Remove(key);

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            var serviceOptionInfos = new ServiceOptionInfoList
            {
                ServiceOptionInfos = ServiceOptions.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(serviceOptionInfos, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }
    }
}