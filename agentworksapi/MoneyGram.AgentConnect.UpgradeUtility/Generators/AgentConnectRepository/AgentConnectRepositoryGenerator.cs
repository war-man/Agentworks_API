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
    class AgentConnectRepositoryGenerator : IGenerator
    {
        public void Generate()
        {
            var toWrite = new List<string>();
            var serviceMethods = typeof(AgentConnectClient).GetAsyncMethods();

            foreach (MethodInfo method in serviceMethods)
            {
                toWrite.Add(
                    GenerateMethod(method)
                );
            }

            var methodsString = String.Join($"{Environment.NewLine}", toWrite);

            WriteFile(methodsString);
        }

        public string GenerateMethod(MethodInfo method)
        {
            var parameter = method.GetParameters()[0];

            var methodNamePascal = method.Name.ToPascalCase().TrimEnd("Async");
            var methodNameCamel = methodNamePascal.ToCamelCase();
            var requestTypePascal = parameter.ParameterType.Name;
            var requestTypeCamel = methodNameCamel + "Request";
            var responseTypePascal = requestTypePascal.Replace("Request", "Response");
            var responseTypeCamel = methodNameCamel + "Response";

            var replacements = new Dictionary<string, string>
            {
                {"MethodNamePascal", methodNamePascal},
                {"MethodNameCamel", methodNameCamel},
                {"RequestTypePascal", requestTypePascal},
                {"RequestTypeCamel", requestTypeCamel},
                {"ResponseTypePascal", responseTypePascal},
                {"ResponseTypeCamel", responseTypeCamel}
            };

            var fileText = Resource.AgentConnectRepositoryTemplate_Method.Format(replacements);
            var lines = fileText.Split(Environment.NewLine).Select(line => "\t\t" + line);

            return string.Join(Environment.NewLine, lines);
        }

        private void WriteFile(string supaString)
        {
            var replacements = new Dictionary<string, string>
            {
                {"METHODS", supaString}
            };

            var fileText = Resource.AgentConnectRepositoryTemplate.Format(replacements);
            fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);

            var filePath = Path.Combine("..\\..\\..\\..\\", "MoneyGram.AgentConnect",
                "AgentConnectRepository.cs");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }
    }
}