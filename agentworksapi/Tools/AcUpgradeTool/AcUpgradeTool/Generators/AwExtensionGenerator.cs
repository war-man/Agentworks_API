using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AcUpgradeTool.Generators
{
    public class AwExtensionGenerator : IGenerator
    {
        private static readonly string AwMapperTemplate = Resources.AwMapperTemplate;
        private static readonly string ModelExtensionTemplate = Resources.AwModelExtensionTemplate;
        private static readonly string ViewModelExtensionTemplate = Resources.AwViewModelExtensionTemplate;

        private static readonly string OutputDir = Path.Combine("output", "AwApi.EntityMapper");
        private static readonly string ModelExtensionDir = Path.Combine("output", "AwApi.EntityMapper", "ModelExtensions");
        private static readonly string ViewModelExtensionDir = Path.Combine("output", "AwApi.EntityMapper", "ViewModelExtensions");

        private static readonly string DomainModelNamespace = "MoneyGram.AgentConnect.DomainModel.Transaction";
        private static readonly string ViewModelNamespace = "AwApi.ViewModels.Transaction";
        private static readonly string ModelExtensionNamespace = "AwApi.EntityMapper.ModelExtensions";
        private static readonly string ViewModelExtensionNamespace = "AwApi.EntityMapper.ViewModelExtensions";

        public void Generate(List<Type> types)
        {
            CreateOutputDirectories();

            var typesToProcess =
                types.Where(
                    type =>
                        !type.GetMethods().Any(x => x.DeclaringType == type) && 
                        !type.IsInterface)
                    .ToList();

            foreach (var type in typesToProcess)
            {
                CreateModelExtension(type);
                CreateViewModelExtension(type);
            }

            CreateAwMapper(typesToProcess);
        }

        public void LogStart()
        {
            Console.Write("\nStarted AgentWorks Extension Generation...");
        }

        public void LogComplete()
        {
            Console.Write("Complete");
        }

        private void CreateOutputDirectories()
        {
            if (Directory.Exists(OutputDir))
            {
                Directory.Delete(OutputDir, true);
            }

            Directory.CreateDirectory(OutputDir);
            Directory.CreateDirectory(ViewModelExtensionDir);
            Directory.CreateDirectory(ModelExtensionDir);
        }

        private void CreateModelExtension(Type type)
        {
            var domainNamespace = CreateNamespace(type, DomainModelNamespace);
            var vmNamespace = CreateNamespace(type, ViewModelNamespace);

            var mapExtensions = HasBaseType(type) ? 
                $"{Environment.NewLine}{GetTab(4)}.IncludeBase<DOMAIN.{type.BaseType.Name}, VM.{type.BaseType.Name}>()" : 
                string.Empty;

            var replacements = new Dictionary<string, string>
            {
                {"DomainNamespace", domainNamespace},
                {"VMNamespace", vmNamespace},
                {"Namespace", ModelExtensionNamespace},
                {"ModelType", type.Name},
                {"MapExtensions", mapExtensions},
            };

            var fileText = ModelExtensionTemplate.Format(replacements);
            var filePath = Path.Combine(ModelExtensionDir, $"{type.Name}Extensions.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }
        private void CreateViewModelExtension(Type type)
        {
            var domainNamespace = CreateNamespace(type, DomainModelNamespace);
            var vmNamespace = CreateNamespace(type, ViewModelNamespace);

            var mapExtensions = HasBaseType(type) ?
                $"{Environment.NewLine}{GetTab(4)}.IncludeBase<VM.{type.BaseType.Name}, DOMAIN.{type.BaseType.Name}>()" :
                string.Empty;

            var replacements = new Dictionary<string, string>
            {
                {"DomainNamespace", domainNamespace},
                {"VMNamespace", vmNamespace},
                {"Namespace", ViewModelExtensionNamespace},
                {"ModelType", type.Name},
                {"MapExtensions", mapExtensions},
            };

            var fileText = ViewModelExtensionTemplate.Format(replacements);
            var filePath = Path.Combine(ViewModelExtensionDir, $"{type.Name}Extensions.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }

        private void CreateAwMapper(List<Type> types)
        {
            var modelExtensions = types.Select(x => $"ModelExtensions.{x.Name}Extensions.DefineMappings();").ToList();
            var viewModelExtensions = types.Select(x => $"ViewModelExtensions.{x.Name}Extensions.DefineMappings();").ToList();

            var modelExtensionsString = string.Join($"{Environment.NewLine}{GetTab(3)}", modelExtensions);
            var viewModelExtensionsString = string.Join($"{Environment.NewLine}{GetTab(3)}", viewModelExtensions);

            var replacements = new Dictionary<string, string>
            {
                {"ModelExtensions", modelExtensionsString},
                {"ViewModelExtensions", viewModelExtensionsString}
            };

            var fileText = AwMapperTemplate.Format(replacements);
            var filePath = Path.Combine(OutputDir, "AwAgentConnectMapper.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }

        private string CreateNamespace(Type type, string rootNamespace)
        {
            if (type.IsRequest())
            {
                return $"{rootNamespace}.Request";
            }

            if (type.IsResponse())
            {
                return $"{rootNamespace}.Response";
            }

            return rootNamespace;;
        }

        private bool HasBaseType(Type type)
        {
            return !type?.BaseType?.FullName?.Contains("System.") ?? false;
        }

        private static string GetTab(int numTabs)
        {
            return string.Concat(Enumerable.Repeat("    ", numTabs));
        }
    }
}