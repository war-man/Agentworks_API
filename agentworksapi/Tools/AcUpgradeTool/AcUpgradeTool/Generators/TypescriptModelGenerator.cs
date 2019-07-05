using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AcUpgradeTool.Generators
{
    public class TypescriptModelGenerator : IGenerator
    {
        private readonly string _outputDir =
            Path.Combine("output", "AwStaticContent", "app", "ts", "viewModels", "transaction");

        private readonly string _modelTemplate = Resources.TsModelTemplate;
        private readonly string _enumTemplate = Resources.TsEnumTemplate;
        private readonly string _modelTypeTemplate = Resources.TsModelTypeTemplate;
        private readonly string _moduleTemplate = Resources.TsModuleTemplate;

        public void Generate(List<Type> types)
        {
            if (Directory.Exists(_outputDir))
            {
                Directory.Delete(_outputDir, true);
            }

            Directory.CreateDirectory(_outputDir);

            var requestAndResponseTypes = GetRequestAndResponseTypes(types);
            var typesToProcess = GetTypesToProcess(types);

            foreach (var type in typesToProcess)
            {
                if (type.IsEnum)
                {
                    GenerateEnum(type);
                }
                else
                {
                    GenerateModel(type);
                }
            }

            CreateViewModelTypeConstant(requestAndResponseTypes);
            CreateModule(requestAndResponseTypes);
            CreateIndex(typesToProcess);
        }

        private void GenerateModel(Type type)
        {
            var modelProperties = GetModelProperties(type);
            var modelType = GetModelType(type);
            var replacements = new Dictionary<string, string>
            {
                {"ModelType", $"{modelType}"},
                {"ModelTypeLower", type.Name.ToCamelCase()},                
                {"Imports", CreateImportString(type)},
                {"ModelProperties", modelProperties}
            };

            var fileText = _modelTemplate.Format(replacements);
            fileText = Regex.Replace(fileText,  @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);
            var filePath = Path.Combine(_outputDir, type.Name + ".ts");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        private void GenerateEnum(Type type)
        {
            var replacements = new Dictionary<string, string>
            {
                {"EnumType", GetModelType(type)},
                {"EnumValues", GetEnumValues(type)}
            };

            var fileText = _enumTemplate.Format(replacements);
            var filePath = Path.Combine(_outputDir, type.Name + ".ts");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        private List<Type> GetRequestAndResponseTypes(List<Type> allTypes)
        {
            var regex = new Regex("(Request|Payload)$");

            return allTypes
                .Where(x => IsRequestOrResponse(x) && IsPoco(x))
                .ToList();
        }

        /// <summary>
        /// Iterates over selected types, extracting a deduped set of child classes
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private List<Type> GetTypesToProcess(List<Type> types)
        {
            var typesToProcess = new List<Type>();

            foreach (var type in types)
            {
                typesToProcess.Add(type);

                var properties = GetProperties(type);

                // Select types whose namespace is the same as the current type
                var agentConnectPropertyTypes = properties
                    .Where(x => x.FieldType.Namespace == type.Namespace && IsPoco(x.FieldType))
                    .Select(x => x.FieldType)
                    .ToList();

                var collectionTypes = properties
                    .Where(x => x.FieldType.IsArray)
                    .Select(x => x.FieldType.GetElementType())
                    .Where(x => x.Namespace == type.Namespace && IsPoco(x))
                    .ToList();

                typesToProcess.AddRange(agentConnectPropertyTypes);
                typesToProcess.AddRange(collectionTypes);
            }

            return typesToProcess
                .Distinct()
                .ToList();
        }

        private string GetModelType(Type type)
        {
            if (type.BaseType.Namespace == type.Namespace)
            {
                return $"{type.Name} extends {type.BaseType.Name}";
            }

            return $"{type.Name}";
        }

        private static bool IsPoco(Type type)
        {
            var typeName = type.Name;
            return !type.GetMethods().Any(x => x.DeclaringType == type) && !type.IsInterface && !type.IsEnum;
        }

        private static bool IsRequestOrResponse(Type type)
        {
            var regex = new Regex("(Request|Payload)$");

            return regex.IsMatch(type.Name);
        }

        private static List<FieldInfo> GetProperties(Type type)
        {
            var properties =
                type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.MemberType == MemberTypes.Field && !x.Name.ToLower().Contains("specified"))
                    .Cast<FieldInfo>()
                    .ToList();

            return properties;
        }

        private static string GetModelProperties(Type type)
        {
            var properties = GetProperties(type)
                .OrderBy(x => x.Name);

            var fields = new List<string>();

            foreach (var prop in properties)
            {
                var propName = prop.Name;

                // Set value types and enums to null
                if (IsPrimitiveType(prop.FieldType))
                {
                    fields.Add($"public {propName}?: {GetPrimitiveType(prop.FieldType)} = null;");
                }

                // Set arrays to empty array
                else if (prop.FieldType.IsArray)
                {
                    var arrayType = IsPrimitiveType(prop.FieldType.GetElementType())
                        ? GetPrimitiveType(prop.FieldType.GetElementType())+"[]"
                        : prop.FieldType.GetElementType().Name+"[]";

                    fields.Add($"public {propName}?: {arrayType} = [];");
                }

                // Complex types should reference their type if within the AgentConnect namespace
                else
                {
                    var fieldType = prop.FieldType.Namespace == type.Namespace ?
                        prop.FieldType.Name :
                        "object";
                    fields.Add($"public {propName}?: {fieldType} = null;");
                }
            }

            var tsObject = $"{FieldsString(fields)}";

            return tsObject;
        }

        private static string GetEnumValues(Type type)
        {
            var enumValues = new List<string>();
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                enumValues.Add(value.ToString());
            }

            return GetTab(1) + string.Join($",{Environment.NewLine}{GetTab(1)}", enumValues) + Environment.NewLine;
        }

        private static bool IsPrimitiveType(Type type)
        {
            return !type.IsEnum && (type.IsSubclassOf(typeof(ValueType)) || type == typeof(string));
        }

        private static string GetPrimitiveType(Type type)
        {
            if (type == typeof(string) || type == typeof(DateTime))
            {
                return "string";
            }
            else if (type == typeof(int) || type == typeof(decimal) || type == typeof(long) || type == typeof(float) || type == typeof(byte))
            {
                return "number";
            }
            else if (type == typeof(bool))
            {
                return "boolean";
            }

            return type.Name;
        }

        private Type GetImportType(Type type)
        {
            if (type.IsArray)
            {
                return GetImportType(type.GetElementType());
            }

            return type;
        }

        private string CreateImportString(Type type)
        {
            var properties = GetProperties(type);

            var typesToImport = new List<string>();

            // Import base type if within agent connect namespace
            if (type.BaseType.Namespace == type.Namespace)
            {
                typesToImport.Add(type.BaseType.Name);
            }

            foreach (var prop in properties)
            {
                var propertyType = GetImportType(prop.FieldType);

                if (IsPrimitiveType(propertyType) || propertyType.Namespace != type.Namespace)
                {
                    continue;
                }

                typesToImport.Add(propertyType.Name);
            }

            var result =
                typesToImport
                    .OrderBy(x => x)
                .Distinct()
                .Select(x => $"import {{{x}}} from './{x}';")
                .ToList();

            return result.Count > 0 ? string.Join(Environment.NewLine, result) : string.Empty;
        }


        private static string FieldsString(List<string> fieldInfos)
        {
            if (!fieldInfos.Any())
            {
                return string.Empty;
            }

            return GetTab(1) + string.Join($"{Environment.NewLine}{GetTab(1)}", fieldInfos) + Environment.NewLine;
        }

        private static string GetTab(int numTabs)
        {
            return string.Concat(Enumerable.Repeat("    ", numTabs));
        }

        private void CreateViewModelTypeConstant(List<Type> types)
        {
            var modelTypes = types
                .OrderBy(x => x.Name)
                .Select(x => $"{x.Name.ToCamelCase()}: '{x.Name}'")
                .ToList();
            var joinedModelTypes = string.Join($",{Environment.NewLine}{GetTab(1)}", modelTypes);

            var replacements = new Dictionary<string, string>
            {
                {"ModelTypes", joinedModelTypes}
            };

            var fileText = _modelTypeTemplate.Format(replacements);
            var filePath = Path.Combine(_outputDir, "TransactionModelType.ts");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        private void CreateModule(List<Type> types)
        {
            // Imports
            var imports = types
                .OrderBy(x => x.Name)
                .Select(x => $"import {{{x.Name}}} from './{x.Name}';")
                .ToList();

            imports.Add($"{Environment.NewLine}import {{TransactionModelType}} from './TransactionModelType';");

            var joinedImports = string.Join(Environment.NewLine, imports);

            // Module Items
            var moduleItems = types
                .Select(x => $".value('{x.Name}', {x.Name})")
                .ToList();

            moduleItems.Add($".constant('TransactionModelType', TransactionModelType)");

            var joinedModuleItems = string.Join($"{Environment.NewLine}{GetTab(1)}", moduleItems);


            var replacements = new Dictionary<string, string>
            {
                {"Imports", joinedImports},
                {"ModuleItems", joinedModuleItems}
            };

            var fileText = _moduleTemplate.Format(replacements);
            var filePath = Path.Combine(_outputDir, "transaction-view-models.module.ts");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }

        private void CreateIndex(List<Type> types)
        {
            var exports = types
                .OrderBy(x => x.Name)
                .Select(x => $"export * from './{x.Name}';")
                .ToList();

            exports.Add("export * from './TransactionModelType';");

            var joinedExports = string.Join(Environment.NewLine, exports.OrderBy(x=>x));

            var filePath = Path.Combine(_outputDir, "index.ts");

            File.WriteAllText(filePath, joinedExports, new UTF8Encoding(false));
        }

        public void LogStart()
        {
            Console.Write($"{Environment.NewLine}Started Typescript Model Generation...");
        }

        public void LogComplete()
        {
            Console.Write("Complete");
        }
    }
}