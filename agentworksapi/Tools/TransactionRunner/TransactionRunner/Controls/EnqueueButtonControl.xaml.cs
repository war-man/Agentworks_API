using System.Windows;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using TransactionRunner.ViewModels;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    public partial class EnqueueButtonControl
    {
        private readonly IEnqueueButtonViewModel _vm;

        public EnqueueButtonControl()
        {
            InitializeComponent();
            _vm = new EnqueueButtonViewModel(StaticTransactionQueue.TransactionQueue,
                StaticTransactionPickerVm.TransactionPickerViewModel);
        }

        private void BtnSend_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.OnClick();
        }

        private void EnqueueButtonControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm.Initialize();
        }
    }
}