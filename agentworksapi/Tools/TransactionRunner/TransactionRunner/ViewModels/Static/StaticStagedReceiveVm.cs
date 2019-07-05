using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    public static class StaticStagedReceiveVm
    {
        private static IStagedReceiveViewModel _vm;

        public static IStagedReceiveViewModel StagedReceiveViewModel =>
            _vm ?? (_vm = new StagedReceiveViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}