using System.Diagnostics;
using System.Windows;
using TransactionRunner.Helpers;

namespace TransactionRunner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            TemplateManager.RegisterTemplates();

            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If ther is more than one, than it is already running.
                MessageBox.Show("Application is already running.");
                Application.Current.Shutdown();
                System.Environment.Exit(0);
                return;
            }

            base.OnStartup(e);
        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = $"An unexpected error has occurred: {e.Exception.Message}. Please retry.";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            #if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it

                e.Handled = false;

            #else

                e.Handled = true; 

            #endif
        }
    }
}