using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MoneyGram.AgentConnect.UpgradeUtility
{
    public static class Extensions
    {
        public static string Format(this string toFormat, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                toFormat = toFormat.Replace("{" + replacement.Key + "}", replacement.Value);
            }

            return toFormat;
        }

        public static string Capitalize(this string toCapitalize)
        {
            return toCapitalize.First().ToString().ToUpper() + toCapitalize.Substring(1);
        }

        public static string ToCamelCase(this string toCamel)
        {
            return toCamel.First().ToString().ToLower() + toCamel.Substring(1);
        }
        public static string ToPascalCase(this string toPascal)
        {
            return toPascal.First().ToString().ToUpper() + toPascal.Substring(1);
        }

        public static bool IsRequest(this Type type)
        {
            return type.Name.ToLower() == "request" || type.BaseType.Name.ToLower() == "request";
        }

        public static List<MethodInfo> GetSyncMethods(this Type type)
        {
            var serviceMethods = type
                .GetMethods()
                .Where(x => x.DeclaringType == type)
                .Where(x =>
                {
                    var returnType = x.ReturnType;
                    var parameters = x.GetParameters();

                    // Don't include async methods...for now!
                    return returnType.BaseType != typeof(System.Threading.Tasks.Task) &&
                           !parameters.Any(param => param.ParameterType == typeof(System.Threading.Tasks.Task));
                })
                .ToList();

            var sortedMethods = serviceMethods
                .OrderBy(x => x.Name)
                .ToList();

            return sortedMethods;
        }

        public static List<MethodInfo> GetAsyncMethods(this Type type)
        {
            var serviceMethods = type
                .GetMethods()
                .Where(x => x.DeclaringType == type)
                .Where(x =>
                {
                    var returnType = x.ReturnType;
                    var parameters = x.GetParameters();

                    // Don't include async methods...for now!
                    return returnType.BaseType == typeof(System.Threading.Tasks.Task);
                })
                .ToList();

            var sortedMethods = serviceMethods
                .OrderBy(x => x.Name)
                .ToList();

            return sortedMethods;
        }

        public static List<Type> GetModelsInNamespace(this Type type)
        {
            var types = type.Assembly.GetTypes()
                .Where(x => x.Namespace == type.Namespace)
                .Where(x => !x.IsEnum)
                .Where(x =>
                {
                    var attr = (XmlTypeAttribute)x.GetCustomAttribute(typeof(XmlTypeAttribute));
                    return attr != null;
                })
                .ToList();

            return types;
        }

        public static List<Type> GetEnumsInNamespace(this Type type)
        {
            var types = type.Assembly.GetTypes()
                .Where(x => x.Namespace == type.Namespace)
                .Where(x => x.IsEnum)
                .ToList();

            return types;
        }

        public static List<string> GetEnumerationValues(this Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static).ToList();

            var properties = new List<string>();

            foreach (var fieldInfo in fields)
            {
                var identifierPrefix = CodeProvider.Instance.IsValidIdentifier(fieldInfo.Name) ? string.Empty : "@";

                properties.Add($"{identifierPrefix}{fieldInfo.Name}");
            }

            return properties;
        }

        public static List<PropertyInfo> GetPropertyInfos(this Type type)
        {
            return type.GetMembers()
                .Where(x => x.MemberType == MemberTypes.Property)
                .Cast<PropertyInfo>()
                .ToList();
        }

        public static bool IsNullableType(this PropertyInfo field, List<PropertyInfo> fields)
        {
            var lowerFieldName = field.Name.ToLower();
            return fields.Any(x => x.Name.ToLower() == lowerFieldName + "specified");
        }

        public static bool IsResponse(this Type type)
        {
            var responseTypes = new List<string>
            {
                "response",
                "payload"
            };

            return responseTypes.Any(x => type.Name.ToLower().Contains(x));
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        public static string GetTypeAlias(this Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return "string";
                case "Decimal":
                    return "decimal";
                case "Int32":
                case "Int64":
                    return "int";
                case "Boolean":
                    return "bool";
                default:
                    return type.Name;
            }
        }
    }
}