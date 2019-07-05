using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    /// Provides available amount ranges from json.
    /// </summary>
    public class AmountRangeRepository : ITestAgentRepository
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "AmountRanges.json");

        private Dictionary<string, AmountRange> AmountRanges { get; }

        public AmountRangeRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            AmountRanges = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<AmountRange>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.Code, x => x)
                : null;

            AmountRanges = AmountRanges ?? new Dictionary<string, AmountRange>();
        }

        public T GetAllValues<T>() where T : class
        {
            return AmountRanges.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = AmountRanges[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return AmountRanges.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            AmountRanges.Add(key, value as AmountRange);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            AmountRanges[key] = value as AmountRange;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            AmountRanges.Remove(key);

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            var amountRangeInfos = new AmountRangeInfoList
            {
                RangeInfos = AmountRanges.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(amountRangeInfos, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }
    }
}