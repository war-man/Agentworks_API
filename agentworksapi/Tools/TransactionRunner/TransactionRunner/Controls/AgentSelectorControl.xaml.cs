using System.Windows;
using System.Windows.Controls;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    ///     Interaction logic for AgentSelectorControl.xaml
    /// </summary>
    public partial class AgentSelectorControl : UserControl
    {
        private readonly IAgentSelectorViewModel _vm;

        /// <summary>
        ///     Constructor
        /// </summary>
        public AgentSelectorControl()
        {
            InitializeComponent();
            _vm = StaticAgentSelectorVm.AgentSelectorViewModel;
        }

        private void AgentSelectorControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm.Initialize();
            DataContext = _vm;
        }

        private void BtnManageAgents_OnClick(object sender, RoutedEventArgs e)
        {
            var agentManagementWindow = new AgentManagementWindow();
            agentManagementWindow.ShowDialog();
        }
    }
}