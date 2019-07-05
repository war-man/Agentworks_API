using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TransactionRunner.Helpers;
using TransactionRunner.ImportExport;
using TransactionRunner.Interfaces;
using TransactionRunner.Settings;
using TransactionRunner.Transactions;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     ViewModel for TransactionRunner
    /// </summary>
    public class MainWindowViewModel : IMainWindowViewModel
    {
        /// <summary>
        ///     Version
        /// </summary>
        public string AppVersion => TrVersion.Version;

        /// <summary>
        /// Application startup mode
        /// </summary>
        public AppStartupMode StartupMode { get; set; } = AppStartupMode.Normal;

        /// <summary>
        ///     Initialize
        /// </summary>
        public void Initialize()
        {
            if (StartupMode == AppStartupMode.Import)
            {
                Import();
            }

            StartupMode = AppStartupMode.Normal;
        }

        /// <summary>
        ///     Called before the main window closes
        /// </summary>
        /// <param name="sender">passed from event handler</param>
        /// <param name="e">passed from event handler</param>
        public void OnClosed(object sender, EventArgs e)
        {
            StaticSettings.SettingsSvc.SaveViewModelSettings();
        }

        /// <summary>
        /// Command for Export menu item (bound to View)
        /// </summary>
        public ICommand ExportCommand
        {
            get
            {
                return new RelayCommand(x => Export(), x => CanExport);
            }
        }

        private void Export()
        {
            var dialog = FileDialogHelper.CreateExportDialog();
            if (dialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                ExportFileFormat format = (ExportFileFormat) dialog.FilterIndex;
                StaticSettings.SettingsSvc.UserSettings.FileFormat = format;

                ExportMode? mode = ExportMode.Overwrite;
                if (File.Exists(dialog.FileName))
                {
                    mode = PopupWindow.ShowDialogWithResult(new OverwriteOrAppendViewModel(), x => x.Result);
                    if (mode == null)
                    {
                        return;
                    }
                }

                var result = StaticImportExport.ImportExportSvc.Export(
                    new List<BaseParams>
                    {
                        StaticTransactionPickerVm.TransactionPickerViewModel.SelectedTransaction.BuildParams
                    },
                    dialog.FileName, mode.Value, false);
                if (result == ImportExportSvc.ExportResult.Success)
                {
                    StaticExportResultsViewModel.ExportResultsViewModel.Initialize(dialog.FileName);
                    PopupWindow.ShowDialog(StaticExportResultsViewModel.ExportResultsViewModel);
                }
                else if(result == ImportExportSvc.ExportResult.ParameterTypeMismatch)
                {
                    MessageBox.Show("Cannot append to selected file because of transaction parameters set mismatch.",
                        "Cannot append", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Expression to control Export menu item availability (used by WPF command engine)
        /// </summary>
        public bool CanExport => StaticTransactionPickerVm.TransactionPickerViewModel?.SelectedTransaction != null;

        /// <summary>
        /// Command for Import menu item (bound to View)
        /// </summary>
        public ICommand ImportCommand => new RelayCommand(x => Import());

        private void Import()
        {
            PopupWindow.ShowDialog(StaticImportVm.ImportViewModel, new Size(1000, 600));
        }
    }
}