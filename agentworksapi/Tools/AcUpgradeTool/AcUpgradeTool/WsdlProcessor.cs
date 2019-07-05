using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services.Description;
using Microsoft.CSharp;

namespace AcUpgradeTool
{
    public static class WsdlProcessor
    {
        public static List<Type> ProcessWsdlTypes(string wsdlLocation)
        {
            LogStart();

            const string outputFile = "output.cs";

            GenerateCodeForWsdl(wsdlLocation, outputFile);

            var assembly = CompileFileToAssembly(outputFile);

            var types = assembly.GetTypes();

            LogComplete();

            return types.ToList();
        }

        private static void LogStart()
        {
            Console.Write("\nStarted WSDL processing...");
        }

        private static void LogComplete()
        {
            Console.Write("Complete");
        }

        private static void GenerateCodeForWsdl(string wsdlLocation, string outputFile)
        {
            var reader = new StreamReader(wsdlLocation);
            var service = ServiceDescription.Read(reader);
            var importer = new ServiceDescriptionImporter();

            importer.AddServiceDescription(service, "", "");

            var @namespace = new CodeNamespace("MoneyGram.AgentConnect.Repository.AgentConnectService");
            var program = new CodeCompileUnit();

            program.Namespaces.Add(@namespace);

            importer.Import(@namespace,
                program);

            var writer = new StreamWriter(outputFile);
            importer.CodeGenerator.GenerateCodeFromCompileUnit(program, writer, new CodeGeneratorOptions());

            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        private static Assembly CompileFileToAssembly(string csFilePath)
        {
            var provider = new CSharpCodeProvider();

            var compilerParams = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = true
            };

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Web.Services.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add("System.ComponentModel.dll");

            var results = provider.CompileAssemblyFromFile(compilerParams, csFilePath);

            return results.CompiledAssembly;
        }
    }
}