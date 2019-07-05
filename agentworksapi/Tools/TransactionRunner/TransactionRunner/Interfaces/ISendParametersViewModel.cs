using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using AmountRange = MoneyGram.AgentConnect.IntegrationTest.Data.Models.AmountRange;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;
using ServiceOption = MoneyGram.AgentConnect.IntegrationTest.Data.Models.ServiceOption;
using System.Windows;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     ViewModel for the send parameters
    /// </summary>
    public interface ISendParametersViewModel
    {
        /// <summary>
        ///     Initializes the viewmodel
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Currently selected country
        /// </summary>
        CountryInfo SelectedCountry { get; set; }

        /// <summary>
        ///     Available countries
        /// </summary>
        List<CountryInfo> CountryList { get; }

        /// <summary>
        ///     Currently selected country subdivision
        /// </summary>
        SubdivisionInfo SelectedCountrySubdivision { get; set; }

        /// <summary>
        ///     Available country subdivisions
        /// </summary>
        List<SubdivisionInfo> CountrySubdivisionList { get; }

        /// <summary>
        ///     Currently selected currency
        /// </summary>
        CurrencyInfo SelectedCurrency { get; set; }

        /// <summary>
        ///     List of available currencies
        /// </summary>
        List<CurrencyInfo> CurrencyList { get; }

        /// <summary>
        ///     Currently selected amount range
        /// </summary>
        AmountRange SelectedAmountRange { get; set; }

        /// <summary>
        ///     Currently filled custom amount
        /// </summary>
        decimal CustomAmount { get; set; }

        /// <summary>
        ///     True when 'Custom amount' option is selected
        /// </summary>
        bool IsCustomAmountRangeSelected { get; }

        /// <summary>
        ///     Available amount ranges
        /// </summary>
        List<AmountRange> AmountRangeList { get; }

        /// <summary>
        ///     Currently selected item choice
        /// </summary>
        FeeType SelectedItemChoice { get; set; }

        /// <summary>
        ///     Available item choices
        /// </summary>
        List<FeeType> ItemChoiceList { get; }

        /// <summary>
        ///     Currently selected service option
        /// </summary>
        ServiceOption SelectedServiceOption { get; set; }

        /// <summary>
        ///     List of available service options
        /// </summary>
        List<ServiceOption> ServiceOptionList {get;}

        /// <summary>
        ///     List of third party options
        /// </summary>
        List<string> ThirdPartyTypeList { get; }

        /// <summary>
        ///  Currently selected third party type
        /// </summary>
        string SelectedThirdPartyType { get; set; }

        /// <summary>
        ///     Gets or sets the value determining if the agent is valid.
        /// </summary>
        bool IsAgentValid { get; set; }

        Visibility ModalVisibility { get; set; }
    }
}