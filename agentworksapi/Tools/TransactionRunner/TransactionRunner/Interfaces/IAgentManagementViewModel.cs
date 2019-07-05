using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     Interface for adding new agents.
    /// </summary>
    public interface IAgentManagementViewModel
    {
        /// <summary>
        ///     Initialize.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Saves the agent to the user's local settings.
        /// </summary>
        void Save();

        /// <summary>
        ///     Clears the selected values in the view model.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Edit Agent.
        /// </summary>
        /// <param name="agent"></param>
        void EditAgent(EnvironmentAgent agent);

        /// <summary>
        ///     Delete Agent.
        /// </summary>
        void DeleteAgent(EnvironmentAgent agent);

        /// <summary>
        ///     Saves the environment to the user's local settings.
        /// </summary>
        void SaveEnvironment();

        /// <summary>
        ///     Deletes the environment from the user's local settings.
        /// </summary>
        /// <param name="environment"></param>
        void DeleteEnvironment(string environment);

        /// <summary>
        ///     Gets the list of agents.
        /// </summary>
        Dictionary<string, List<EnvironmentAgent>> EnvironmentAgentList { get; }

        /// <summary>
        ///     Gets the list of environments.
        /// </summary>
        List<string> EnvironmentList { get; }

        /// <summary>
        ///     Get or sets the environment.
        /// </summary>
        string SelectedEnvironment { get; set; }

        /// <summary>
        ///     Gets the list of countries.
        /// </summary>
        List<CountryInfo> CountryList { get; }

        /// <summary>
        ///     Gets the selected country.
        /// </summary>
        CountryInfo SelectedCountry { get; set; }

        /// <summary>
        ///     Gets the list of country subdivisions.
        /// </summary>
        List<SubdivisionInfo> CountrySubdivisionList { get; }

        /// <summary>
        ///     Gets the selected country subdivision.
        /// </summary>
        SubdivisionInfo SelectedCountrySubdivision { get; set; }

        /// <summary>
        ///     Gets or sets the agent id.
        /// </summary>
        string AgentId { get; set; }

        /// <summary>
        ///     Get or sets the agent pos.
        /// </summary>
        string AgentPos { get; set; }

        /// <summary>
        ///     Gets or sets the agent password.
        /// </summary>
        string AgentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the language.
        /// </summary>
        string Language { get; set; }

        /// <summary>
        ///     Gets or sets the selected currency.
        /// </summary>
        CurrencyInfo SelectedCurrency { get; set; }

        /// <summary>
        ///     Gets the list of currencies.
        /// </summary>
        List<CurrencyInfo> CurrencyList { get; }

        /// <summary>
        ///     Determines if the form is valid to save the agent.
        /// </summary>
        bool IsAgentValid { get; set; }

        /// <summary>
        ///     Gets or sets the environment name.
        /// </summary>
        string EnvironmentName { get; set; }

        /// <summary>
        ///     Determines if the form is valid to save the environment.
        /// </summary>
        bool IsEnvironmentValid { get; set; }
    }
}