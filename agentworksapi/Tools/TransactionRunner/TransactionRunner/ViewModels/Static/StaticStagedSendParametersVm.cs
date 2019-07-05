using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    ///     Used to ensure single send parameters viewmodel is created.  Can be injected into handlers, etc. to get current
    ///     values for selected parameters
    /// </summary>
    public static class StaticStagedSendParametersVm
    {
        private static ISendParametersViewModel _vm;

        /// <summary>
        ///     Get static viewmodel
        /// </summary>
        public static ISendParametersViewModel StagedSendParametersViewModel =>
            _vm ?? (_vm = new SendParametersViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus, StaticTransactionNames.StagedSend));
    }
}