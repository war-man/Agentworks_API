using System;
using System.Windows.Controls;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for StagedReceiveControl.xaml
    /// </summary>
    public partial class StagedReceiveControl : UserControl
    {
        private IStagedReceiveViewModel _vm;
        public StagedReceiveControl()
        {
            InitializeComponent();
        }

        private void StagedReceiveControl_Initialized(object sender, EventArgs e)
        {
            _vm = StaticStagedReceiveVm.StagedReceiveViewModel;
            _vm.Initialize();
            DataContext = _vm;
        }
    }
}
