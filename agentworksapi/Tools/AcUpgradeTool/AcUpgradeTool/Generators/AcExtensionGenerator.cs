using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AcUpgradeTool.Generators
{
    public class AcExtensionGenerator : IGenerator
    {
        private static readonly string AcMapperTemplate = Resources.AcMapperTemplate;
        private static readonly string DomainModelExtensionTemplate = Resources.AcDomainModelExtensionTemplate;
        private static readonly string DomainModelRequestExtensionTemplate = Resources.AcDomainModelRequestExtensionTemplate;
        private static readonly string ServiceModelExtensionTemplate = Resources.AcServiceModelExtensionTemplate;

        private static readonly string OutputDir = Path.Combine("output", "MoneyGram.AgentConnect.Repository",
            "EntityMapper");

        private static readonly string DomainModelExtensionDir = Path.Combine("output",
            "MoneyGram.AgentConnect.Repository", "EntityMapper", "DomainModelExtensions");

        private static readonly string ServiceModelExtensionDir = Path.Combine("output",
            "MoneyGram.AgentConnect.Repository", "EntityMapper", "ServiceModelExtensions");

        private static readonly string DomainModelNamespace = "MoneyGram.AgentConnect.DomainModel.Transaction";

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
                CreateDomainModelExtension(type);
                CreateServiceModelExtension(type);
            }

            CreateAcMapper(typesToProcess);
        }

        public void LogStart()
        {
            Console.Write("\nStarted AgentConnect Extension Generation...");
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
            Directory.CreateDirectory(DomainModelExtensionDir);
            Directory.CreateDirectory(ServiceModelExtensionDir);
        }

        private void CreateDomainModelExtension(Type type)
        {
            var domainNamespace = CreateNamespace(type, DomainModelNamespace);

            var mapExtensions = DomainToServiceMapExtensions(type);

            var replacements = new Dictionary<string, string>
            {
                {"DomainNamespace", domainNamespace},
                {"ModelType", type.Name},
                {"CreateMapExtensions", mapExtensions}
            };

            var template = type.IsRequest() && !type.IsEnum ? DomainModelRequestExtensionTemplate : DomainModelExtensionTemplate;
            var fileText = template.Format(replacements);
            var filePath = Path.Combine(DomainModelExtensionDir, $"{type.Name}Extensions.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }

        private void CreateServiceModelExtension(Type type)
        {
            var domainNamespace = CreateNamespace(type, DomainModelNamespace);

            var mapExtensions = ServiceToDomainMapExtensions(type);

            var replacements = new Dictionary<string, string>
            {
                {"DomainNamespace", domainNamespace},
                {"ModelType", type.Name},
                {"CreateMapExtensions", mapExtensions}
            };

            var fileText = ServiceModelExtensionTemplate.Format(replacements);
            var filePath = Path.Combine(ServiceModelExtensionDir, $"{type.Name}Extensions.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }

        private void CreateAcMapper(List<Type> types)
        {
            var domainModelExtensions =
                types.Select(x => $"DomainModelExtensions.{x.Name}Extensions.DefineMappings();").ToList();
            var serviceModelExtensions =
                types.Select(x => $"ServiceModelExtensions.{x.Name}Extensions.DefineMappings();").ToList();

            var domainModelExtensionsString = string.Join($"{Environment.NewLine}{GetTab(3)}", domainModelExtensions);
            var serviceModelExtensionsString = string.Join($"{Environment.NewLine}{GetTab(3)}", serviceModelExtensions);

            var replacements = new Dictionary<string, string>
            {
                {"DomainModelExtensions", domainModelExtensionsString},
                {"ServiceModelExtensions", serviceModelExtensionsString}
            };

            var fileText = AcMapperTemplate.Format(replacements);
            var filePath = Path.Combine(OutputDir, "AcMapper.cs");

            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }

        private string DomainToServiceMapExtensions(Type type)
        {
            if (type.IsEnum)
            {
                return string.Empty;
            }

            var listConversions = new List<string>
            {
                string.Empty
            };

            var fields =
                type.GetMembers()
                    .Where(x => x.MemberType == MemberTypes.Field && x.DeclaringType == type)
                    .Cast<FieldInfo>()
                    .ToList();

            foreach (var fieldInfo in fields.Where(fi => fi.FieldType.IsArray))
            {
                listConversions.Add(
                    $".ForMember(srv => srv.{fieldInfo.Name}, opt => opt.MapFrom(x => x.{fieldInfo.Name.Capitalize()}.ListToArray()))");
            }

            foreach (var fieldInfo in fields.Where(fi => fi.Name.ToLower().EndsWith("specified")))
            {
                var fieldName = fieldInfo.Name.Substring(0, fieldInfo.Name.LastIndexOf("Specified"));

                listConversions.Add(
                    $".ForMember(srv => srv.{fieldInfo.Name}, opt => opt.MapFrom(x => x.{fieldName.Capitalize()}.HasValue))");
            }

            if (HasBaseType(type))
            {
                listConversions.Add($".IncludeBase<DOMAIN.{type.BaseType.Name}, SERVICE.{type.BaseType.Name}>()");
            }

            return string.Join($"{Environment.NewLine}{GetTab(4)}", listConversions);
        }

        private string ServiceToDomainMapExtensions(Type type)
        {
            if (type.IsEnum)
            {
                return string.Empty;
            }

            var listConversions = new List<string>
            {
                string.Empty
            };

            var fields =
                type.GetMembers()
                    .Where(x => x.MemberType == MemberTypes.Field && x.DeclaringType == type)
                    .Cast<FieldInfo>()
                    .ToList();

            foreach (var fieldInfo in fields.Where(fi => fi.FieldType.IsArray))
            {
                listConversions.Add(
                    $".ForMember(dom => dom.{fieldInfo.Name.Capitalize()}, opt => opt.MapFrom(x => x.{fieldInfo.Name}.ArrayToList()))");
            }

            foreach (var fieldInfo in fields.Where(fi => fi.Name.ToLower().EndsWith("specified")))
            {
                var fieldName = fieldInfo.Name.Substring(0, fieldInfo.Name.LastIndexOf("Specified"));
                var fieldType = fields.First(x => x.Name.ToLower() == fieldName.ToLower()).FieldType;

                var fieldTypeName = fieldType.Namespace.ToLower().Contains("system") ? 
                    fieldType.Namespace + "." + fieldType.Name :
                    "SERVICE." + fieldType.Name;

                listConversions.Add(
                    $".ForMember(dom => dom.{fieldName.Capitalize()}, opt => opt.MapFrom(x => x.{fieldInfo.Name} ? x.{fieldName} : ({fieldTypeName}?) null))");
            }

            if (HasBaseType(type))
            {
                listConversions.Add($".IncludeBase<SERVICE.{type.BaseType.Name}, DOMAIN.{type.BaseType.Name}>()");
            }

            return string.Join($"{Environment.NewLine}{GetTab(4)}", listConversions);
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

            return rootNamespace;
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