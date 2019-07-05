using TransactionRunner.Interfaces;

namespace TransactionRunner.ViewModels.Static
{
    public class StaticExportResultsViewModel
    {
        private static IExportResultsViewModel _vm;

        public static IExportResultsViewModel ExportResultsViewModel =>
            _vm ?? (_vm = new ExportResultsViewModel());
    }
}
