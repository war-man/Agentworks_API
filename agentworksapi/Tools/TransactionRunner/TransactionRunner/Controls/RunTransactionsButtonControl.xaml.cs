using System;
using System.Windows;
using System.Windows.Controls;
using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.ViewModels;

namespace TransactionRunner.Controls
{
    /// <summary>
    ///     Interaction logic for SendTransactionControl.xaml
    /// </summary>
    public partial class RunTransactionsButtonControl : UserControl
    {
        private IRunTransactionsButtonViewModel _vm;

        public RunTransactionsButtonControl()
        {
            InitializeComponent();
        }
        
        private void SendClick(object sender, RoutedEventArgs e)
        {
            _vm.SendClick();
        }

        private void RunTransactionsButtonControl_Initialized(object sender, EventArgs e)
        {
            _vm = new RunTransactionsButtonViewModel(StaticMessageBus.MessageBus);
            _vm.Initialize();
            DataContext = _vm;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _vm.StopClick();
        }
    }
}