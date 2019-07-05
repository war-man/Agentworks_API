using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LangProcessor.FileWriters
{
    public class MissingKeysWriter : IMissingKeysWriter
    {
        private static ILog log = LogManager.GetLogger(typeof(MissingKeysWriter));
        
        public void CreateConversionReport(string destinationDirectory, IEnumerable<LanguageInfo> languageInfo)
        {
            var baseLanguageInfo = languageInfo.First(x => x.LanguageCode == Constants.BaseLanguage);
            var requiredKeys = baseLanguageInfo.Strings.Keys;
            
            var missingTranslations = new ConcurrentDictionary<string, Dictionary<string, string>>();

            Parallel.ForEach(languageInfo, (language) =>
            {
                var missingKeys = requiredKeys.Except(language.Strings.Keys);

                if (missingKeys.Any())
                {
                    log.Info(language.LanguageCode + " is missing " + missingKeys.Count() + " key(s)");
                    var missingKeyDict = missingKeys.ToDictionary(x => x, x => baseLanguageInfo.Strings[x]);
                    missingTranslations.TryAdd(language.LanguageCode, missingKeyDict);
                }
            });

            this.AddMissingKeys(destinationDirectory, missingTranslations);
            var sortedMissingTranslations = new SortedDictionary<string, Dictionary<string, string>>(missingTranslations);

            log.Info("Writing missing keys to file: " + Path.Combine(destinationDirectory, Constants.MissingKeys + Constants.JsonFileExtension));

            sortedMissingTranslations.WriteToJsonFile(destinationDirectory, Constants.MissingKeys + Constants.JsonFileExtension);
            CreateMissingKeysSpreadsheets(destinationDirectory, sortedMissingTranslations);
        }

        private void AddMissingKeys(string destinationDirectory, IDictionary<string, Dictionary<string, string>> missingKeys)
        {
            Parallel.ForEach(missingKeys, (language) =>
            {
                var filePath = Path.Combine(destinationDirectory, language.Key + Constants.JsonFileExtension);
                var fileContents = File.ReadAllText(filePath);
                var translations = JsonConvert.DeserializeObject<LanguageInfo>(fileContents);

                foreach (var missingKey in language.Value)
                {
                    if (!translations.Strings.ContainsKey(missingKey.Key))
                    {
                        translations.Strings.Add(missingKey.Key, missingKey.Value);
                    }
                }

                translations.WriteToJsonFile(destinationDirectory, language.Key + Constants.JsonFileExtension);
            });
        }

        private void CreateMissingKeysSpreadsheets(string destinationDirectory, IDictionary<string, Dictionary<string, string>> missingKeys, string separator = "\t")
        {
            foreach (var language in missingKeys)
            {
                var fileName = Constants.MissingKeys + "_" + language.Key + Constants.TsvFileExtension;
                fileName = Path.Combine(destinationDirectory, fileName);
                File.WriteAllLines(fileName, language.Value.Select(x => x.Key + separator + x.Value));
            }
        }
    }
}
