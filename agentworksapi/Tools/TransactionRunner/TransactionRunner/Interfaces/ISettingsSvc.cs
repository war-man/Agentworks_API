using TransactionRunner.ViewModels;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     Handles getting and saving settings for the application
    /// </summary>
    public interface ISettingsSvc
    {
        /// <summary>
        ///     Property to retrieve user settings
        /// </summary>
        UserSettings UserSettings { get; }


	    /// <summary>
	    ///     Saves selected settings from provided view models
	    /// </summary>
	    /// <param name="agentSelector">agent selector view model</param>
	    /// <param name="receiveAgentSelector"></param>
	    /// <param name="sendParameters">send parameters view model</param>
	    /// <param name="sendReversalParameters"></param>
	    /// <param name="receiveParameters"></param>
	    /// <param name="transactionPicker">transaction picker view model</param>
	    /// <returns></returns>
	    bool SaveViewModelSettings();

        /// <summary>
        ///     Saves the current user settings.
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        ///     Initialize default settings and configuration files.
        /// </summary>
        void Initialize();
    }
}