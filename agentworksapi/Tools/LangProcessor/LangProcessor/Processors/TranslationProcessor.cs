using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LangProcessor.Processors
{
    public class TranslationProcessor : ITranslationProcessor
    {
        private static ILog log = LogManager.GetLogger(typeof(TranslationProcessor));

        public LanguageInfo ProcessTranslation(string sourceDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, string locale, IEnumerable<KeyValuePair<string, string>> rawTranslations)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(locale);
            var version = langSourceMetadata.ContainsKey(locale) ? langSourceMetadata[locale].Version : File.GetLastWriteTime(Path.Combine(sourceDirectory, cultureInfo + Constants.JsonFileExtension));

            var distinctTranslations = rawTranslations.GroupBy(x => x.Key).Select(group => group.First());
            var translationDictionary = distinctTranslations.ToDictionary(x => x.Key, x => x.Value);

            var languageInfo = new LanguageInfo
            {
                Version = Arguments.ForceOverwrite ? Process.GetCurrentProcess().StartTime : version,
                LanguageCode = locale,
                Language = cultureInfo.DisplayName.FirstLetterToUpper(),
                LocalizedLanguageName = cultureInfo.NativeName.FirstLetterToUpper(),
                Strings = new SortedDictionary<string, string>(translationDictionary)
            };

            log.Info("Processed " + languageInfo.Strings.Count() + " translations for locale: " + locale);

            return languageInfo;
        }

        public IEnumerable<LanguageInfo> ProcessTranslations(string sourceDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, IDictionary<string, IEnumerable<KeyValuePair<string, string>>> rawTranslations)
        {
            var processedTranslations = new List<LanguageInfo>();

            foreach (var locale in rawTranslations)
            {
                processedTranslations.Add(this.ProcessTranslation(sourceDirectory, langSourceMetadata, locale.Key, locale.Value));
            }

            return processedTranslations;
        }
    }
}
