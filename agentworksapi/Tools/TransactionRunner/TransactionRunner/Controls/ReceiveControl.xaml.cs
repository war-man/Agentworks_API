using System;
using System.Windows.Controls;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for ReceiveControl.xaml
    /// </summary>
    public partial class ReceiveControl : UserControl
    {
        private IReceiveViewModel _vm;
        public ReceiveControl()
        {
            InitializeComponent();
        }

        private void ReceiveControl_Initialized(object sender, EventArgs e)
        {
            _vm = StaticReceiveVm.ReceiveViewModel;
            _vm.Initialize();
            DataContext = _vm;
        }
    }
}