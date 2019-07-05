using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LangProcessor.FileWriters
{
    public class LangFileWriter : ILangFileWriter
    {
        private static ILog log = LogManager.GetLogger(typeof(LangFileWriter));

        public void CreateTranslationFiles(string destinationDirectory, IDictionary<string, LanguageMetadata> previousTranslations, IEnumerable<LanguageInfo> processedTranslations)
        {
            Parallel.ForEach(processedTranslations, (translation) =>
            {
                var languageCode = translation.LanguageCode;

                LanguageMetadata previousTranslationMetadata = null;
                previousTranslations.TryGetValue(languageCode, out previousTranslationMetadata);

                if (Arguments.ForceOverwrite || previousTranslationMetadata.IsOutOfDate(translation, destinationDirectory))
                {
                    log.Info("Writing locale " + languageCode + " to file: " + Path.Combine(destinationDirectory, languageCode + Constants.JsonFileExtension));
                    translation.WriteToJsonFile(destinationDirectory, languageCode + Constants.JsonFileExtension);
                }
            });
        }
    }
}
