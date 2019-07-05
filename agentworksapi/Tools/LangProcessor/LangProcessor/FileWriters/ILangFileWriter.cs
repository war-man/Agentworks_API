using LangProcessor.Models;
using System.Collections.Generic;

namespace LangProcessor.FileWriters
{
    public interface ILangFileWriter
    {
        void CreateTranslationFiles(string destinationDirectory, IDictionary<string, LanguageMetadata> previousTranslations, IEnumerable<LanguageInfo> processedTranslations);
    }
}