using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangProcessor.FileWriters
{
    public class DuplicateKeysWriter : IDuplicateKeysWriter
    {
        private static ILog log = LogManager.GetLogger(typeof(DuplicateKeysWriter));

        public bool CreateDuplicateKeysReport(string destinationDirectory, IDictionary<string, IEnumerable<KeyValuePair<string, string>>> rawTranslations)
        {
            var duplicateKeys = new Dictionary<string, IEnumerable<KeyValuePair<string, string>>>();

            foreach (var locale in rawTranslations)
            {
                var duplicateTranslations = locale.Value.GroupBy(x => x.Key)
                                                        .Where(x => x.Count() > 1)
                                                        .SelectMany(x => x)
                                                        .ToList();

                if (duplicateTranslations.Any())
                {
                    log.Error(duplicateTranslations.Count() + " duplicate keys found for locale: " + locale.Key);
                    duplicateKeys.Add(locale.Key, duplicateTranslations);
                }
            }

            if(duplicateKeys.Any())
            {
                duplicateKeys.WriteToJsonFile(destinationDirectory, Constants.DuplicateKeys + Constants.JsonFileExtension);
                return true;
            }
            else
            {
                File.Delete(Path.Combine(destinationDirectory, Constants.DuplicateKeys + Constants.JsonFileExtension));
                return false;
            }
        }
    }
}