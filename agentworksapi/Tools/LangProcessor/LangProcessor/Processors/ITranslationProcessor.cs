using LangProcessor.Models;
using System.Collections.Generic;

namespace LangProcessor.Processors
{
    public interface ITranslationProcessor
    {
        LanguageInfo ProcessTranslation(string sourceDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, string locale, IEnumerable<KeyValuePair<string, string>> rawTranslations);

        IEnumerable<LanguageInfo> ProcessTranslations(string sourceDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, IDictionary<string, IEnumerable<KeyValuePair<string, string>>> rawTranslations);
    }
}
