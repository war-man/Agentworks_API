using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    ///     Static AddAgentViewModel.
    /// </summary>
    public static class StaticAgentManagementVm
    {
        private static IAgentManagementViewModel _vm;

        public static IAgentManagementViewModel AddAgentViewModel =>
            _vm ?? (_vm = new AgentManagementViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}