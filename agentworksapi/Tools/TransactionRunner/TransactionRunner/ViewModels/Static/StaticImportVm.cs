using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    /// Static Import view model
    /// </summary>
    public class StaticImportVm
    {
        private static IImportViewModel _vm;

        /// <summary>
        /// Static import view model instance
        /// </summary>
        public static IImportViewModel ImportViewModel =>
            _vm ?? (_vm = new ImportViewModel(StaticMessageBus.MessageBus));
    }
}