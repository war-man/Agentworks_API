using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LangProcessor.FileWriters
{
    public class OutputOrchestrator : IOutputOrchestrator
    {
        private static ILog log = LogManager.GetLogger(typeof(OutputOrchestrator));

        private ILangFileWriter langFileWriter;
        private ILangIndexWriter langIndexWriter;
        private IMissingKeysWriter missingKeysWriter;

        public OutputOrchestrator(ILangFileWriter langFileWriter, ILangIndexWriter langIndexWriter, IMissingKeysWriter missingKeysWriter)
        {
            this.langFileWriter = langFileWriter;
            this.langIndexWriter = langIndexWriter;
            this.missingKeysWriter = missingKeysWriter;
        }

        public void CreateOutput(string sourceDirectory, string destinationDirectory, Dictionary<string, VersionedLanguage> langSourceMetadata, Dictionary<string, LanguageMetadata> previousTranslationMetadata, IEnumerable<LanguageInfo> processedTranslations)
        {
            if (previousTranslationMetadata.AreOutOfDate(processedTranslations) || Arguments.ForceOverwrite)
            {
                log.Info("Previous translations are out of date...overwriting");

                this.langFileWriter.CreateTranslationFiles(destinationDirectory, previousTranslationMetadata, processedTranslations);
                this.missingKeysWriter.CreateConversionReport(destinationDirectory, processedTranslations);
                this.langIndexWriter.CreateIndex(destinationDirectory, processedTranslations);
            }

            if (Arguments.ForceOverwrite)
            {
                log.Info("Updating version info in " + Constants.LangSourceMetadata);

                foreach (var language in langSourceMetadata)
                {
                    language.Value.Version = Process.GetCurrentProcess().StartTime;
                }

                langSourceMetadata.WriteToJsonFile(sourceDirectory, Constants.LangSourceMetadata + Constants.JsonFileExtension);
            }

            this.CleanDistDir(processedTranslations, destinationDirectory);
        }

        private void CleanDistDir(IEnumerable<LanguageMetadata> processedLanguages, string outputDir)
        {
            log.Info("Removing extraneous json files from " + outputDir);

            var fileWhiteList = new HashSet<string>(processedLanguages.Select(x => x.LanguageCode).Concat(Constants.DestMetadataFiles));

            foreach (var filePath in Directory.GetFiles(outputDir, "*" + Constants.JsonFileExtension))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                if (!fileWhiteList.Contains(fileName))
                {
                    log.Info("Removing " + fileName);
                    File.Delete(filePath);
                }
            }
        }
    }
}
