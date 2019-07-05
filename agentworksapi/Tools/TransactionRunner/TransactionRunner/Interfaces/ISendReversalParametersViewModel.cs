using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using System.Windows;
using AmountRange = MoneyGram.AgentConnect.IntegrationTest.Data.Models.AmountRange;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;
using ServiceOption = MoneyGram.AgentConnect.IntegrationTest.Data.Models.ServiceOption;

namespace TransactionRunner.Interfaces
{
    /// <summary>
    ///     View model for send reversal parameters.
    /// </summary>
    public interface ISendReversalParametersViewModel
    {
        /// <summary>
        ///     Gets or sets the selected reason for refund.
        /// </summary>
        EnumeratedIdentifierInfo SelectedRefundReason { get; set; }

        /// <summary>
        ///     Gets or sets the lists of reasons for refund.
        /// </summary>
        List<EnumeratedIdentifierInfo> RefundReasonList { get; }

        /// <summary>
        ///     Gets or sets whether or not there is a refund fee.
        /// </summary>
        bool RefundFee { get; set; }

        /// <summary>
        ///     Initialize the view model.
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
        ///     Available amount ranges
        /// </summary>
        List<AmountRange> AmountRangeList { get; }

        /// <summary>
        ///     Currently filled custom amount
        /// </summary>
        decimal CustomAmount { get; set; }

        /// <summary>
        ///     True when 'Custom amount' option is selected
        /// </summary>
        bool IsCustomAmountRangeSelected { get; }

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
        List<ServiceOption> ServiceOptionList { get; }

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