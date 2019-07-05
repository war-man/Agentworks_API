﻿using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;
using ThirdPartyType = MoneyGram.AgentConnect.IntegrationTest.Data.Setup.TestThirdPartyType;
using System.Windows;
using TransactionRunner.ViewModels.Settings;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     ViewModel for the SendParameters pane.  Raises property changed notifications when properties change.
    /// </summary>
    public class SendParametersViewModel : BaseAgentViewModel, ISendParametersViewModel
    {
        private readonly string _transactionName;
        private readonly ISettingsSvc _settingsSvc;
        private readonly CountryRepository _countryRepository;
        private readonly CountrySubdivisionRepository _countrySubdivisionRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly AmountRangeRepository _amountRangeRepository;
        private readonly FeeTypeRepository _feeTypeRepository;
        private readonly ServiceOptionRepository _serviceOptionRepository;
        private readonly IMessageBus _messageBus;

        private readonly AmountRange _amountRangeCustom = new AmountRange { Code="CustomAmount", Display = "Custom Amount"};

        /// <summary>
        ///     Create this viewmodel with settings provided by given settingsSvc
        /// </summary>
        /// <param name="settingsSvc">Settings service</param>
        /// <param name="messageBus"></param>
        /// <param name="transactionName"></param>
        public SendParametersViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus, string transactionName)
            : base(messageBus)
        {
            _settingsSvc = settingsSvc;
            _countryRepository = new CountryRepository();
            _countrySubdivisionRepository = new CountrySubdivisionRepository();
            _currencyRepository = new CurrencyRepository();
            _amountRangeRepository = new AmountRangeRepository();
            _feeTypeRepository = new FeeTypeRepository();
            _serviceOptionRepository = new ServiceOptionRepository();
            _messageBus = messageBus;
            _transactionName = transactionName;
            _messageBus.Subscribe<ProductChangeEvent>(ProductChanged);
        }

        /// <summary>
        ///     initializes the viewmodel, based on any settings provided
        /// </summary>
        public new void Initialize()
        {
            SetDefaults(_settingsSvc?.UserSettings);
            ModalVisibility = Visibility.Collapsed;
            base.Initialize();
        }

        private void SetDefaults(UserSettings settings)
        {
            SendSettings settingsSource = _transactionName == StaticTransactionNames.Send
                ? settings.SendSettings
                : settings.StagedSendSettings;

            SelectedCountry =
                CountryList?.FirstOrDefault(country => country.CountryCode == settingsSource.CountryCode) ??
                CountryList?.FirstOrDefault();

            SelectedCountrySubdivision =
                CountrySubdivisionList.FirstOrDefault(
                    countrySubdivision => countrySubdivision.CountrySubdivisionCode ==
                                          settingsSource.CountrySubdivisionCode) ??
                CountrySubdivisionList.First();

            SelectedCurrency = CurrencyList?.FirstOrDefault(curr => curr.CurrencyCode == settingsSource.CurrencyCode) ??
                               CurrencyList?.FirstOrDefault();

            SelectedAmountRange =
                AmountRangeList?.FirstOrDefault(ar => ar.Code == settingsSource.AmountRange) ??
                AmountRangeList?.FirstOrDefault();

            CustomAmount = settingsSource.CustomAmount;

            SelectedItemChoice =
                ItemChoiceList?.FirstOrDefault(ic => ic.Code == settingsSource.ItemChoice) ??
                ItemChoiceList?.FirstOrDefault();

            SelectedServiceOption =
                ServiceOptionList?.FirstOrDefault(so => so.Key == settingsSource.ServiceOption) ??
                ServiceOptionList?.FirstOrDefault();

            SelectedThirdPartyType = ThirdPartyTypeList?.FirstOrDefault(tp => tp == settingsSource.ThirdPartyType) ??
                                     ThirdPartyTypeList?.FirstOrDefault();
        }

        #region Country

        private CountryInfo _selectedCountry;

        /// <summary>
        ///     Bound to selected country.  Raises property changed event.
        /// </summary>
        public CountryInfo SelectedCountry
        {
            get { return _selectedCountry; }

            set
            {
                _selectedCountry = value;
                RaisePropertyChanged(nameof(SelectedCountry));
                RaisePropertyChanged(nameof(CountrySubdivisionList));
                SelectedCountrySubdivision = CountrySubdivisionList?.First();
                RaisePropertyChanged(nameof(SelectedCountrySubdivision));
            }
        }

        /// <summary>
        ///     Bound to list of available countries.
        /// </summary>
        public List<CountryInfo> CountryList => _countryRepository.GetAllValues<List<CountryInfo>>();

        #endregion

        #region CountrySubdivision

        private static readonly List<SubdivisionInfo> NoSubdivisions = new List<SubdivisionInfo>
            {new SubdivisionInfo {CountrySubdivisionName = "N/A"}};

        private SubdivisionInfo _selectedCountrySubdivision;

        /// <summary>
        ///     Bound to selected subdivision/state/province.  Raises property changed event.
        /// </summary>
        public SubdivisionInfo SelectedCountrySubdivision
        {
            get => _selectedCountrySubdivision;
            set
            {
                _selectedCountrySubdivision = value;
                RaisePropertyChanged(nameof(SelectedCountrySubdivision));
                _messageBus.Publish(new SendStateChangedEvent { SelectedCountrySubdivision = SelectedCountrySubdivision });
            }
        }

        /// <summary>
        ///     Bound to available subdivisions for current country.  Raises property changed event.
        /// </summary>
        public List<SubdivisionInfo> CountrySubdivisionList => _selectedCountry != null && _countrySubdivisionRepository.ContainsKey(_selectedCountry.CountryCode)
            ? _countrySubdivisionRepository.GetValue<List<SubdivisionInfo>>(_selectedCountry.CountryCode)
            : NoSubdivisions;

        #endregion

        #region Currency

        private CurrencyInfo _selectedCurrency;

        /// <summary>
        ///     Bound to selected currency.  Raises property changed event.
        /// </summary>
        public CurrencyInfo SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
                RaisePropertyChanged(nameof(SelectedCurrency));
            }
        }

        /// <summary>
        ///     Bound to available currencies.
        /// </summary>
        public List<CurrencyInfo> CurrencyList => _currencyRepository.GetAllValues<List<CurrencyInfo>>();

        #endregion

        #region AmountRanges

        private AmountRange _selectedAmountRange;

        /// <summary>
        ///     Bound to available AmountRanges.  Raises property changed event.
        /// </summary>
        public AmountRange SelectedAmountRange
        {
            get { return _selectedAmountRange; }
            set
            {
                _selectedAmountRange = value;
                RaisePropertyChanged(nameof(SelectedAmountRange));
                RaisePropertyChanged(nameof(IsCustomAmountRangeSelected));
            }
        }

        /// <summary>
        ///     Bound to 'Custom Amount' field on UI
        /// </summary>
        public decimal CustomAmount
        {
            get { return _customAmount; }
            set
            {
                _customAmount = value;
                RaisePropertyChanged(nameof(CustomAmount));
            }
        }

        /// <summary>
        /// True when 'Custom amount' option is selected
        /// </summary>
        public bool IsCustomAmountRangeSelected => object.ReferenceEquals(SelectedAmountRange, _amountRangeCustom);

        /// <summary>
        ///     Bound to available amount ranges
        /// </summary>
        public List<AmountRange> AmountRangeList => _amountRangeRepository.GetAllValues<List<AmountRange>>()
                                                                          .Append(_amountRangeCustom)
                                                                          .ToList();

        #endregion

        #region Item Choices

        private FeeType _selectedItemChoice;

        /// <summary>
        ///     Bound to selected item choice.  Raises property changed event.
        /// </summary>
        public FeeType SelectedItemChoice
        {
            get { return _selectedItemChoice; }
            set
            {
                _selectedItemChoice = value;
                RaisePropertyChanged(nameof(SelectedItemChoice));
            }
        }

        /// <summary>
        ///     Bound to available item choices.
        /// </summary>
        public List<FeeType> ItemChoiceList => _feeTypeRepository.GetAllValues<List<FeeType>>();

        #endregion

        #region Service Options

        private ServiceOption _selectedServiceOption;

        /// <summary>
        ///     Bound to selected service option.  Raises property changed event.
        /// </summary>
        public ServiceOption SelectedServiceOption
        {
            get { return _selectedServiceOption; }
            set
            {
                _selectedServiceOption = value;
                RaisePropertyChanged(nameof(SelectedServiceOption));
            }
        }

        /// <summary>
        ///     Bound to available service options
        /// </summary>
        public List<ServiceOption> ServiceOptionList => _serviceOptionRepository.GetAllValues<List<ServiceOption>>();

        #endregion

        #region ThirdPartyType
        private string _selectedThirdPartyType;
        public string SelectedThirdPartyType
        {
            get { return _selectedThirdPartyType; }
            set
            {
                _selectedThirdPartyType = value;
                RaisePropertyChanged(nameof(SelectedThirdPartyType));
            }
        }

        public List<string> ThirdPartyTypeList => new List<string> { ThirdPartyType.None, ThirdPartyType.Org, ThirdPartyType.Person };
        #endregion

        private Visibility _modalVisibility;

        /// <summary>
        ///     Bound to Modal.  Raises property changed event.
        /// </summary>
        public Visibility ModalVisibility
        {
            get { return _modalVisibility; }
            set
            {
                _modalVisibility = value;
                RaisePropertyChanged(nameof(ModalVisibility));
            }
        }

        private void ProductChanged(ProductChangeEvent obj)
        {
            Visibility = obj.ProductType == SessionType.SEND && obj.TransactionName == _transactionName ? Visibility.Visible : Visibility.Hidden;
        }

        private Visibility _visibility;
        private decimal _customAmount;

        /// <summary>
        ///     Gets or sets the visibility of the control, which is based on the product type.
        /// </summary>
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                RaisePropertyChanged(nameof(Visibility));
            }
        }
    }
}