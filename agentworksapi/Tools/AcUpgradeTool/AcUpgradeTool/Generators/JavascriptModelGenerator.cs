using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AcUpgradeTool.Generators
{
    public class JavascriptModelGenerator : IGenerator
    {
        private readonly string _outputDir = Path.Combine("output", "AwStaticContent", "app", "js", "viewModels", "transaction");
        private readonly string _template = Resources.JsModelTemplate;
        private readonly string _modelTypeTemplate = Resources.JsModelTypeTemplate;

        private readonly List<Regex> _typesToProcess = new List<Regex>
        {
            new Regex("Request$"),
            new Regex("Payload$")
        };

        public void Generate(List<Type> types)
        {
            if (Directory.Exists(_outputDir))
            {
                Directory.Delete(_outputDir, true);
            }

            Directory.CreateDirectory(_outputDir);

            var generatedTypes = new List<Type>();
            foreach (var type in types)
            {
                // Ignore non-poco classes
                if (type.GetMethods().Any(x => x.DeclaringType == type) || type.IsInterface)
                {
                    continue;
                }

                // Ignore classes not included in _typesToProcess
                if (!_typesToProcess.Any(regex => regex.IsMatch(type.Name)))
                {
                    continue;
                }

                generatedTypes.Add(type);

                var replacements = new Dictionary<string, string>
                {
                    {"ModelType", type.Name},
                    {"ModelTypeLower", type.Name.ToCamelCase()},
                    {"ModelProperties", GetModelProperties(type)}
                };

                var fileText = _template.Format(replacements);
                var filePath = Path.Combine(_outputDir, type.Name + ".js");

                File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
            }

            CreateViewModelTypeConstant(generatedTypes);
        }

        public void LogStart()
        {
            Console.Write($"{Environment.NewLine}Started Javascript Model Generation...");
        }

        public void LogComplete()
        {
            Console.Write("Complete");
        }

        private static string GetModelProperties(Type type, int currentTabs = 3)
        {
            var properties =
                type.GetMembers()
                    .Where(x => x.MemberType == MemberTypes.Field && !x.Name.ToLower().Contains("specified"))
                    .Cast<FieldInfo>();

            var fields = new List<string>();
            var baseTypeFields = new List<FieldInfo>();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var baseTypeOnlyProperty = !ShouldIncludeProperty(prop, type);

                // If property should only be exposed on the base type, exclude it from the fields on the JS model
                if (baseTypeOnlyProperty)
                {
                    baseTypeFields.Add(prop);
                    continue;
                }

                // Set value types and enums to null
                if (prop.FieldType.IsSubclassOf(typeof (ValueType)) || prop.FieldType == typeof (string))
                {
                    fields.Add($"{propName}: null");
                }

                // Set arrays to empty array
                else if (prop.FieldType.IsArray)
                {
                    fields.Add($"{propName}: []");
                }

                // Complex types should be fully expanded
                else
                {
                    fields.Add($"{propName}: {GetModelProperties(prop.FieldType, currentTabs + 1)}");
                }
            }

            var jsObject = $"{{" +
                           $"{FieldsString(fields, currentTabs)}" +
                           $"{BaseTypeFieldsString(baseTypeFields, currentTabs)}" +
                           $"{Environment.NewLine}{GetTab(currentTabs - 1)}}}";

            return jsObject;
        }

        private static string FieldsString(List<string> fieldInfos, int currentTabs)
        {
            if (!fieldInfos.Any())
            {
                return string.Empty;
            }

            return $"{Environment.NewLine}{GetTab(currentTabs)}" +
                   string.Join($",{Environment.NewLine}{GetTab(currentTabs)}", fieldInfos);
        }

        private static string BaseTypeFieldsString(List<FieldInfo> baseTypeFields, int currentTabs)
        {
            if (!baseTypeFields.Any())
            {
                return string.Empty;
            }

            var baseType = baseTypeFields.First().DeclaringType.Name;
            var fieldNames = baseTypeFields.Select(f => $"{Environment.NewLine}{GetTab(currentTabs)}// {f.Name}");

            return $"{Environment.NewLine}{GetTab(currentTabs)}// DO NOT UNCOMMENT" +
                   $"{Environment.NewLine}{GetTab(currentTabs)}// Properties defined by base type {baseType}" +
                   $"{string.Join(string.Empty, fieldNames)}";
        }

        private static string GetTab(int numTabs)
        {
            return string.Concat(Enumerable.Repeat("    ", numTabs));
        }

        /// <summary>
        ///     Determines if a field should be included on the JS model.
        ///     Properties from base types Request and Payload are not included on the JS models.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        private static bool ShouldIncludeProperty(FieldInfo field, Type fromType)
        {
            // Include all properties on Request and Payload objects
            if (fromType.Name.ToLower() == "request" || fromType.Name.ToLower() == "payload")
            {
                return true;
            }

            // Do not include properties declared on Request or Payload
            return !(field.DeclaringType.Name.ToLower() == "request" || field.DeclaringType.Name.ToLower() == "payload");
        }

        private void CreateViewModelTypeConstant(List<Type> types)
        {
            var modelTypes = types.Select(x => $"{x.Name.ToCamelCase()}: '{x.Name}'").ToList();
            var joinedModelTypes = string.Join($",{Environment.NewLine}{GetTab(2)}", modelTypes);

            var replacements = new Dictionary<string, string>
            {
                {"ModelTypes", joinedModelTypes}
            };

            var fileText = _modelTypeTemplate.Format(replacements);
            var filePath = Path.Combine(_outputDir, "TransactionModelType.js");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }
    }
}