using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using TransactionRunner.ViewModels.Static;
using AmountRange = MoneyGram.AgentConnect.IntegrationTest.Data.Models.AmountRange;
using ThirdPartyType = MoneyGram.AgentConnect.IntegrationTest.Data.Setup.TestThirdPartyType;

namespace TransactionRunner.ViewModels
{
    public class BillPayViewModel: BaseAgentViewModel, IBillPayViewModel
    {
        private readonly string _transactionName = StaticTransactionNames.BillPay;
        private IMessageBus _messageBus;
        private ISettingsSvc _settingsSvc;
        private readonly CountryRepository _countryRepository;
        private readonly CountrySubdivisionRepository _countrySubdivisionRepository;
        private readonly AmountRangeRepository _amountRangeRepository;

        private readonly AmountRange _amountRangeCustom = new AmountRange() { Code = "CustomAmount", Display = "Custom Amount" };
        
        //private List<string> _delimitedStates = new List<string> { AgentLocation.MN, AgentLocation.NY, AgentLocation.OK, AgentLocation.OR, AgentLocation.FL };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBus"></param>
        /// <param name="settingsSvc"></param>
        public BillPayViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus)
            : base(messageBus)
        {
            _messageBus = messageBus;
            _settingsSvc = settingsSvc;
            _countryRepository = new CountryRepository();
            _countrySubdivisionRepository = new CountrySubdivisionRepository();
            _amountRangeRepository = new AmountRangeRepository();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            SetDefaults(_settingsSvc?.UserSettings);
            ModalVisibility = Visibility.Collapsed;
            _messageBus.Subscribe<ProductChangeEvent>(ProductChanged);
        }

        private void SetDefaults(UserSettings settings)
        {
            SelectedCountry = CountryList?.FirstOrDefault(c => c.CountryCode == settings.BillPaySettings.CountryCode) ??
                              CountryList?.FirstOrDefault();
            SelectedAmountRange =
                AmountRangeList?.FirstOrDefault(ar => ar.Code == settings.BillPaySettings.AmountRange) ??
                AmountRangeList?.FirstOrDefault();
            CustomAmount = settings.BillPaySettings.CustomAmount;

            SelectedThirdPartyType =
                ThirdPartyTypeList?.FirstOrDefault(tp => tp == settings.BillPaySettings.ThirdPartyType) ??
                ThirdPartyTypeList?.FirstOrDefault();
            BillerList = new List<TestBiller>
            {
                Billers.HubbardAttorney,
                Billers.Ford,
                Billers.ComcastCableXfinity,
                new TestBiller {Code = "ManualEntry", Name = "Manual Entry"}
            };
            SelectedBiller = BillerList?.FirstOrDefault(b => b.Code == settings.BillPaySettings.Biller) ??
                             BillerList.FirstOrDefault();

            ManualBillerCode = settings.BillPaySettings.ManualBillerCode;
            ManualBillerAccountNumber = settings.BillPaySettings.ManualBillerAccountNumber;
        }

        private void ProductChanged(ProductChangeEvent obj)
        {
            Visibility = obj.TransactionName == _transactionName ? Visibility.Visible : Visibility.Hidden;
        }

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

        private string _manualBillerCode;

        /// <summary>
        ///     Gets or sets the manual entry for biller code.
        /// </summary>
        public string ManualBillerCode
        {
            get => _manualBillerCode;
            set
            {
                _manualBillerCode = value;
                RaisePropertyChanged(nameof(ManualBillerCode));
            }
        }

        private string _manualBillerAccountNumber;

        /// <summary>
        ///     Gets or sets the manual entry for biller code.
        /// </summary>
        public string ManualBillerAccountNumber
        {
            get => _manualBillerAccountNumber;
            set
            {
                _manualBillerAccountNumber = value;
                RaisePropertyChanged(nameof(ManualBillerAccountNumber));
            }
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
            }
        }

        /// <summary>
        ///     Bound to list of available countries.
        /// </summary>
        public List<CountryInfo> CountryList => _countryRepository.GetAllValues<List<CountryInfo>>().Where(x => x.CountryCode == "USA").ToList();

        #endregion

        #region Biller
        private TestBiller _selectedBiller;
        public TestBiller SelectedBiller
        {
            get { return _selectedBiller; }
            set
            {
                _selectedBiller = value;
                RaisePropertyChanged(nameof(SelectedBiller));
            }
        }

        public List<TestBiller> BillerList { get; set; }
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
    }
}