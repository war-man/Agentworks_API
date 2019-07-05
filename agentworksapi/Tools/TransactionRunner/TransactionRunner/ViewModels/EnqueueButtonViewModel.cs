using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     Enqueue Button View Model.
    /// </summary>
    public class EnqueueButtonViewModel : IEnqueueButtonViewModel
    {
        private readonly ITransactionQueue _transactionQueue;
        private readonly ITransactionPickerViewModel _transactionPicker;

        /// <summary>
        ///     Instantiate a new Enqueue Button View Model.
        /// </summary>
        /// <param name="transactionQueue"></param>
        /// <param name="transactionPicker"></param>
        public EnqueueButtonViewModel(ITransactionQueue transactionQueue, ITransactionPickerViewModel transactionPicker)
        {
            _transactionQueue = transactionQueue;
            _transactionPicker = transactionPicker;
        }

        /// <summary>
        ///     Initialize the vm
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        ///     called when Enqueue button is clicked.
        /// </summary>
        public void OnClick()
        {
            _transactionQueue.Add(_transactionPicker.SelectedTransaction);
        }
    }
}