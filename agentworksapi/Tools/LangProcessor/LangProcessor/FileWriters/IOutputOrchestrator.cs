using LangProcessor.Models;
using System.Collections.Generic;

namespace LangProcessor.FileWriters
{
    public interface IOutputOrchestrator
    {
        void CreateOutput(string sourceDirectory, string destinationDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, Dictionary<string, LanguageMetadata> previousTranslationMetadata, IEnumerable<LanguageInfo> processedTranslations);
    }
}