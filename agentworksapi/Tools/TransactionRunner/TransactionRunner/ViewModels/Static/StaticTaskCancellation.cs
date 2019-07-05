using TransactionRunner.Models;

namespace TransactionRunner.ViewModels.Static
{
    public static class StaticTaskCancellation
    {
        private static TaskCancellation _model;

        /// <summary>
        ///     Get static viewmodel
        /// </summary>
        public static TaskCancellation TaskCancellation =>
            _model ?? (_model = new TaskCancellation());
    }
}