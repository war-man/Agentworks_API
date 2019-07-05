using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     ViewModel for adding new agents.
    /// </summary>
    public class AgentManagementViewModel : IAgentManagementViewModel, INotifyPropertyChanged
    {
        private readonly ISettingsSvc _settingsSvc;
        private readonly IMessageBus _messageBus;
        private readonly CountryRepository _countryRepository;
        private readonly CountrySubdivisionRepository _countrySubdivisionRepository;
        private readonly CurrencyRepository _currencyRepository;

        /// <summary>
        ///     Instantiates a new AddAgentViewModel.
        /// </summary>
        public AgentManagementViewModel()
        {

        }

        /// <summary>
        ///     Instantiates a new AddAgentViewModel.
        /// </summary>
        /// <param name="settingsSvc"></param>
        /// <param name="messageBus"></param>
        public AgentManagementViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus)
        {
            _settingsSvc = settingsSvc;
            _messageBus = messageBus;
            _countryRepository = new CountryRepository();
            _countrySubdivisionRepository = new CountrySubdivisionRepository();
            _currencyRepository = new CurrencyRepository();
        }

        /// <summary>
        ///     Initialize the view model.
        /// </summary>
        public void Initialize()
        {
            LoadEnvironments();
            LoadAgents();
        }

        private void LoadAgents()
        {
            var agents = new List<EnvironmentAgent>();
            var environments = EnvironmentAgentList.Select(x => new EnvironmentAgentInfoList { Environment = x.Key, Agents = x.Value });
            foreach (var environment in environments)
            {
                environment.Agents.ForEach(x => x.Environment = environment.Environment);
                agents.AddRange(environment.Agents);
            }
            GroupedAgents = new ListCollectionView(agents);
            GroupedAgents.GroupDescriptions.Add(new PropertyGroupDescription("Environment"));
            _messageBus.Publish(new AgentManagementChangedEvent());
        }

        private void LoadEnvironments()
        {
            EnvironmentAgentList = _settingsSvc?.UserSettings?.EnvironmentAgents ?? new Dictionary<string, List<EnvironmentAgent>>();
            EnvironmentList = EnvironmentAgentList.Keys.ToList();
            _messageBus.Publish(new EnvironmentChangeEvent { SelectedEnvironment = _selectedEnvironment });
        }

        /// <summary>
        ///     Saves the agent to the user's local settings.
        /// </summary>
        public void Save()
        {
            if (_settingsSvc?.UserSettings == null) return;

            var agent = this.ToEnvironmentAgent();
            // This is to remove the - from the subdivisions before saving.
            if (agent.AgentStateCode.Contains("-"))
            {
                agent.AgentStateCode = agent.AgentStateCode.Split('-')[1];
            }

            if (_settingsSvc.UserSettings.ContainsAgent(agent.Environment, agent.AgentId, agent.AgentSequence))
            {
                var oldAgent = _settingsSvc.UserSettings.GetAgent(agent.Environment, agent.AgentId, agent.AgentSequence);
                _settingsSvc.UserSettings.UpdateAgent(oldAgent, agent);
            }
            else
            {
                _settingsSvc.UserSettings.AddAgent(agent);
            }
            _settingsSvc.Save();

            Clear();

            LoadAgents();
            RaisePropertyChanged(nameof(GroupedAgents));

            _messageBus.Publish(new AgentManagementChangedEvent());
        }

        /// <summary>
        ///     Clears the selected values in the view model.
        /// </summary>
        public void Clear()
        {
            SelectedEnvironment = null;
            SelectedCountry = null;
            SelectedCountrySubdivision = null;
            AgentId = string.Empty;
            AgentPos = string.Empty;
            AgentPassword = string.Empty;
            Language = string.Empty;
            SelectedCurrency = null;
            EnvironmentName = string.Empty;
        }

        /// <summary>
        ///     Edit Agent.
        /// </summary>
        /// <param name="agent"></param>
        public void EditAgent(EnvironmentAgent agent)
        {
            if (agent == null) return;

            SelectedEnvironment = agent.Environment;
            SelectedCountry = CountryList?.Find(x => x.CountryCode == agent.AgentCountryIsoCode);

            var selectedSub = CountrySubdivisionList?.Find(x =>
                x.CountrySubdivisionName != null && agent.AgentState != null &&
                x.CountrySubdivisionName.ToUpper().Contains(agent.AgentState.ToUpper()));

            if (selectedSub != null && selectedSub.CountrySubdivisionCode.Contains("-"))
            {
                selectedSub.CountrySubdivisionCode = selectedSub.CountrySubdivisionCode.Split('-')[1];
            }
            SelectedCountrySubdivision = selectedSub;
            AgentId = agent.AgentId;
            AgentPos = agent.AgentSequence;
            AgentPassword = agent.AgentPassword;
            Language = agent.Language;
            SelectedCurrency = CurrencyList?.Find(x => x.CurrencyCode == agent.SendCurrencies?.First());
        }

        /// <summary>
        ///     Delete Agent.
        /// </summary>
        public void DeleteAgent(EnvironmentAgent agent)
        {
            _settingsSvc.UserSettings.DeleteAgent(agent.Environment, agent.AgentId, agent.AgentSequence);
            _settingsSvc.Save();

            LoadEnvironments();
            RaisePropertyChanged(nameof(EnvironmentList));
            LoadAgents();
            RaisePropertyChanged(nameof(GroupedAgents));

            _messageBus.Publish(new AgentManagementChangedEvent());
        }

        /// <summary>
        ///     Saves the environment to the user's local settings.
        /// </summary>
        public void SaveEnvironment()
        {
            if (_settingsSvc?.UserSettings == null) return;

            _settingsSvc.UserSettings.EnvironmentAgents.Add(EnvironmentName, new List<EnvironmentAgent>());
            _settingsSvc.Save();

            EnvironmentName = string.Empty;

            LoadEnvironments();
            RaisePropertyChanged(nameof(EnvironmentList));
            LoadAgents();
            RaisePropertyChanged(nameof(GroupedAgents));

            _messageBus.Publish(new AgentManagementChangedEvent());
        }

        /// <summary>
        ///     Deletes the environment from the user's local settings.
        /// </summary>
        /// <param name="environment"></param>
        public void DeleteEnvironment(string environment)
        {
            if (_settingsSvc?.UserSettings == null) return;

            _settingsSvc.UserSettings.EnvironmentAgents.Remove(environment);
            _settingsSvc.Save();

            LoadEnvironments();
            RaisePropertyChanged(nameof(EnvironmentList));
            LoadAgents();
            RaisePropertyChanged(nameof(GroupedAgents));
        }

        /// <summary>
        ///     Get or sets the collection of agents that are grouped and displayed in the grid.
        /// </summary>
        public ICollectionView GroupedAgents { get; set; }

        public Dictionary<string, List<EnvironmentAgent>> EnvironmentAgentList { get; set; }

        public List<string> EnvironmentList { get; set; }

        private string _selectedEnvironment;

        public string SelectedEnvironment
        {
            get { return _selectedEnvironment; }
            set
            {
                _selectedEnvironment = value;
                RaisePropertyChanged(nameof(SelectedEnvironment));
            }
        }

        public List<CountryInfo> CountryList => _countryRepository.GetAllValues<List<CountryInfo>>();

        private CountryInfo _selectedCountry;

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

        private static readonly List<SubdivisionInfo> NoSubdivisions = new List<SubdivisionInfo> { new SubdivisionInfo { CountrySubdivisionName = "N/A", CountrySubdivisionCode = "N/A" } };

        public List<SubdivisionInfo> CountrySubdivisionList => _selectedCountry != null && _countrySubdivisionRepository.ContainsKey(_selectedCountry.CountryCode)
            ? _countrySubdivisionRepository.GetValue<List<SubdivisionInfo>>(_selectedCountry.CountryCode)
            : NoSubdivisions;

        private SubdivisionInfo _selectedCountrySubdivision;

        public SubdivisionInfo SelectedCountrySubdivision
        {
            get { return _selectedCountrySubdivision; }
            set
            {
                _selectedCountrySubdivision = value;
                RaisePropertyChanged(nameof(SelectedCountrySubdivision));
                _messageBus.Publish(new AgentManagementChangedEvent());
            }
        }

        private string _agentId;

        public string AgentId
        {
            get { return _agentId; }
            set
            {
                _agentId = value;
                RaisePropertyChanged(nameof(AgentId));
            }
        }

        private string _agentPos;

        public string AgentPos
        {
            get { return _agentPos; }
            set
            {
                _agentPos = value;
                RaisePropertyChanged(nameof(AgentPos));
            }
        }

        private string _agentPassword;

        public string AgentPassword
        {
            get { return _agentPassword; }
            set
            {
                _agentPassword = value;
                RaisePropertyChanged(nameof(AgentPassword));
            }
        }

        private string _language;

        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                RaisePropertyChanged(nameof(Language));
            }
        }

        private CurrencyInfo _selectedCurrency;

        public CurrencyInfo SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
                RaisePropertyChanged(nameof(SelectedCurrency));
            }
        }

        public List<CurrencyInfo> CurrencyList => _currencyRepository.GetAllValues<List<CurrencyInfo>>().OrderByDescending(x=>x.CurrencyName).ToList();

        private bool _isAgentValid;

        public bool IsAgentValid
        {
            get { return _isAgentValid; }
            set
            {
                _isAgentValid = value;
                RaisePropertyChanged(nameof(IsAgentValid));
            }
        }

        private string _environmentName;

        public string EnvironmentName
        {
            get { return _environmentName; }
            set
            {
                _environmentName = value;
                RaisePropertyChanged(nameof(EnvironmentName));
            }
        }

        private bool _isEnvironmentValid;

        public bool IsEnvironmentValid
        {
            get { return _isEnvironmentValid; }
            set
            {
                _isEnvironmentValid = value;
                RaisePropertyChanged(nameof(IsEnvironmentValid));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property has been changed.
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != "IsAgentValid" && propertyName != "IsEnvironmentValid")
            {
                CheckIfAgentIsValid();
                CheckIfEnvironmentIsValid();
            }
        }

        private void CheckIfAgentIsValid()
        {
            IsAgentValid = !string.IsNullOrEmpty(SelectedEnvironment) && SelectedCountry != null
                            && SelectedCountrySubdivision != null && !string.IsNullOrEmpty(AgentId)
                            && !string.IsNullOrEmpty(AgentPos) && !string.IsNullOrEmpty(AgentPassword)
                            && !string.IsNullOrEmpty(Language) && SelectedCurrency != null;
        }

        private void CheckIfEnvironmentIsValid()
        {
            IsEnvironmentValid = !string.IsNullOrEmpty(EnvironmentName);
        }
    }
}