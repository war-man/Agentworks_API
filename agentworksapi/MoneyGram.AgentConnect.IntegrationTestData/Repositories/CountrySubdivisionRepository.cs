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
    ///     Gets available country subdivisions (states, provinces, etc) from json.
    /// </summary>
    public class CountrySubdivisionRepository : ITestAgentRepository, ITestAgentSearchable
    {
        private readonly string _configurationFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), "Data", "CountrySubdivisions.json");

        public CountrySubdivisionRepository()
        {
            var jsonData = JsonFileHelper.GetFileContents(_configurationFilePath);

            Subdivisions = !string.IsNullOrEmpty(jsonData)
                ? JsonProcessor.DeserializeObject<Dictionary<string, List<CountrySubdivisionInfo>>>(jsonData)
                    .FirstOrDefault().Value?
                    .ToDictionary(x => x.CountryCode, x => x.Subdivisions)
                : null;

            Subdivisions = Subdivisions ?? new Dictionary<string, List<SubdivisionInfo>>();
        }

        private Dictionary<string, List<SubdivisionInfo>> Subdivisions { get; }

        public T GetAllValues<T>() where T : class
        {
            return Subdivisions
                .Select(x => new CountrySubdivisionInfo
                {
                    CountryCode = x.Key,
                    Subdivisions = x.Value
                }).ToList() as T;
        }

        public T GetValue<T>(string key) where T : class
        {
            var value = Subdivisions[key];

            return value as T;
        }

        public bool ContainsKey(string key)
        {
            return Subdivisions.ContainsKey(key);
        }

        public void AddValue<T>(string key, T value)
        {
            Subdivisions.Add(key, value as List<SubdivisionInfo>);

            SaveConfiguration();
        }

        public void UpdateValue<T>(string key, T value)
        {
            Subdivisions[key] = value as List<SubdivisionInfo>;

            SaveConfiguration();
        }

        public void RemoveValue(string key)
        {
            Subdivisions.Remove(key);

            SaveConfiguration();
        }

        public T Search<T>(string searchTerm) where T : class
        {
            var countySubdivisionList = GetAllValues<List<CountrySubdivisionInfo>>();

            var countrySubdivisions =
                from countrySubdivision in countySubdivisionList
                where ContainsSearchTerm(countrySubdivision, searchTerm)
                select countrySubdivision;

            return countrySubdivisions.ToList() as T;
        }

        private void SaveConfiguration()
        {
            var countrySubdivisionInfos = new CountrySubdivisionInfoList
            {
                CountrySubdivisionInfos = Subdivisions
                    .Select(x => new CountrySubdivisionInfo
                    {
                        CountryCode = x.Key,
                        Subdivisions = x.Value
                    }).ToList()
            };

            var fileContents = JsonProcessor.SerializeObject(countrySubdivisionInfos, indented: true);

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
                if(propertyValue is List<SubdivisionInfo> && (propertyValue as List<SubdivisionInfo>).Any(value => ContainsSearchTerm(value, searchTerm)))
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