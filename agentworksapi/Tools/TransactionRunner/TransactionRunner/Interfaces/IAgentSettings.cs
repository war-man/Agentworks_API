namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     Interface for managing agents in the user's local storage.
    /// </summary>
    public interface IAgentSettings
    {
        /// <summary>
        ///     Adds a new agent to the user's settings file.
        /// </summary>
        /// <param name="agentVm"></param>
        void AddAgent(IAgentManagementViewModel agentVm);

        /// <summary>
        ///     Gets an existing agent from the user's settings file.
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="agentId"></param>
        /// <param name="agentPos"></param>
        /// <returns></returns>
        IAgentManagementViewModel GetAgent(string environment, string agentId, string agentPos);

        /// <summary>
        ///     Updates an agent in the user's settings file.
        /// </summary>
        /// <param name="oldAgentVm"></param>
        /// <param name="newAgentVm"></param>
        void UpdateAgent(IAgentManagementViewModel oldAgentVm, IAgentManagementViewModel newAgentVm);

        /// <summary>
        ///     Deletes an agent from the user's settings file.
        /// </summary>
        /// <param name="agentVm"></param>
        void DeleteAgent(IAgentManagementViewModel agentVm);
    }
}