using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    public class RefundReasonRepository : ITestAgentRepository
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "RefundReasons.json");

        public RefundReasonRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            RefundReasons = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<EnumeratedIdentifierInfo>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.Identifier, x => x)
                : null;

            RefundReasons = RefundReasons ?? new Dictionary<string, EnumeratedIdentifierInfo>();
        }

        private Dictionary<string, EnumeratedIdentifierInfo> RefundReasons { get; }

        public T GetAllValues<T>() where T : class
        {
            return RefundReasons.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = RefundReasons[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return RefundReasons.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            RefundReasons.Add(key, value as EnumeratedIdentifierInfo);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            RefundReasons[key] = value as EnumeratedIdentifierInfo;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            RefundReasons.Remove(key);

            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            var refundReasonInfos = new RefundReasonInfoList
            {
                RefundReasonInfos = RefundReasons.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(refundReasonInfos, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
        }
    }
}