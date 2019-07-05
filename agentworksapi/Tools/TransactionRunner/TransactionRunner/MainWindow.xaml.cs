using System;
using System.IO;
using System.Windows;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using TransactionRunner.Helpers;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels;

namespace TransactionRunner
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMainWindowViewModel _vm;

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Initializer.Initialize();
            _vm = new MainWindowViewModel();
        }

        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && File.Exists(args[1]))
            {
                _vm.StartupMode = AppStartupMode.Import;
            }

            _vm.Initialize();
            DataContext = _vm;
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            _vm.OnClosed(sender, eventArgs);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemManageAgents_Click(object sender, RoutedEventArgs e)
        {
            var agentManagementWindow = new AgentManagementWindow();
            agentManagementWindow.ShowDialog();
        }
    }
}