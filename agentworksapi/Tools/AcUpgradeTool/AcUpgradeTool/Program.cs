using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AcUpgradeTool.Generators;

namespace AcUpgradeTool
{
    public class Program
    {
        private static readonly List<IGenerator> Generators = new List<IGenerator>
        {
//            new JavascriptModelGenerator(),
            new TypescriptModelGenerator(),
//            new CSharpModelGenerator("MoneyGram.AgentConnect.DomainModel.Transaction", Path.Combine("output", "MoneyGram.AgentConnect.DomainModel", "Transaction")),
//            new CSharpModelGenerator("AwApi.ViewModels.Transaction", Path.Combine("output", "AwApi.ViewModels", "Transaction")),
//            new AwExtensionGenerator(),
//            new AcExtensionGenerator()
        };

        public static void Main(string[] args)
        {
            LogStart();

            var arguments = ProcessArguments(args);

            if (arguments.ShowHelp)
            {
                Console.WriteLine(Resources.HelpFile);
                return;
            }

            var wsdlTypes = WsdlProcessor.ProcessWsdlTypes(arguments.WsdlPath);

            foreach (var generator in Generators)
            {
                generator.LogStart();
                generator.Generate(wsdlTypes);
                generator.LogComplete();
            }

            LogComplete();
        }
        
        private static Arguments ProcessArguments(string[] args)
        {
            return new Arguments
            {
                ShowHelp = !args.Any() || args.Any(x => x == "?" || x == "/h"),
                WsdlPath = args.Any() ? args[0] : string.Empty
            };
        }

        private static void LogStart()
        {
            Console.WriteLine($"AcModelUpgradeTool version: {Version.AcUpgradeToolVersion}");
        }

        private static void LogComplete()
        {
            Console.Write($"{Environment.NewLine}{Environment.NewLine}Upgrade complete...press any key to exit");
            Console.ReadLine();
        }
    }
}