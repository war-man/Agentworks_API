using AutoMapper;
using LangProcessor.FileParsers;
using LangProcessor.FileWriters;
using LangProcessor.Models;
using LangProcessor.Processors;
using Microsoft.Practices.Unity;
using MoneyGram.Common.Diagnostics;
using MoneyGram.Common.Diagnostics.Ioc;
using System;

namespace LangProcessor
{
    public class Program
    {
        private static ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            try
            {
                Arguments.SetArguments(args);
                Arguments.LogArguments();

                using (var container = new UnityContainer())
                {
                    RegisterDependencies(container);
                    ConfigureAutoMapper();

                    container.Resolve<ILangProcessor>().Run();
                }
            }
            catch(Exception e)
            {
                log.Error("=== ERROR ===\n");
                log.Error(e.ToString());
                throw e;
            }
        }

        private static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<ILangProcessor, Processors.LangProcessor>();
            container.RegisterType<ISourceDeserializer, TabDelimitedSourceDeserializer>();
            container.RegisterType<ILangFileWriter, LangFileWriter>();
            container.RegisterType<ILangIndexWriter, LangIndexWriter>();
            container.RegisterType<IMissingKeysWriter, MissingKeysWriter>();
            container.RegisterType<ITranslationProcessor, TranslationProcessor>();
            container.RegisterType<IDuplicateKeysWriter, DuplicateKeysWriter>();
            container.RegisterType<IOutputOrchestrator, OutputOrchestrator>();
            
//            log.LogUnityMappings(container);
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LanguageInfo, LanguageMetadata>();
                cfg.CreateMap<LanguageMetadata, LanguageInfo>()
                    .ForMember(info => info.Strings, opt => opt.Ignore());
            });
            
            Mapper.AssertConfigurationIsValid();
        }
    }
}