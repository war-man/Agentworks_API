using System.Windows.Controls;
using TransactionRunner.Interfaces;

namespace TransactionRunner.Controls
{
    /// <summary>
    ///     Interaction logic for SendParametersControl.xaml
    /// </summary>
    public partial class SendParametersControl : UserControl
    {
        private ISendParametersViewModel _viewModel;

        /// <summary>
        ///     Initiate a new Send Parameters Control.
        /// </summary>
        public SendParametersControl()
        {
            InitializeComponent();
        }

        public ISendParametersViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                _viewModel.Initialize();
                DataContext = value;
            }
        }
    }
}