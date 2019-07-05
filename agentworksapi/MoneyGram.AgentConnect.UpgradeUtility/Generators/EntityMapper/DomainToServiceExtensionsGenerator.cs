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
    class DomainToServiceExtensionsGenerator : IGenerator
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
                properties.Add($"\t\t{propertyInfo.Name} = {GetAssignmentValue(nameCamel, propertyInfo, modelProperties)},");
            }

            var replacements = new Dictionary<string, string>
            {
                {"NAME", modelType.Name },
                {"NAME_CAMEL", nameCamel },
                {"PROPERTIES", string.Join(Environment.NewLine, properties) },
            };

            var fileText = Resource.DomainToServiceExtensionsTemplate_Extension.Format(replacements);
            var lines = fileText.Split(Environment.NewLine).Select(line => "\t\t" + line);

            return string.Join(Environment.NewLine, lines);
        }

        private void WriteFile(string extensionsString)
        {
            var replacements = new Dictionary<string, string>
            {
                {"EXTENSIONS", extensionsString}
            };

            var fileText = Resource.DomainToServiceExtensionsTemplate.Format(replacements);
            fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);

            var filePath = Path.Combine("..\\..\\..\\..\\", "MoneyGram.AgentConnect", "EntityMapper",
                "DomainToServiceExtensions.cs");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        public static string GetNullableTypeConversion(Type type)
        {
            if (type.IsEnum)
            {
                return string.Empty;
            }

            return $" ?? default({type.GetTypeAlias()})";
        }

        public static string GetAssignmentValue(string paramName, PropertyInfo info, List<PropertyInfo> properties)
        {
            if (info.Name.ToLower().EndsWith("specified"))
            {
                var propName = info.Name.Substring(0, info.Name.LastIndexOf("Specified"));
                return $"{paramName}.{propName.ToPascalCase()}.HasValue";
            }

            var isNullable = info.IsNullableType(properties);
            var propertyType = info.PropertyType.Name;
            var propertyName = $"{paramName}.{info.Name.ToPascalCase()}";
            propertyName += isNullable ? $"{GetNullableTypeConversion(info.PropertyType)}" : String.Empty;

            if (info.PropertyType.IsEnum)
            {
                var enumPropName = $"{paramName}.{info.Name.ToPascalCase()}";
                var enumPropType = $"{info.PropertyType.Name}";

                return isNullable ?
                    $"{paramName}.{info.Name.ToPascalCase()}.HasValue ? (SERVICE.{enumPropType}){enumPropName} : GetDefaultEnumValue<SERVICE.{enumPropType}>()" :
                    $"(SERVICE.{enumPropType}){enumPropName}";
            }
            else if (info.PropertyType.IsArray)
            {
                var elementType = info.PropertyType.GetElementType();
                var isNestedArray = elementType.IsArray;

                if (elementType.Namespace.ToLower().Contains("moneygram"))
                {
                    elementType = isNestedArray ? elementType.GetElementType() : elementType;

                    var selectStatement = isNestedArray ?
                        $"?.Select(x => x?.Select(y => y.ConvertToService<DOMAIN.{elementType.Name}, SERVICE.{elementType.Name}>(agent)).ToArray()).ToArray()" :
                        $"?.ConvertToService<DOMAIN.{elementType.Name}, SERVICE.{elementType.Name}>(agent).ToArray()";

                    return $"{propertyName}{selectStatement}";
                }
                else
                {
                    return $"{propertyName}?.ToArray()";
                }
            }
            else if (info.PropertyType.Namespace.ToLower().Contains("moneygram"))
            {
                return $"{propertyName} != null ? {propertyName}.ConvertToService<DOMAIN.{propertyType}, SERVICE.{propertyType}>(agent) : null";
            }

            // base case for value types
            return propertyName;
        }
    }
}