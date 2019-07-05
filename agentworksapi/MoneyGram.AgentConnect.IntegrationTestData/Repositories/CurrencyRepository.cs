using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.Common.Json;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    /// <summary>
    ///     Provides available currencies from json.
    /// </summary>
    public class CurrencyRepository : ITestAgentRepository, ITestAgentSearchable
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "Currencies.json");

        public CurrencyRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            Currencies = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<CurrencyInfo>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.CurrencyCode, x => x)
                : null;

            Currencies = Currencies ?? new Dictionary<string, CurrencyInfo>();
        }

        private Dictionary<string, CurrencyInfo> Currencies { get; }

        public T GetAllValues<T>() where T : class
        {
            return Currencies.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = Currencies[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return Currencies.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            Currencies.Add(key, value as CurrencyInfo);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            Currencies[key] = value as CurrencyInfo;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            Currencies.Remove(key);

            SaveConfiguration();
        }

        public T Search<T>(string searchTerm) where T : class
        {
            var currencyList = GetAllValues<List<CurrencyInfo>>();

            var matchedCurrencies =
                from currency in currencyList
                where ContainsSearchTerm(currency, searchTerm)
                select currency;

            return matchedCurrencies.ToList() as T;
        }

        private void SaveConfiguration()
        {
            var currencyInfos = new CurrencyInfoList
            {
                CurrencyInfos = Currencies.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(currencyInfos, indented: true);

            JsonFileHelper.SaveFileContents(_configurationFilePath, fileContents);
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