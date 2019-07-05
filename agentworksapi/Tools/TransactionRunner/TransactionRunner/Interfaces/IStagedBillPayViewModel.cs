using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using AmountRange = MoneyGram.AgentConnect.IntegrationTest.Data.Models.AmountRange;
using System.Collections.Generic;
using System.Windows;

namespace TransactionRunner.Interfaces
{
    public interface IStagedBillPayViewModel
    {
        void Initialize();

        /// <summary>
        ///     Currently selected country
        /// </summary>
        CountryInfo SelectedCountry { get; set; }

        /// <summary>
        ///     Available countries
        /// </summary>
        List<CountryInfo> CountryList { get; }

        ///// <summary>
        /////     Currently selected country subdivision
        ///// </summary>
        //SubdivisionInfo SelectedCountrySubdivision { get; set; }

        ///// <summary>
        /////     Available country subdivisions
        ///// </summary>
        //List<SubdivisionInfo> CountrySubdivisionList { get; }

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
        ///     Gets or sets the value determining if the agent is valid.
        /// </summary>
        bool IsAgentValid { get; set; }

        /// <summary>
        ///     List of third party options
        /// </summary>
        List<string> ThirdPartyTypeList { get; }

        /// <summary>
        ///  Currently selected third party type
        /// </summary>
        string SelectedThirdPartyType { get; set; }

        /// <summary>
        ///  Currently selected biller
        /// </summary>
        TestBiller SelectedBiller { get; set; }

        /// <summary>
        ///    Manually entered biller code 
        /// </summary>
        string ManualBillerCode { get; set; }

        /// <summary>
        ///  Manually entered biller account number
        /// </summary>
        string ManualBillerAccountNumber { get; set; }

        Visibility ModalVisibility { get; set; }
    }
}