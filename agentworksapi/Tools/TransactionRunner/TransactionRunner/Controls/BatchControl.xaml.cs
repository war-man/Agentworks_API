using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for BatchControl.xaml
    /// </summary>
    public partial class BatchControl : UserControl
    {
        private IBatchNumberViewModel _vm;
        public BatchControl()
        {
            InitializeComponent();
        }

        private void BatchControl_Initialized(object sender, System.EventArgs e)
        {
            _vm = StaticBatchNumberVm.BatchNumberViewModel;
            _vm.Initialize();
            DataContext = _vm;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}