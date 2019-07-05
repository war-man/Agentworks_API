using System.IO;
using Microsoft.Win32;
using TransactionRunner.Settings;

namespace TransactionRunner.Helpers
{
    /// <summary>
    /// Helper for Import/Export file dialogs
    /// </summary>
    public static class FileDialogHelper
    {
        private const string DialogFilter = "CSV Files|*.csv|XML Files|*.xml";

        /// <summary>
        /// Creates dialog for Export
        /// </summary>
        /// <returns></returns>
        public static SaveFileDialog CreateExportDialog()
        {
            return new SaveFileDialog
            {
                AddExtension = true,
                Filter = DialogFilter,
                InitialDirectory = GetInitialDirectory(),
                OverwritePrompt = false,  //using custom overwrite prompt
                FilterIndex = (int)StaticSettings.SettingsSvc.UserSettings.FileFormat
            };
        }

        /// <summary>
        /// Creates dialog for Import
        /// </summary>
        /// <returns></returns>
        public static OpenFileDialog CreateImportDialog()
        {
            return new OpenFileDialog
            {
                AddExtension = true,
                Filter = DialogFilter,
                InitialDirectory = GetInitialDirectory(),
                FilterIndex = (int)StaticSettings.SettingsSvc.UserSettings.FileFormat
            };
        }

        private static string GetInitialDirectory()
        {
            return Directory.Exists(StaticSettings.SettingsSvc.UserSettings.LastUsedDirectory)
                ? StaticSettings.SettingsSvc.UserSettings.LastUsedDirectory
                : Directory.GetCurrentDirectory();
        }
    }
}