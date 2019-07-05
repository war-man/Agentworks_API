using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using TransactionRunner.Interfaces;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     Extension methods for transaction runner view models.
    /// </summary>
    public static class ViewModelExtensions
    {
        /// <summary>
        ///     Converts an IAddAgentViewModel to an EnvironmentAgent.
        /// </summary>
        /// <param name="agentManagementViewModel"></param>
        /// <returns></returns>
        public static EnvironmentAgent ToEnvironmentAgent(this IAgentManagementViewModel agentManagementViewModel)
        {
            var environmentAgent = new EnvironmentAgent
            {
                Environment = agentManagementViewModel.SelectedEnvironment,
                AgentCountryIsoCode = agentManagementViewModel.SelectedCountry?.CountryCode,
                AgentCountry = agentManagementViewModel.SelectedCountry?.CountryName,
                AgentStateCode = agentManagementViewModel.SelectedCountrySubdivision?.CountrySubdivisionCode,
                AgentState = agentManagementViewModel.SelectedCountrySubdivision?.CountrySubdivisionName,
                AgentId = agentManagementViewModel.AgentId,
                AgentSequence = agentManagementViewModel.AgentPos,
                AgentPassword = agentManagementViewModel.AgentPassword,
                Language = agentManagementViewModel.Language,
                SendCurrencies = new List<string> {agentManagementViewModel.SelectedCurrency.CurrencyCode}
            };
            return environmentAgent;
        }

        /// <summary>
        ///     Converts and EnnvironmentAgent to an IAddAgentViewModel.
        /// </summary>
        /// <param name="environmentAgent"></param>
        /// <returns></returns>
        public static IAgentManagementViewModel ToAddAgentViewModel(this EnvironmentAgent environmentAgent)
        {
            var agentManagementViewModel = new AgentManagementViewModel
            {
                SelectedEnvironment = environmentAgent.Environment,
                SelectedCountry = new CountryInfo
                {
                    CountryCode = environmentAgent.AgentCountryIsoCode,
                    CountryName = environmentAgent.AgentCountry
                },
                SelectedCountrySubdivision = new SubdivisionInfo
                {
                    CountrySubdivisionCode = environmentAgent.AgentStateCode,
                    CountrySubdivisionName = environmentAgent.AgentState
                },
                AgentId = environmentAgent.AgentId,
                AgentPos = environmentAgent.AgentSequence,
                AgentPassword = environmentAgent.AgentPassword,
                Language = environmentAgent.Language,
                SelectedCurrency = environmentAgent.SendCurrencies?.Select(x => new CurrencyInfo {CurrencyCode = x})
                    .FirstOrDefault()
            };
            return agentManagementViewModel;
        }
    }
}