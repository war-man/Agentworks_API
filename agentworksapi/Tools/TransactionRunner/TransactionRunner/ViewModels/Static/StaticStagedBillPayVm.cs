using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
	public static class StaticStagedBillPayVm
	{
		private static IStagedBillPayViewModel _vm;

		public static IStagedBillPayViewModel StagedBillPayViewModel =>
			_vm ?? (_vm = new StagedBillPayViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));

	}
}