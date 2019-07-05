using LangProcessor.Models;
using System.Collections.Generic;

namespace LangProcessor.FileWriters
{
    public interface ILangIndexWriter
    {
        void CreateIndex(string destinationDirectory, IEnumerable<LanguageInfo> languageInfo);
    }
}