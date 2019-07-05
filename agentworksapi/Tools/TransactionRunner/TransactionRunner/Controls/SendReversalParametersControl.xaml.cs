using System.Windows.Controls;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    ///     Interaction logic for SendReversalParametersControl.xaml
    /// </summary>
    public partial class SendReversalParametersControl : UserControl
    {
        private ISendReversalParametersViewModel _vm;

        /// <summary>
        ///     Initiate a new Send Reversal Parameters Control.
        /// </summary>
        public SendReversalParametersControl()
        {
            InitializeComponent();
        }

        private void SendReversalParametersControl_Initialized(object sender, System.EventArgs e)
        {
            _vm = StaticSendReversalParametersVm.SendReversalParametersViewModel;
            _vm.Initialize();
            DataContext = _vm;
        }
    }
}