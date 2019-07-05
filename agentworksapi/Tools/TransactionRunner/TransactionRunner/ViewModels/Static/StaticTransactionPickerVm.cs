using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    ///     Used to ensure single transaction picker viewmodel is created.  Can be injected into handlers, etc. to get current
    ///     values for selected parameters
    /// </summary>
    public static class StaticTransactionPickerVm
    {
        private static ITransactionPickerViewModel _vm;

        /// <summary>
        ///     Static view model for transaction picker.  Can be injected
        /// </summary>
        public static ITransactionPickerViewModel TransactionPickerViewModel =>
            _vm ?? (_vm = new TransactionPickerViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}