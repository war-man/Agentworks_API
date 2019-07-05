using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect.UpgradeUtility.Generators.AgentConnectRepository
{
    class AgentConnectModelsGenerator : IGenerator
    {
        public void Generate()
        {
            var toWrite = new List<string>();

            var modelTypes = typeof(AgentConnectClient).GetModelsInNamespace();
            var enumTypes = typeof(AgentConnectClient).GetEnumsInNamespace();

            foreach (var modelType in modelTypes)
            {
                toWrite.Add(GenerateClass(modelType));
            }

            foreach (var enumType in enumTypes)
            {
                toWrite.Add(GenerateEnum(enumType));
            }

            var methodsString = String.Join($"{Environment.NewLine}", toWrite);

            WriteFile(methodsString);
        }

        private string GenerateClass(Type modelType)
        {
            var fields = GetFields(modelType);
            var modelProperties = fields.Where(x => !IsSpecifiedField(x)).ToList();
            var properties = new List<string>();

            foreach (var property in modelProperties)
            {
                properties.Add($"\tpublic {GetPropertyType(property, fields)} {property.Name.Capitalize()} {{ get; set; }}");
            }

            var replacements = new Dictionary<string, string>
            {
                {"CLASSNAME", GetClassNameWithInheritance(modelType) },
                {"PROPERTIES", string.Join(Environment.NewLine, properties) },
            };

            var fileText = Resource.AgentConnectModelsTemplate_Class.Format(replacements);
            var lines = fileText.Split(Environment.NewLine).Select(line => "\t\t" + line);

            return string.Join(Environment.NewLine, lines);
        }

        private string GenerateEnum(Type type)
        {
            var enumValues = type.GetEnumerationValues();
            var properties = new List<string>();
            for (var i = 0; i < enumValues.Count; i++)
            {
                properties.Add($"\t{enumValues[i]}{(i != enumValues.Count - 1 ? "," : "")}");

            }

            var replacements = new Dictionary<string, string>
            {
                {"CLASSNAME", type.Name },
                {"PROPERTIES", string.Join(Environment.NewLine, properties) },
            };

            var fileText = Resource.AgentConnectModelsTemplate_Enum.Format(replacements);
            var lines = fileText.Split(Environment.NewLine).Select(line => "\t\t" + line);

            return string.Join(Environment.NewLine, lines);
        }

        private void WriteFile(string modelsString)
        {
            var replacements = new Dictionary<string, string>
            {
                {"MODELS", modelsString}
            };

            var fileText = Resource.AgentConnectModelsTemplate.Format(replacements);
            fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);

            var filePath = Path.Combine("..\\..\\..\\..\\", "MoneyGram.AgentConnect.DomainModel", "Transaction",
                "AgentConnectModels.cs");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        private static bool IsNullableType(System.Reflection.PropertyInfo field, List<System.Reflection.PropertyInfo> fields)
        {
            var lowerFieldName = field.Name.ToLower();
            return fields.Any(x => x.Name.ToLower() == lowerFieldName + "specified");
        }

        public static bool IsSpecifiedField(System.Reflection.PropertyInfo field)
        {
            return field.Name.ToLower().EndsWith("specified");
        }

        public static List<System.Reflection.PropertyInfo> GetFields(Type type)
        {
            return type.GetMembers()
                .Where(x => x.MemberType == MemberTypes.Property && x.DeclaringType == type)
                .Cast<System.Reflection.PropertyInfo>()
                .ToList();
        }

        public static string GetClassNameWithInheritance(Type type)
        {
            var baseType = type.BaseType;
            var baseTypeString = baseType != null && baseType != typeof(object) && baseType != typeof(Enum)
                ? $" : {baseType.Name}"
                : string.Empty;

            return type.Name + baseTypeString;
        }

        public static string GetPropertyType(System.Reflection.PropertyInfo fieldInfo, List<System.Reflection.PropertyInfo> fields)
        {
            var propType = fieldInfo.PropertyType.IsArray
                ? $"List<{GetTypeAlias(fieldInfo.PropertyType.GetElementType().Name)}>"
                : GetTypeAlias(fieldInfo.PropertyType.Name);

            propType += IsNullableType(fieldInfo, fields) ? "?" : string.Empty;

            return propType;
        }

        public static string GetTypeAlias(string propertyType)
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
    }
}