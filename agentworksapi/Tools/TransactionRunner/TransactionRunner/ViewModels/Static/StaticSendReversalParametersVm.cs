using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    ///     Static Send Reversal Parameters View Model
    /// </summary>
    public static class StaticSendReversalParametersVm
    {
        private static ISendReversalParametersViewModel _vm;

        /// <summary>
        ///     Gets the static view model.
        /// </summary>
        public static ISendReversalParametersViewModel SendReversalParametersViewModel =>
            _vm ?? (_vm = new SendReversalParametersViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}