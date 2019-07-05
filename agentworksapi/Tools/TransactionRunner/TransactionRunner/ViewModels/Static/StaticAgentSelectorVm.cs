using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.Settings;

namespace TransactionRunner.ViewModels.Static
{
    /// <summary>
    ///     Used to ensure single AgentSelector viewmodel is created.  Can be injected into handlers, etc. to get current
    ///     values for selected parameters
    /// </summary>
    public class StaticAgentSelectorVm
    {
        private static IAgentSelectorViewModel _vm;
        /// <summary>
		///     Gets the static Agent Selector view model.
		/// </summary>
		public static IAgentSelectorViewModel AgentSelectorViewModel =>
            _vm ?? (_vm = new AgentSelectorViewModel(StaticSettings.SettingsSvc, StaticMessageBus.MessageBus));
    }
}