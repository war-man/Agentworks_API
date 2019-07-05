using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    public static class StaticBillPayVm
    {
        private static IBillPayViewModel _vm;

        public static IBillPayViewModel BillPayViewModel =>
            _vm ?? (_vm = new BillPayViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}