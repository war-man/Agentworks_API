using System.Threading.Tasks;
using TransactionRunner.Interfaces;
using TransactionRunner.Models;
using TransactionRunner.Timer;
using TransactionRunner.Transactions;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     Run Transactions Button View Model.
    /// </summary>
    public class RunTransactionsButtonViewModel : BaseAgentViewModel, IRunTransactionsButtonViewModel
    {
        private const string SendLabelDefaultText = "Run";
        private const string SendLabelSendingText = "Running...";
        private const string StopLabelDefaultText = "Stop Batch";
        private const string StopLabelText = "Stopping...";

        private readonly IMessageBus _messageBus;

        /// <summary>
        ///     Instantiate a new Run Transactions Button View Model.
        /// </summary>
        /// <param name="messageBus"></param>
        public RunTransactionsButtonViewModel(IMessageBus messageBus) 
            : base(messageBus)
        {
            _messageBus = messageBus;
        }

        /// <summary>
        ///     Gets or sets text on the run button.
        /// </summary>
        private string _runButtonContent;
        public string RunButtonContent
        {
            get { return _runButtonContent; }
            set
            {
                _runButtonContent = value;
                RaisePropertyChanged(nameof(RunButtonContent));
            }
        }

        private string _stopButtonContent;
        public string StopButtonContent
        {
            get { return _stopButtonContent; }
            set
            {
                _stopButtonContent = value;
                RaisePropertyChanged(nameof(StopButtonContent));
            }
        }

        private bool _stopButtonEnabled;
        public bool StopButtonEnabled
        {
            get { return _stopButtonEnabled; }
            set
            {
                _stopButtonEnabled = value;
                RaisePropertyChanged(nameof(StopButtonEnabled));
            }
        }

        private bool _runButtonEnabled;
        public bool RunButtonEnabled
        {
            get { return _runButtonEnabled; }
            set
            {
                _runButtonEnabled = value;
                RaisePropertyChanged(nameof(RunButtonEnabled));
            }
        }

        public void StopClick()
        {
            StopButtonContent = StopLabelText;
            StaticTaskCancellation.TaskCancellation.Cancel();
        }

        public async Task SendClick()
        {
            StaticModal.ShowModal();
            PrepareBeforeRun();
            _messageBus.Subscribe<TransactionComplete>(ResetAfterRun);
            _messageBus.Subscribe<DisableStopButtonEvent>(CancellationDisable);
            _messageBus.Publish(new RunTransactionsButtonClicked());
        }
        public void CancellationDisable(DisableStopButtonEvent e)
        {
            // Disable Cancellation button.
            StopButtonEnabled = false;
        }

        public new void Initialize()
        {
            RunButtonContent = SendLabelDefaultText;
            StopButtonContent = StopLabelDefaultText;
            RunButtonEnabled = true;
            StopButtonEnabled = false;
            base.Initialize();
        }

        /// <summary>
        ///     Starts and resets the timers and changes the text on the "send" button.
        /// </summary>
        private void PrepareBeforeRun()
        {
            RunButtonContent = SendLabelSendingText;
            StopButtonEnabled = true;
            RunButtonEnabled = false;
            _messageBus.Publish(new TimerStartEvent());
        }


        /// <summary>
        ///     The opposite of PrepareBeforeSend; stops the timers and resets the text on the send button.
        /// </summary>
        private void ResetAfterRun(TransactionComplete evtComplete)
        {
            RunButtonContent = SendLabelDefaultText;
            StopButtonContent = StopLabelDefaultText;
            StopButtonEnabled = false;
            RunButtonEnabled = true;
            StaticTaskCancellation.TaskCancellation.ResetCancellation();
            StaticModal.HideModal();
            _messageBus.Publish(new TimerStopEvent());
        }
    }
}