using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;
using ThirdPartyType = MoneyGram.AgentConnect.IntegrationTest.Data.Setup.TestThirdPartyType;
using System.Windows;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.ViewModels
{
    public class StagedReceiveViewModel : BaseAgentViewModel, IStagedReceiveViewModel
    {
        private readonly string _transactionName = StaticTransactionNames.StagedReceive;
        private readonly ISettingsSvc _settingsSvc;
        private readonly CountryRepository _countryRepository;
        private readonly CountrySubdivisionRepository _countrySubdivisionRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly AmountRangeRepository _amountRangeRepository;
        private readonly FeeTypeRepository _feeTypeRepository;
        private readonly ServiceOptionRepository _serviceOptionRepository;
        private readonly IMessageBus _messageBus;

        private readonly AmountRange _amountRangeCustom = new AmountRange() { Code = "CustomAmount", Display = "Custom Amount" };

        /// <summary>
        ///     Create this viewmodel with settings provided by given settingsSvc
        /// </summary>
        /// <param name="settingsSvc">Settings service</param>
        /// <param name="messageBus"></param>
        public StagedReceiveViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus)
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
            _messageBus.Subscribe<ProductChangeEvent>(ProductChanged);
            _messageBus.Subscribe<AgentLocationChangedEvent>(AgentLocationChanged);
        }

        /// <summary>
        ///     initializes the viewmodel, based on any settings provided
        /// </summary>
        public new void Initialize()
        {
            FilterCountries();
            FilterCountrySubdivisions();
            SetDefaults(_settingsSvc?.UserSettings);
            ModalVisibility = Visibility.Collapsed;
            base.Initialize();
        }

        private void SetDefaults(UserSettings settings)
        {
            SelectedCountry =
                CountryList?.FirstOrDefault(
                    country => country.CountryCode == settings.StagedReceiveSettings.CountryCode) ??
                CountryList?.FirstOrDefault();

            SelectedCountrySubdivision =
                CountrySubdivisionList.FirstOrDefault(
                    countrySubdivision => countrySubdivision.CountrySubdivisionCode ==
                                          settings.StagedReceiveSettings.CountrySubdivisionCode) ??
                CountrySubdivisionList.FirstOrDefault();

            SelectedCurrency =
                CurrencyList?.FirstOrDefault(curr =>
                    curr.CurrencyCode == settings.StagedReceiveSettings.CurrencyCode) ?? CurrencyList?.FirstOrDefault();

            SelectedAmountRange =
                AmountRangeList?.FirstOrDefault(ar => ar.Code == settings.StagedReceiveSettings.AmountRange) ??
                AmountRangeList?.FirstOrDefault();

            CustomAmount = settings.StagedReceiveSettings.CustomAmount;

            SelectedItemChoice =
                ItemChoiceList?.FirstOrDefault(ic => ic.Code == settings.StagedReceiveSettings.ItemChoice) ??
                ItemChoiceList?.FirstOrDefault();

            SelectedServiceOption =
                ServiceOptionList?.FirstOrDefault(so => so.Key == settings.StagedReceiveSettings.ServiceOption) ??
                ServiceOptionList?.FirstOrDefault();

            SelectedThirdPartyType =
                ThirdPartyTypeList?.FirstOrDefault(tp => tp == settings.StagedReceiveSettings.ThirdPartyType) ??
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
                FilterCountrySubdivisions();
                SelectedCountrySubdivision = CountrySubdivisionList?.FirstOrDefault();
            }
        }

        private List<CountryInfo> _countryList;

        /// <summary>
        ///     Bound to list of available countries.
        /// </summary>
        public List<CountryInfo> CountryList
        {
            get { return _countryList; }
            set
            {
                _countryList = value;
                RaisePropertyChanged(nameof(CountryList));
            }
        }

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

        private List<SubdivisionInfo> _countrySubdivisionList;

        /// <summary>
        ///     Bound to available subdivisions for current country.  Raises property changed event.
        /// </summary>
        public List<SubdivisionInfo> CountrySubdivisionList
        {
            get { return _countrySubdivisionList; }
            set
            {
                _countrySubdivisionList = value;
                RaisePropertyChanged(nameof(CountrySubdivisionList));
            }
        }

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

        private decimal _customAmount;

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
        private void AgentLocationChanged(AgentLocationChangedEvent evt)
        {
            FilterCountries();
            FilterCountrySubdivisions();
        }
        private void ProductChanged(ProductChangeEvent obj)
        {
            Visibility = obj.TransactionName == _transactionName ? Visibility.Visible : Visibility.Hidden;
            if(Visibility == Visibility.Visible)
            {
                FilterCountries();
                FilterCountrySubdivisions();
            }
        }
        private void FilterCountries()
        {
            List<CountryInfo> filteredCountries = new List<CountryInfo>();
            var selectedAgentLocation = StaticAgentSelectorVm.AgentSelectorViewModel.SelectedAgentLocation;
            if (selectedAgentLocation?.AgentCountryIsoCode != null)
            {
                filteredCountries = _countryRepository.GetAllValues<List<CountryInfo>>().ToList().Where(x=>x.CountryCode.ToLower() == selectedAgentLocation.AgentCountryIsoCode.ToLower()).ToList();
            }
            else
            {
                filteredCountries = _countryRepository.GetAllValues<List<CountryInfo>>().ToList();
            }
            CountryList = filteredCountries;
        }
        private void FilterCountrySubdivisions()
        {
            List<SubdivisionInfo> filteredSubDivisionList = new List<SubdivisionInfo>();
            var selectedAgentLocation = StaticAgentSelectorVm.AgentSelectorViewModel.SelectedAgentLocation;
            if (selectedAgentLocation != null)
            {
                var availableLocations = StaticAgentSelectorVm.AgentSelectorViewModel.AgentLocationList.Where(x => x.AgentState != StaticAgentSelectorVm.AgentSelectorViewModel.SelectedAgentLocation.AgentState).ToList();
                filteredSubDivisionList = _selectedCountry != null && _countrySubdivisionRepository.ContainsKey(_selectedCountry.CountryCode)
            ? _countrySubdivisionRepository.GetValue<List<SubdivisionInfo>>(_selectedCountry.CountryCode).Where(x => availableLocations.Any(y => y.AgentState.ToLower() == x.CountrySubdivisionName.ToLower())).ToList()
            : NoSubdivisions;
            }
            else
            {
                filteredSubDivisionList = _selectedCountry != null && _countrySubdivisionRepository.ContainsKey(_selectedCountry.CountryCode)
            ? _countrySubdivisionRepository.GetValue<List<SubdivisionInfo>>(_selectedCountry.CountryCode)
            : NoSubdivisions;
            }
            CountrySubdivisionList = filteredSubDivisionList;
            SelectedCountrySubdivision = CountrySubdivisionList.FirstOrDefault();
        }

        private Visibility _visibility;

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