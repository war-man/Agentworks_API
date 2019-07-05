using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect.UpgradeUtility.Generators.IAgentConnect
{
    public class IAgentConnectGenerator : IGenerator
    {

        public void Generate()
        {
            var toWrite = new List<string>();
            var serviceMethods = typeof(AgentConnectClient).GetAsyncMethods();

            foreach (MethodInfo method in serviceMethods)
            {
                var parameter = method.GetParameters()[0];
                var parameterName = parameter.ParameterType.Name;

                var parameterString = $"{parameterName} {parameterName.ToCamelCase()}";
                var returnType = parameterName.Replace("Request", "Response");

                toWrite.Add($"{returnType} {method.Name.ToPascalCase().TrimEnd("Async")}(Agent agent, {parameterString});");
                toWrite.Add($"Task<{returnType}> {method.Name.ToPascalCase()}(Agent agent, {parameterString});");
            }

            var supaString = String.Join($"{Environment.NewLine}\t\t", toWrite);

            WriteFile(supaString);
        }

        private void WriteFile(string interfaceMethods)
        {
            var replacements = new Dictionary<string, string>
            {
                {"INTERFACE_METHODS", interfaceMethods}
            };

            var fileText = Resource.IAgentConnectTemplate.Format(replacements);
            fileText = Regex.Replace(fileText, @"^\s+$[\r\n]*", Environment.NewLine, RegexOptions.Multiline);

            var filePath = Path.Combine("..\\..\\..\\..\\", "MoneyGram.AgentConnect", "IAgentConnect.cs");

            File.WriteAllText(filePath, fileText, new UTF8Encoding(false));
        }
    }
}
