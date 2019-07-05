using TransactionRunner.Interfaces;

namespace TransactionRunner.ViewModels.Static
{
    public static class StaticBatchNumberVm
    {
        private static IBatchNumberViewModel _vm;

        /// <summary>
        ///     Get static viewmodel
        /// </summary>
        public static IBatchNumberViewModel BatchNumberViewModel =>
            _vm ?? (_vm = new BatchNumberViewModel());
    }
}