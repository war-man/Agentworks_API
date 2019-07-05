using TransactionRunner.Interfaces;

namespace TransactionRunner.ViewModels.Static
{
    public class StaticImportResultsViewModel
    {
        private static IImportResultsViewModel _vm;

        public static IImportResultsViewModel ImportResultsViewModel =>
            _vm ?? (_vm = new ImportResultsViewModel());
    }
}