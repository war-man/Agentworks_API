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
    ///     Provides list of countries from json.
    /// </summary>
    public class CountryRepository : ITestAgentRepository, ITestAgentSearchable
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "Countries.json");

        public CountryRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            Countries = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<CountryInfo>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.CountryCode, x => x)
                : null;

            Countries = Countries ?? new Dictionary<string, CountryInfo>();
        }

        private Dictionary<string, CountryInfo> Countries { get; }

        public T GetAllValues<T>() where T : class
        {
            return Countries.Select(x => x.Value).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = Countries[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return Countries.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            Countries.Add(key, value as CountryInfo);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            Countries[key] = value as CountryInfo;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            Countries.Remove(key);

            SaveConfiguration();
        }

        public T Search<T>(string searchTerm) where T : class
        {
            var countryList = GetAllValues<List<CountryInfo>>();

            var countries =
                from country in countryList
                where ContainsSearchTerm(country, searchTerm)
                select country;

            return countries.ToList() as T;
        }

        private void SaveConfiguration()
        {
            var countryInfos = new CountryInfoList
            {
                CountryInfos = Countries.Values.ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(countryInfos, indented: true);

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