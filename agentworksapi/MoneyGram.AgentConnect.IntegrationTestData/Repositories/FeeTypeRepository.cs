using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    ///     Provides available item choices from json.
    /// </summary>
    public class FeeTypeRepository : ITestAgentRepository
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "FeeTypes.json");

        public FeeTypeRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            FeeTypes = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<FeeType>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.Code, x => x)
                : null;

            FeeTypes = FeeTypes ?? new Dictionary<string, FeeType>();
        }

        private Dictionary<string, FeeType> FeeTypes { get; }

        public T GetAllValues<T>() where T : class
        {
            return FeeTypes.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = FeeTypes[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return FeeTypes.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            FeeTypes.Add(key, value as FeeType);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            FeeTypes[key] = value as FeeType;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            FeeTypes.Remove(key);

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            var feeTypeInfos = new FeeTypeInfoList
            {
                ChoiceInfos = FeeTypes.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(feeTypeInfos, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }
    }
}