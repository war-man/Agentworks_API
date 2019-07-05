using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

namespace AcUpgradeTool.Generators
{
    public class CSharpModelGenerator : IGenerator
    {
        private static readonly string Template = Resources.CSharpModelTemplate;

        private readonly string _defaultNamespace;
        private readonly string _modelDir;
        private readonly string _requestModelDir;
        private readonly string _requestNamespace;
        private readonly string _responseModelDir;
        private readonly string _responseNamespace;

        private readonly CSharpCodeProvider _codeProvider;

        public CSharpModelGenerator(string rootNamespace, string modelDir)
        {
            _modelDir = modelDir;
            _defaultNamespace = rootNamespace;

            _requestNamespace = _defaultNamespace + ".Request";
            _responseNamespace = _defaultNamespace + ".Response";
            _requestModelDir = Path.Combine(_modelDir, "Request");
            _responseModelDir = Path.Combine(_modelDir, "Response");

            _codeProvider = new CSharpCodeProvider();
        }

        public void Generate(List<Type> types)
        {
            CreateOutputDirectories();

            foreach (var type in types)
            {
                // Ignore non-poco and container classes
                if (type.GetMethods().Any(x => x.DeclaringType == type) || type.IsInterface)
                {
                    continue;
                }

                var replacements = new Dictionary<string, string>
                {
                    {"usingStatements", GetAdditionalUsingStatements(type)},
                    {"namespace", GetModelNamespace(type)},
                    {"classType", GetClassType(type, types)},
                    {"className", GetClassNameWithInheritance(type)},
                    {"properties", GetModelProperties(type)}
                };

                var fileText = Template.Format(replacements);
                var filePath = Path.Combine(GetModelDir(type), type.Name + ".cs");

                File.WriteAllText(filePath, fileText, Encoding.UTF8);
            }
        }

        public void LogStart()
        {
            Console.Write($"{Environment.NewLine}Started C# Model Generation for {_modelDir}...");
        }

        public void LogComplete()
        {
            Console.Write("Complete");
        }


        private void CreateOutputDirectories()
        {
            if (Directory.Exists(_modelDir))
            {
                Directory.Delete(_modelDir, true);
            }

            Directory.CreateDirectory(_modelDir);
            Directory.CreateDirectory(_requestModelDir);
            Directory.CreateDirectory(_responseModelDir);
        }

        private string GetModelNamespace(Type type)
        {
            var modelNamespace = type.IsRequest() ? _requestNamespace : _defaultNamespace;
            modelNamespace = type.IsResponse() ? _responseNamespace : modelNamespace;

            return modelNamespace;
        }

        private string GetClassType(Type type, List<Type> typesInAssembly)
        {
            if (type.IsEnum)
            {
                return "enum";
            }

            return typesInAssembly.Any(x => x.IsSubclassOf(type)) ? "class" : "sealed class";
        }

        private string GetClassNameWithInheritance(Type type)
        {
            var baseType = type.BaseType;
            var baseTypeString = baseType != null && baseType != typeof (object) && baseType != typeof (Enum)
                ? $" : {baseType.Name}"
                : string.Empty;

            return type.Name + baseTypeString;
        }

        private string GetModelProperties(Type type)
        {
            return type.IsEnum
                ? ModelPropertiesForEnum(type)
                : ModelPropertiesForClass(type);
        }

        private string ModelPropertiesForEnum(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList();

            var properties = new List<string>();
            
            foreach (var fieldInfo in fields)
            {
                var identifierPrefix = _codeProvider.IsValidIdentifier(fieldInfo.Name) ? string.Empty : "@";

                properties.Add($"{identifierPrefix}{fieldInfo.Name}");
            }

            return string.Join($",{Environment.NewLine}{GetTab(2)}", properties);
        }

        private string ModelPropertiesForClass(Type type)
        {
            var fields =
                type.GetMembers()
                    .Where(x => x.MemberType == MemberTypes.Field && x.DeclaringType == type)
                    .Cast<FieldInfo>()
                    .ToList();

            var nullableProperties = NullableProperties(fields);

            var properties = new List<string>();

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.Name.ToLower().EndsWith("specified"))
                {
                    continue;
                }

                var propType = GetPropertyType(fieldInfo, nullableProperties);

                properties.Add($"public {propType} {fieldInfo.Name.Capitalize()} {{ get; set; }}");
            }

            return string.Join($"{Environment.NewLine}{GetTab(2)}", properties);
        }

        private HashSet<string> NullableProperties(List<FieldInfo> fieldInfos)
        {
            var specifiedProperties = fieldInfos
                .Where(x => x.Name.ToLower().EndsWith("specified"))
                .Select(x => Regex.Replace(x.Name, "Specified$", string.Empty));

            return new HashSet<string>(specifiedProperties);
        }

        private string GetPropertyType(FieldInfo fieldInfo, HashSet<string> nullableProperties)
        {
            var propType = fieldInfo.FieldType.IsArray
                ? $"List<{GetTypeAlias(fieldInfo.FieldType.GetElementType().Name)}>"
                : GetTypeAlias(fieldInfo.FieldType.Name);

            var isNullable = nullableProperties.Contains(fieldInfo.Name);
            propType += isNullable ? "?" : string.Empty;

            return propType;
        }

        private string GetTypeAlias(string propertyType)
        {
            switch (propertyType)
            {
                case "String":
                    return "string";
                case "Decimal":
                    return "decimal";
                case "Int32":
                    return "int";
                case "Boolean":
                    return "bool";
                default:
                    return propertyType;
            }
        }

        private string GetAdditionalUsingStatements(Type type)
        {
            var properties = type.GetMembers()
                .Where(x => x.MemberType == MemberTypes.Field && x.DeclaringType == type)
                .Cast<FieldInfo>();

            if (properties.Any(fieldInfo => fieldInfo.FieldType.IsArray))
            {
                return $"using System.Collections.Generic;{Environment.NewLine}";
            }

            return string.Empty;
        }

        private string GetModelDir(Type type)
        {
            if (type.IsRequest())
            {
                return _requestModelDir;
            }

            if (type.IsResponse())
            {
                return _responseModelDir;
            }

            return _modelDir;
        }

        private static string GetTab(int numTabs)
        {
            return string.Concat(Enumerable.Repeat("    ", numTabs));
        }
    }
}