using TransactionRunner.ViewModels;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    /// Interface for imported transaction batch results
    /// </summary>
    public interface IImportResultsViewModel : IPopupViewModel
    {
        /// <summary>
        /// Initializes view model instance
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="errorsCount"></param>
        void Initialize(string directoryPath, int errorsCount);

        /// <summary>
        /// Directory path of saved transaction batch results
        /// </summary>
        string DirectoryPath { get; }
    }
}