using System.Windows.Controls;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for AgentManagementControl.xaml
    /// </summary>
    public partial class AgentManagementControl : UserControl
    {
        private readonly IAgentManagementViewModel _vm;

        public AgentManagementControl()
        {
            InitializeComponent();
            _vm = StaticAgentManagementVm.AddAgentViewModel;
        }

        private void AddAgentControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.Initialize();
            DataContext = _vm;
        }

        private void BtnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.Save();
        }

        private void DeleteAgent(object sender, System.Windows.RoutedEventArgs e)
        {
            var agent = ((Button)sender).DataContext as EnvironmentAgent;
            _vm.DeleteAgent(agent);
            
        }

        private void EditAgent(object sender, System.Windows.RoutedEventArgs e)
        {
            var agent = ((Button)sender).DataContext as EnvironmentAgent;
            _vm.EditAgent(agent);
        }

        private void BtnSaveEnvironment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.SaveEnvironment();
        }

        private void BtnDeleteEnvironment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var environment = ((Button)sender).DataContext as string;
            _vm.DeleteEnvironment(environment);
        }

        private void TxtEnvironmentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var environment = ((TextBox) sender).Text;
            _vm.IsEnvironmentValid = !string.IsNullOrEmpty(environment);
        }
    }
}