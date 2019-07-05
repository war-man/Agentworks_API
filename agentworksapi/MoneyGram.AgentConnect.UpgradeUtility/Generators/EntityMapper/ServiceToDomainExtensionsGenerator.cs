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
    class ServiceToDomainExtensionsGenerator : IGenerator
    {
        public void Generate()
        {
            var toWrite = new List<string>();

            var modelTypes = typeof(AgentConnectClient).GetModelsInNamespace();

            foreach (var modelType in modelTypes)
            {
                toWrite.Add(GenerateExtensions(modelType));
            }

            var methodsString = String.Join($"{Environment.NewLine}", toWrite);

            WriteFile(methodsString);
        }

        private string GenerateExtensions(Type modelType)
        {
            var properties = new List<string>();
            var modelProperties = modelType.GetPropertyInfos();
            var nameCamel = modelType.Name.ToCamelCase();

            foreach (var propertyInfo in modelProperties)
            {
                if (propertyInfo.Name.ToLower().EndsWith("specified"))
                {
                    continue;
                }

                properties.Add($"\t\t{propertyInfo.Name.ToPascalCase()} = {GetAssignmentValue(nameCamel, propertyInfo, modelProperties)},");
            }

            var replacements = new Dictionary<string, string>
            {
                {"NAME", modelType.Name },
                {"NAME_CAMEL", nameCamel },
                {"PROPERTIES", string.Join(Environment.NewLine, properties) },
            };

            var fileText = Resource.ServiceToDomainExtensionsTemplate_Extension.Format(replacements);
            var lines = fileText.Split(Environment.NewLine).Select(line => "\t\t" + line);

            return string.Join(Environment.NewLine, lines);
        }

        private void WriteFile(string extensionsString)
        {
            var replacements = new Dictionary<string, string>
            {
                {"EXTENSIONS", extensionsString}
            };

            var fileText = Resource.ServiceToDomainExtensionsTemplate.Format(replacements);
            fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);

            var filePath = Path.Combine("..\\..\\..\\..\\", "MoneyGram.AgentConnect", "EntityMapper",
                "ServiceToDomainExtensions.cs");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        public static bool IsValueType(Type t)
        {
            return t.IsValueType;
        }

        public static string GetAssignmentValue(string paramName, PropertyInfo info, List<PropertyInfo> properties)
        {
            var isSpecified = true;
            var isNullable = info.IsNullableType(properties);
            var propertyType = info.PropertyType.Name;
            var propertyName = $"{paramName}.{info.Name}";

            var specifiedPrefix = isNullable ? $"{propertyName}Specified == true ? " : string.Empty;
            var specifiedSuffix = isNullable ? $" : null" : String.Empty;
            var nullableOperand = isNullable ? "?" : string.Empty;

            if (info.PropertyType.IsEnum)
            {
                return $"{specifiedPrefix}(DOMAIN.{info.PropertyType.Name}{nullableOperand}){propertyName}{specifiedSuffix}";
            }
            else if (info.PropertyType.IsArray)
            {
                var elementType = info.PropertyType.GetElementType();
                var isNestedArray = elementType.IsArray;

                if (elementType.Namespace.ToLower().Contains("moneygram"))
                {
                    elementType = isNestedArray ? elementType.GetElementType() : elementType;

                    var selectStatement = isNestedArray ?
                        $"?.Select(x => x?.Select(y => y.ConvertToDomain<SERVICE.{elementType.Name}, DOMAIN.{elementType.Name}>()).ToArray()).ToList()" :
                        $"?.Select(x => x.ConvertToDomain<SERVICE.{elementType.Name}, DOMAIN.{elementType.Name}>()).ToList()";

                    return $"{specifiedPrefix}{propertyName}{selectStatement}{specifiedSuffix}";
                }
                else
                {
                    return $"{specifiedSuffix}{propertyName}?.ToList(){specifiedSuffix}";
                }
            }
            else if (info.PropertyType.Namespace.ToLower().Contains("moneygram"))
            {
                return $"{specifiedPrefix}({propertyName} != null ? {propertyName}.ConvertToDomain<SERVICE.{propertyType}, DOMAIN.{propertyType}>() : null){specifiedSuffix}";
            }

            // base case for value types
            var nullableType = isNullable ? $"({info.PropertyType.GetTypeAlias()}?)" : String.Empty;
            return $"{specifiedPrefix}{nullableType}{propertyName}{specifiedSuffix}";
        }
    }
}