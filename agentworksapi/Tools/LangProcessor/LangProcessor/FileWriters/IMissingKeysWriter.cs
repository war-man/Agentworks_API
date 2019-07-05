using LangProcessor.Models;
using System.Collections.Generic;

namespace LangProcessor.FileWriters
{
    public interface IMissingKeysWriter
    {
        void CreateConversionReport(string destinationDirectory, IEnumerable<LanguageInfo> languageInfo);
    }
}