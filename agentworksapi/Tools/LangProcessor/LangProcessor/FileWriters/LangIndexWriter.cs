using AutoMapper;
using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace LangProcessor.FileWriters
{
    public class LangIndexWriter : ILangIndexWriter
    {
        private static ILog log = LogManager.GetLogger(typeof(LangIndexWriter));

        public void CreateIndex(string destinationDirectory, IEnumerable<LanguageInfo> languageInfo)
        {
            var languageMetadata = Mapper.Map<IEnumerable<LanguageInfo>, IEnumerable<LanguageMetadata>>(languageInfo);

            var serializedInfo = new SortedDictionary<string, LanguageMetadata>(languageMetadata.ToDictionary(x => x.LanguageCode));

            log.Info("Writing languages.json to file");
            serializedInfo.WriteToJsonFile(destinationDirectory, Constants.Languages + Constants.JsonFileExtension);
        }
    }
}
