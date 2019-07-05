using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace MoneyGram.Common.Json
{
    public static class JsonFileHelper
    {
        public static string ExecutingDir()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
        }

        public static string GetFileContents(string fileName)
        {
            string fileContents = null;
            if(File.Exists(fileName))
            {
                fileContents = File.ReadAllText(fileName, Encoding.UTF8);
            }
            return fileContents;
        }

        public static void SaveFileContents(string fileName, string fileContents)
        {
            if(File.Exists(fileName))
            {
                File.WriteAllText(fileName, fileContents, Encoding.UTF8);
            }
        }
    }
}