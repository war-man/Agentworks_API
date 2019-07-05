using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for ExportResultsControl.xaml
    /// </summary>
    public partial class ExportResultsControl
    {
        /// <summary>
        /// Creates ExportResultsControl instance
        /// </summary>
        public ExportResultsControl()
        {
            InitializeComponent();
            DataContext = StaticExportResultsViewModel.ExportResultsViewModel;
        }
    }
}