using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using TransactionRunner.ConfigProviders;
using TransactionRunner.Controls;
using TransactionRunner.Helpers;
using TransactionRunner.ImportExport;
using TransactionRunner.Interfaces;
using TransactionRunner.Models;
using TransactionRunner.Transactions;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    /// ViewModel for OpenFile window
    /// </summary>
    [Template(typeof(ImportControl))]
    public class ImportViewModel : PopupViewModelBase, IImportViewModel
    {
        private ObservableCollection<BaseParams> _transactions;
        private BaseParams _selectedTransaction;
        private double _currentProgress;
        private string _fileName;
        private bool _isBusy;
        private ResultWriter _writer;
        private string _openedFileName;

        /// <summary>
        /// Constructs new viewModel instance
        /// </summary>
        /// <param name="messageBus"></param>
        public ImportViewModel(IMessageBus messageBus)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]))
            {
                _fileName = args[1];
            }

            messageBus.Subscribe<TransactionBatchProgressEvent>(HandleTransactionBatchProgress);
            messageBus.Subscribe<TransactionResultsReceivedMessage>(HandleTransactionResultsReceived);
            messageBus.Subscribe<TransactionStartedEvent>(HandleTransactionStarted);
            messageBus.Subscribe<DisableStopButtonEvent>(HandleDisableStopButton);
            messageBus.Subscribe<TransactionUnrecoverableErrorEvent>(HandleTransactionUnrecoverableError);
        }

        private string OpenedFileName
        {
            get => _openedFileName;
            set
            {
                _openedFileName = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private void HandleTransactionUnrecoverableError(TransactionUnrecoverableErrorEvent message)
        {
            if (message.Mode != TransactionQueueMode.Import)
            {
                return;
            }

            InvokeOnUIThread(() =>
            {
                var transaction = Transactions.Single(x => x.TransactionGuid == message.Guid);
                transaction.ReferenceNumber = "ERROR";
                transaction.ResultFilePath = null;
                transaction.IsBusy = false;
                transaction.ShowResultText = true;
                ++ErrorsCount;
                MessageBox.Show($"Unrecoverable error occured during transaction {message.Exception?.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private void HandleTransactionStarted(TransactionStartedEvent message)
        {
            if (message.Mode != TransactionQueueMode.Import)
            {
                return;
            }

            InvokeOnUIThread(() =>
            {
                Transactions.Single(x => x.TransactionGuid == message.Guid).IsBusy = true;
            });
        }

        private void HandleTransactionResultsReceived(TransactionResultsReceivedMessage message)
        {
            if (message.Mode != TransactionQueueMode.Import)
            {
                return;
            }

            InvokeOnUIThread(() =>
            {
                var transaction = Transactions.Single(x => x.TransactionGuid == message.Guid);
                transaction.ReferenceNumber = message.ReferenceNumber;
                transaction.IsBusy = false;
                transaction.ResultFilePath = _writer.SaveResultsToDisk(message.DetailsString, message.ReferenceNumber);

                if (message.IsError)
                {
                    ++ErrorsCount;
                }

                if (IsStopped || Transactions.Where(x => x.IsEnabled).All(x => x.ReferenceNumber != null))
                {
                    HandleBatchResult();
                }
            });
        }

        private void HandleDisableStopButton(DisableStopButtonEvent message)
        {
            IsLast = true;
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Title of Window
        /// </summary>
        public override string Title => string.IsNullOrEmpty(OpenedFileName) ? "Import" :$"Import [{OpenedFileName}]";

        /// <summary>
        /// List of currently loaded transactions
        /// </summary>
        public ObservableCollection<BaseParams> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                RaisePropertyChanged(nameof(Transactions));
            }
        }

        /// <summary>
        /// Bound selected item on list
        /// </summary>
        public BaseParams SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                RaisePropertyChanged(nameof(SelectedTransaction));
            }
        }

        /// <summary>
        /// Command for Open file button
        /// </summary>
        public RelayCommand OpenFileCommand => new RelayCommand(x => OpenFile(), x => CanOpenFile);

        /// <summary>
        /// Initializes view model for its use
        /// </summary>
        public override void Initialize()
        {
            Transactions = new ObservableCollection<BaseParams>();
            OpenedFileName = null;
            OpenFile();
        }

        private void OpenFile()
        {
            string fileName = _fileName;
            _fileName = null;

            OpenFileDialog dialog = FileDialogHelper.CreateImportDialog();
            if (fileName == null && dialog.ShowDialog().GetValueOrDefault())
            {
                fileName = dialog.FileName;
            }

            if (fileName != null)
            {
                OpenedFileName = fileName;
                try
                {
                    Transactions =
                        new ObservableCollection<BaseParams>(StaticImportExport.ImportExportSvc.Load(fileName));
                }
                catch
                {
                    Transactions = new ObservableCollection<BaseParams>();
                    MessageBox.Show(
                        "Could not load any transactions from selected file. Please check if file is valid.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                SetIsSelectable(true);
            }
        }

        private bool CanOpenFile => !IsBusy;

        /// <summary>
        /// Command for Move up command
        /// </summary>
        public ICommand MoveUpCommand => new RelayCommand(x => MoveSelected(-1), x => CanMoveUp);

        private bool CanMoveUp => SelectedTransaction != null && Transactions.IndexOf(SelectedTransaction) > 0 && !IsBusy;

        /// <summary>
        /// Command for Move down command
        /// </summary>
        public ICommand MoveDownCommand => new RelayCommand(x => MoveSelected(1), x => CanMoveDown);

        private bool CanMoveDown => SelectedTransaction != null && Transactions.IndexOf(SelectedTransaction) < Transactions.Count - 1 && !IsBusy;

        private void MoveSelected(int direction)
        {
            int index = Transactions.IndexOf(SelectedTransaction);
            var transaction = SelectedTransaction;
            Transactions.RemoveAt(index);
            Transactions.Insert(index + direction, transaction);
            SelectedTransaction = transaction;
        }

        /// <summary>
        /// Current progress of batch run
        /// </summary>
        public double CurrentProgress
        {
            get => _currentProgress;
            set
            {
                _currentProgress = value;
                RaisePropertyChanged(nameof(CurrentProgress));
            }
        }

        private int ErrorsCount { get; set; }

        private void HandleTransactionBatchProgress(TransactionBatchProgressEvent progressEvent)
        {
            CurrentProgress = progressEvent.Progress;
        }

        /// <summary>
        /// Command for Run batch button
        /// </summary>
        public ICommand RunBatchCommand => new RelayCommand(x => RunBatch(), x => CanRunBatch);

        private void RunBatch()
        {
            _writer = new ResultWriter();

            ErrorsCount = 0;
            IsStopped = false;
            IsLast = false;
            IsBusy = true;
            SetIsSelectable(false);
            StaticTaskCancellation.TaskCancellation.ResetCancellation();

            ITransactionQueue queue = StaticTransactionQueue.TransactionQueue;
            queue.Reset();

            foreach (BaseParams transaction in Transactions.Where(x => x.IsEnabled))
            {
                transaction.IsBusy = false;
                transaction.ReferenceNumber = null;
                transaction.ShowResultText = false;

                var handler =
                    TransactionProvider.TransactionHandlers.FirstOrDefault(x => x.Display == transaction.TransactionName);
                transaction.TransactionGuid = queue.Add(handler, transaction);
            }

            queue.Run(TransactionQueueMode.Import).ContinueWith(x => InvokeOnUIThread(() =>
            {
                IsBusy = false;
            }));
        }

        private void HandleBatchResult()
        {
            SetIsSelectable(true);

            string fileName = Path.GetFileNameWithoutExtension(OpenedFileName);
            string extension = Path.GetExtension(OpenedFileName);
            string resultFileName = Path.Combine(_writer.BatchFolderPath, $"{fileName}_result{extension}");

            StaticImportExport.ImportExportSvc.Export(Transactions, resultFileName, ExportMode.Overwrite, true);

            StaticImportResultsViewModel.ImportResultsViewModel.Initialize(resultFileName, ErrorsCount);
            PopupWindow.ShowDialog(StaticImportResultsViewModel.ImportResultsViewModel);
        }

        private void SetIsSelectable(bool value)
        {
            if (Transactions != null)
            {
                foreach (BaseParams transaction in Transactions)
                {
                    transaction.IsSelectable = value;
                }
            }
        }

        private bool CanRunBatch => Transactions != null && Transactions.Any(x => x.IsEnabled) && !IsBusy;

        /// <summary>
        /// True when the window is in processing state
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Command for opening result file for single transaction
        /// </summary>
        public RelayCommand OpenResultCommand => new RelayCommand(x => OpenResult(x as string));

        private void OpenResult(string filePath)
        {
            FileHelper.OpenFile(filePath);
        }

        /// <summary>
        /// Command bound to Stop button
        /// </summary>
        public ICommand StopBatchCommand => new RelayCommand(x => StopBatch(), x => CanStopBatch);

        private void StopBatch()
        {
            StaticTaskCancellation.TaskCancellation.Cancel();
            IsStopped = true;
        }

        private bool CanStopBatch => IsBusy && !IsStopped && !IsLast;

        private bool IsStopped { get; set; }

        private bool IsLast { get; set; }

        /// <summary>
        /// Called before closing the window
        /// </summary>
        /// <returns></returns>
        public override bool CanClose()
        {
            if (IsBusy)
            {
                MessageBox.Show(
                    "Cannot close window when transactions are running. Please stop the batch and try again.",
                    "Cannot close", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}