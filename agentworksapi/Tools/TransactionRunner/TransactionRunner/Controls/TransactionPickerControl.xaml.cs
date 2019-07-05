using System.Windows;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    public partial class TransactionPickerControl
    {
        private ITransactionPickerViewModel _vm;

        public TransactionPickerControl()
        {
            InitializeComponent();
        }

        private void TransactionPicker_Initialized(object sender, System.EventArgs e)
        {
            _vm = StaticTransactionPickerVm.TransactionPickerViewModel;
            _vm.Initialize();
            DataContext = _vm;
        }

        private void TransactionPickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.Load();
        }
    }
}