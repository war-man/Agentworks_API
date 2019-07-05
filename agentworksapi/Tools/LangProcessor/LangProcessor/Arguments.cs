using MoneyGram.Common.Diagnostics;
using System.IO;
using System.Linq;

namespace LangProcessor.Models
{
    public static class Arguments
    {
        private static string[] args;
        private static ILog log = LogManager.GetLogger(typeof(Arguments));

        public static string SourceDirectory
        {
            get
            {
                var sourceParam = "-s";
                var dir = Arguments.args.FirstOrDefault(x => x.Contains(sourceParam));

                if (dir != null)
                {
                    return dir.Substring(sourceParam.Length);
                }

                return Directory.GetCurrentDirectory();
            }
        }

        public static string DestinationDirectory
        {
            get
            {
                var destParam = "-d";
                var dir = Arguments.args.FirstOrDefault(x => x.Contains(destParam));

                if (dir != null)
                {
                    return dir.Substring(destParam.Length);
                }

                return Directory.GetCurrentDirectory();
            }
        }

        public static bool ForceOverwrite
        {
            get
            {
                return Arguments.args.Any(x => x == "-f");
            }
        }

        public static bool HelpRequired
        {
            get
            {
                return Arguments.args.Any(x => x == "-h" || x == "--help" || x == "?");
            }
        }

        public static bool SingleApplicationMode
        {
            get
            {
                return Arguments.args.Any(x => x == "-a");
            }
        }
        
        public static void SetArguments(string[] args)
        {
            Arguments.args = args;
        }

        public static void LogArguments()
        {
            log.Info("Input Directory: " + SourceDirectory);
            log.Info("Destination Directory: " + DestinationDirectory);
            log.Info("Force Overwrite: " + ForceOverwrite);
            log.Info("Single Application Mode: " + SingleApplicationMode);
            log.Info("Help Required: " + HelpRequired);
        }
    }
}
