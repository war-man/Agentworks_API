using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using TransactionRunner.Interfaces;
using TransactionRunner.Timer;
using TransactionRunner.Transactions;
using TransactionRunner.Helpers;
using System.Linq;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     Viewmodel for the results pane of the application
    /// </summary>
    public class ResultsPaneViewModel : INotifyPropertyChanged, IResultsPaneViewModel
    {
        private readonly IMessageBus _messageBus;
        private StringBuilder _stringBuilder;

        /// <summary>
        ///     The Timer view model.
        /// </summary>
        public IElapsedTimerViewModel ElapsedTimerViewModel { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="messageBus"></param>
        public ResultsPaneViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _stringBuilder = new StringBuilder();
            TransactionOutputList = new ObservableCollection<OutputViewModel>();
            ElapsedTimerViewModel = new ElapsedTimerViewModel(_messageBus);
        }

        /// <summary>
        ///     Raises events when property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _referenceNumber;

        /// <summary>
        ///     Property to access or set Reference number on the UI.  Raises property changed event
        /// </summary>
        public string ReferenceNumber
        {
            get { return _referenceNumber; }
            set
            {
                _referenceNumber = value;
                RaisePropertyChanged(nameof(ReferenceNumber));
            }
        }

        private ObservableCollection<OutputViewModel> _transactionOutputList;

        /// <summary>
        ///     The output from the execution of transactions.
        /// </summary>
        public ObservableCollection<OutputViewModel> TransactionOutputList
        {
            get { return _transactionOutputList; }
            set
            {
                _transactionOutputList = value;
                RaisePropertyChanged(nameof(TransactionOutputList));
            }
        }

        private string _timeTaken;
        private ResultWriter _writer;

        /// <summary>
        ///     Property to access or set Time Taken on the ui.  Raises propery changed event.
        /// </summary>
        public string TimeTaken
        {
            get { return _timeTaken; }
            set
            {
                _timeTaken = value;
                RaisePropertyChanged(nameof(TimeTaken));
            }
        }

        /// <summary>
        ///     Initialize this viewmodel
        /// </summary>
        public void Initialize()
        {
            _messageBus.Subscribe<TransactionResultsReceivedMessage>(ProcessResultsReceived);
            _messageBus.Subscribe<RunTransactionsButtonClicked>(ResetResultsOutput);
        }

        private void ProcessResultsReceived(TransactionResultsReceivedMessage obj)
        {
            if (obj.Mode != TransactionQueueMode.Batch)
            {
                return;
            }

            if (ReferenceNumber.Length == 0)
            {
                ReferenceNumber += obj.ReferenceNumber;
            }
            else
            {
                ReferenceNumber += "," + obj.ReferenceNumber;
            }
            _stringBuilder.Clear();
            _stringBuilder.AppendLine(obj.DetailsString);
            var savedPath = _writer.SaveResultsToDisk(_stringBuilder.ToString(), obj.ReferenceNumber);
            var outputVm = new OutputViewModel
            {
                Path = savedPath,
                PathForView = savedPath,
                ElapsedTimeSpan = ElapsedTimerViewModel.ElapsedTime
            };
            outputVm.ElapsedTime = CalculateElapsedTime(ElapsedTimerViewModel);
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                TransactionOutputList.Add(outputVm);
            });
        }

        private string CalculateElapsedTime(IElapsedTimerViewModel vm)
        {
            var displayTime = $"{vm.ElapsedTime.Minutes}:{vm.ElapsedTime.Seconds:00}:{vm.ElapsedTime.Milliseconds.ToString("000").Substring(0, 2)}";
            if (TransactionOutputList.Any())
            {
                var lastAdded = TransactionOutputList.Last();
                var recordedTimeSpan = lastAdded.ElapsedTimeSpan;
                var currentTimeSpan = vm.ElapsedTime;
                var subtractedTimeSpan = currentTimeSpan.Subtract(recordedTimeSpan);
                if (subtractedTimeSpan != TimeSpan.Zero)
                {
                    // We have a new time
                    displayTime = $"{subtractedTimeSpan.Minutes}:{subtractedTimeSpan.Seconds:00}:{subtractedTimeSpan.Milliseconds.ToString("000").Substring(0, 2)}";
                }
            }
            return displayTime;
        }
        private void ResetResultsOutput(RunTransactionsButtonClicked obj)
        {
            ReferenceNumber = string.Empty;
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                TransactionOutputList.Clear();
            });
            _stringBuilder.Clear();
            _writer = new ResultWriter();
        }
    }
}