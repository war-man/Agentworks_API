using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for ImportResultsControl.xaml
    /// </summary>
    public partial class ImportResultsControl
    {
        /// <summary>
        /// Initializes class for view
        /// </summary>
        public ImportResultsControl()
        {
            InitializeComponent();
            DataContext = StaticImportResultsViewModel.ImportResultsViewModel;
        }
    }
}