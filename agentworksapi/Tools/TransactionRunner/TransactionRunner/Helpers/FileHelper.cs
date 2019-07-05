using System.Diagnostics;
using System.IO;

namespace TransactionRunner.Helpers
{
    public static class FileHelper
    {
        public static string SaveOutputData(string data, string fullPath, bool checkIfFileExists = true)
        {
            int count = 1;

            var path = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Path.GetFileNameWithoutExtension(fullPath);
            var extension = Path.GetExtension(fullPath);
            while (checkIfFileExists && File.Exists(fullPath))
            {
                string tempFileName = string.Format("{0}({1})", filename, count++);
                fullPath = Path.Combine(path, tempFileName + extension);
            }
            
            File.WriteAllText(fullPath, data);
            return fullPath;
        }

        public static void OpenDirectory(string fullPath)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.GetDirectoryName(fullPath),
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public static void OpenFile(string fullPath)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = fullPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
    }
}