using LangProcessor.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangProcessor
{
    public static class LangProcessorExtensions
    {
        public static void WriteToJsonFile(this object obj, string directory, string fileName)
        {
            var path = Path.Combine(directory, fileName);
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(path, json, System.Text.Encoding.UTF8);
        }

        public static string FirstLetterToUpper(this string str)
        {
            if (str == null || str.Length == 0)
            {
                return str;
            }

            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// Determines if the previous set of tranlsations is out of date
        /// </summary>
        /// <param name="previousTranslations">The set of translations read from the previous languages.json file</param>
        /// <param name="currentTranslations">The set of translations created from .source files</param>
        /// <returns>Returns true if the previous translations do not contain a translation, or if the version of any translation is out of date</returns>
        public static bool AreOutOfDate(this Dictionary<string, LanguageMetadata> previousTranslations, IEnumerable<LanguageMetadata> currentTranslations)
        {
            var isMissingKey = currentTranslations.Any(x => !previousTranslations.ContainsKey(x.LanguageCode));
            var isTranslationOutOfDate = !isMissingKey && currentTranslations.Any(x => previousTranslations[x.LanguageCode].Version.CompareTo(x.Version) < 0);
            var isMissingTranslation = previousTranslations.Keys.Any(x => !currentTranslations.Any(y => y.LanguageCode == x));

            return isMissingKey || isTranslationOutOfDate || isMissingTranslation;
        }

        public static bool IsOutOfDate(this LanguageMetadata previousTranslations, LanguageMetadata currentTranslations, string outputDir)
        {
            var fileMissing = !File.Exists(Path.Combine(outputDir, currentTranslations.LanguageCode + ".json"));
            var newTranslation = previousTranslations == null;
            var translationOutOfDate = !newTranslation && previousTranslations.Version.CompareTo(currentTranslations.Version) < 0;

            return fileMissing || newTranslation || translationOutOfDate;
        }
    }
}
