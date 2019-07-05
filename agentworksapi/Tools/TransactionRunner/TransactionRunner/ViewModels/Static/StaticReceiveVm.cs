using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    public static class StaticReceiveVm
    {
        private static IReceiveViewModel _vm;

        public static IReceiveViewModel ReceiveViewModel =>
            _vm ?? (_vm = new ReceiveViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}