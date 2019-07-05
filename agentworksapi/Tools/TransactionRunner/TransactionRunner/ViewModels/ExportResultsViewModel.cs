using System.IO;
using TransactionRunner.Controls;
using TransactionRunner.Helpers;
using TransactionRunner.Interfaces;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    /// View model for export results window
    /// </summary>
    [Template(typeof(ExportResultsControl))]
    public class ExportResultsViewModel : PopupViewModelBase, IExportResultsViewModel
    {
        /// <summary>
        /// Returns popup title
        /// </summary>
        public override string Title => "Export results";

        private string _filePath;
        private string _directoryPath;

        /// <summary>
        /// Initializes view model
        /// </summary>
        /// <param name="filePath"></param>
        public void Initialize(string filePath)
        {
            FilePath = filePath;
            DirectoryPath = new FileInfo(FilePath).DirectoryName + "\\";

            StaticSettings.SettingsSvc.UserSettings.LastUsedDirectory = DirectoryPath;
        }

        /// <summary>
        /// File path
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            private set
            {
                _filePath = value;
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        /// <summary>
        /// Directory path
        /// </summary>
        public string DirectoryPath
        {
            get => _directoryPath;
            private set
            {
                _directoryPath = value;
                RaisePropertyChanged(nameof(DirectoryPath));
            }
        }

        /// <summary>
        /// Open directory command
        /// </summary>
        public RelayCommand OpenDirectoryCommand => new RelayCommand(x => FileHelper.OpenDirectory(DirectoryPath));

        /// <summary>
        /// Open file command
        /// </summary>
        public RelayCommand OpenFileCommand => new RelayCommand(x => FileHelper.OpenFile(FilePath));

        /// <summary>
        /// Command to close dialog
        /// </summary>
        public RelayCommand CloseCommand => new RelayCommand(x => OnRequestClose());
    }
}