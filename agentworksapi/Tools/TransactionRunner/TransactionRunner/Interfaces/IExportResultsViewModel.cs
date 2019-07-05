using TransactionRunner.ViewModels;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    /// Interface for export results view model
    /// </summary>
    public interface IExportResultsViewModel : IPopupViewModel
    {
        /// <summary>
        /// Initializes view model instance
        /// </summary>
        /// <param name="filePath"></param>
        void Initialize(string filePath);

        /// <summary>
        /// Full file path of saved file
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Directory path of saved file
        /// </summary>
        string DirectoryPath { get; }
    }
}