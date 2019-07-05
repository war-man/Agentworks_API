using System;
using System.IO;
using System.Reflection;

namespace TransactionRunner.Helpers
{
    /// <summary>
    /// Writer for transaction results
    /// </summary>
    public class ResultWriter
    {
        /// <summary>
        /// Folder name 
        /// </summary>
        public string BatchFolderPath { get; }

        /// <summary>
        /// Creates new instance. Initializes batch folder name.
        /// </summary>
        public ResultWriter()
        {
            string executingDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            string batchFolderName = DateTime.Now.ToString("s").Replace(":", ".");
            BatchFolderPath = Path.Combine(executingDir, "Output", batchFolderName);
        }

        /// <summary>
        /// Saves results to file in appropriate directory
        /// </summary>
        /// <param name="data"></param>
        /// <param name="referenceNumber"></param>
        /// <returns></returns>
        public string SaveResultsToDisk(string data, string referenceNumber)
        {
            string filePath = Path.Combine(BatchFolderPath, $"{referenceNumber}.txt");
            return FileHelper.SaveOutputData(data, filePath);
        }
    }
}