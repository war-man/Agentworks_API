using System.Collections.Generic;
using TransactionRunner.Transactions;

namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Interface for export/import file handlers
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// File format
        /// </summary>
        ExportFileFormat FileFormat { get; }

        /// <summary>
        /// Returns header for file
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        string GetHeader(BaseParams transactionParams, bool includeReferenceNumber);

        /// <summary>
        /// Returns data row for transaction parameters
        /// </summary>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        string GetData(IEnumerable<BaseParams> transactionParams, bool includeReferenceNumber);

        /// <summary>
        /// Appends criteria to existing file contents
        /// </summary>
        /// <param name="existingContents"></param>
        /// <param name="transactionParams"></param>
        /// <param name="includeReferenceNumber"></param>
        /// <returns></returns>
        string Append(string existingContents, IEnumerable<BaseParams> transactionParams, bool includeReferenceNumber);

        /// <summary>
        /// Loads file contents and returns transaction criteria
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        IEnumerable<BaseParams> LoadTransactions(string fileContents);
    }
}