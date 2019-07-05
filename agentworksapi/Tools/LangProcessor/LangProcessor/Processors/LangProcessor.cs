using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangProcessor.FileParsers;
using LangProcessor.FileWriters;
using LangProcessor.Models;
using MoneyGram.Common.Diagnostics;
using Newtonsoft.Json;

namespace LangProcessor.Processors
{
    public class LangProcessor : ILangProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (LangProcessor));
        private readonly IDuplicateKeysWriter duplicateKeysWriter;

        private readonly ISourceDeserializer fileParser;
        private readonly IOutputOrchestrator outputOrchestrator;
        private readonly ITranslationProcessor translationProcessor;

        public LangProcessor(ISourceDeserializer fileParser, ITranslationProcessor translationProcessor,
            IDuplicateKeysWriter duplicateKeysWriter, IOutputOrchestrator outputOrchestrator)
        {
            this.fileParser = fileParser;
            this.translationProcessor = translationProcessor;
            this.duplicateKeysWriter = duplicateKeysWriter;
            this.outputOrchestrator = outputOrchestrator;
        }

        public void Run()
        {
            if (Arguments.HelpRequired)
            {
                Console.WriteLine(Resource.HelpFile);
                return;
            }

            if (Arguments.SingleApplicationMode)
            {
                ProcessDirectory(Arguments.SourceDirectory, Arguments.DestinationDirectory);
            }
            else
            {
                foreach (var appDirectory in Directory.GetDirectories(Arguments.SourceDirectory))
                {
                    var appName = Path.GetFileName(appDirectory);

                    var sourceDirectory = Path.Combine(appDirectory, Constants.DefaultSourceDirectory);
                    var destinationDirectory =
                        Path.Combine(Arguments.DestinationDirectory, appName ?? "",
                            Constants.DefaultDestinationDirectory);

                    ProcessDirectory(sourceDirectory, destinationDirectory);
                }
            }
        }

        private void ProcessDirectory(string sourceDirectory, string destinationDirectory)
        {
            var langSourceMetadata =
                File.Exists(Path.Combine(sourceDirectory, Constants.LangSourceMetadata + Constants.JsonFileExtension))
                    ? JsonConvert.DeserializeObject<Dictionary<string, VersionedLanguage>>(
                        File.ReadAllText(Path.Combine(sourceDirectory,
                            Constants.LangSourceMetadata + Constants.JsonFileExtension)))
                    : new Dictionary<string, VersionedLanguage>();

            var previousTranslationMetadata =
                File.Exists(Path.Combine(destinationDirectory, Constants.Languages + Constants.JsonFileExtension))
                    ? JsonConvert.DeserializeObject<Dictionary<string, LanguageMetadata>>(
                        File.ReadAllText(Path.Combine(destinationDirectory,
                            Constants.Languages + Constants.JsonFileExtension)))
                    : new Dictionary<string, LanguageMetadata>();

            var sourceFiles = Directory.GetFiles(sourceDirectory)
                .Where(path => Constants.SourceFileRegex.IsMatch(path))
                .Where(path => langSourceMetadata.ContainsKey(Constants.SourceFileRegex.Match(path).Groups[1].Value))
                .ToList();

            var rawTranslations = fileParser.Deserialize(sourceFiles);

            var processedTranslations = translationProcessor.ProcessTranslations(sourceDirectory, langSourceMetadata,
                rawTranslations);

            var duplicatesExist = duplicateKeysWriter.CreateDuplicateKeysReport(destinationDirectory, rawTranslations);

            if (duplicatesExist)
            {
                var errorMsg = "Duplicate keys exists...Please inspect duplicateKeys.json for more details";
                log.Error(errorMsg);
                throw new Exception(errorMsg);
            }

            outputOrchestrator.CreateOutput(sourceDirectory, destinationDirectory, langSourceMetadata,
                previousTranslationMetadata, processedTranslations);
        }
    }
}