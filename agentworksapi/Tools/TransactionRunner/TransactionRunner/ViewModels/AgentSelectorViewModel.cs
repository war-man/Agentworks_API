using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using System.Windows;
using MoneyGram.AgentConnect.IntegrationTest.Data;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using TransactionRunner.ViewModels.Static;
using MoneyGram.AgentConnect.IntegrationTest.Operations;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     ViewModel for the environment/agent selector
    /// </summary>
    public class AgentSelectorViewModel : INotifyPropertyChanged, IAgentSelectorViewModel
    {
        private readonly ISettingsSvc _settingsSvc;
        private readonly IMessageBus _messageBus;

        private const string NotAvailable = "N/A";

        /// <summary>
        ///     Create viewmodel with provided settingsSvc and messageBus
        /// </summary>
        /// <param name="settingsSvc"></param>
        /// <param name="messageBus"></param>
        public AgentSelectorViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus)
        {
            _settingsSvc = settingsSvc;
            _messageBus = messageBus;
        }

        /// <summary>
        ///     initializes the viewmodel
        /// </summary>
        public void Initialize()
        {
            LoadAgents();
            SetDefaults(_settingsSvc?.UserSettings);
            _messageBus.Subscribe<AgentManagementChangedEvent>(AgentManagementChange);
            ModalVisibility = Visibility.Collapsed;
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

        private void LoadAgents()
        {
            EnvironmentList = _settingsSvc?.UserSettings?.EnvironmentAgents?.Select(x => x.Key).ToList();
        }

        private void AgentManagementChange(AgentManagementChangedEvent obj)
        {
            LoadAgents();
            RaisePropertyChanged(nameof(EnvironmentList));
            RaisePropertyChanged(nameof(AgentLocationList));
            SelectedAgentLocation = AgentLocationList.First();
            RaisePropertyChanged(nameof(SelectedAgentLocation));
            RaisePropertyChanged(nameof(AgentList));
            SelectedAgent = AgentList.First();
            RaisePropertyChanged(nameof(SelectedAgent));
            RaisePropertyChanged(nameof(AgentPosList));
        }

        private bool _isAgentValid;

        /// <summary>
        ///     Gets the value determine whether or not a valid agent is selected.
        /// </summary>
        public bool IsAgentValid
        {
            get { return _isAgentValid; }
            set
            {
                _isAgentValid = value;
                _messageBus.Publish(new AgentChangedEvent {AgentIsValid = _isAgentValid});
            }
        }

        /// <summary>
        ///     Handles changes for environment and location fields
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetDefaults(UserSettings settings)
        {
            SelectedEnvironment = EnvironmentList?.FirstOrDefault(env => env == settings.Environment);
            SelectedAgentLocation =
                AgentLocationList.FirstOrDefault(loc => loc.AgentStateCode == settings.AgentLocation) ??
                AgentLocationList.First();
            SelectedAgent = AgentList.FirstOrDefault(agent => agent.AgentId == settings.AgentId) ??
                            AgentList.First();
            SelectedAgentPos = AgentPosList.FirstOrDefault(agentPos => agentPos.AgentSequence == settings.AgentPosId) ??
                               AgentPosList.First();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region AcEnvironment

        private string _selectedEnvironment;

        /// <summary>
        ///     Bound to environment combobox on the UI.  Raises property changed event.
        /// </summary>
        public string SelectedEnvironment
        {
            get { return _selectedEnvironment; }

            set
            {
                //this property
                _selectedEnvironment = value;
                RaisePropertyChanged(nameof(SelectedEnvironment));
                //propagate
                RaisePropertyChanged(nameof(AgentLocationList));
                SelectedAgentLocation = AgentLocationList.First();
                RaisePropertyChanged(nameof(SelectedAgentLocation));
                _messageBus.Publish(new EnvironmentChangeEvent {SelectedEnvironment = _selectedEnvironment});

                TestConfig.TestSettings = new IntegrationTestSettingsModel {AcEnvironment = _selectedEnvironment};
                Initializer.Initialize();
                StaticTransactionPickerVm.TransactionPickerViewModel.LoadTransactionList();
            }
        }

        /// <summary>
        ///     Bound to environment list.
        /// </summary>
        public List<string> EnvironmentList { get; set; }

        #endregion

        #region AgentLocation

        private static readonly List<EnvironmentAgent> NoAgentLocations =
            new List<EnvironmentAgent> {new EnvironmentAgent {AgentState = NotAvailable}};

        private EnvironmentAgent _selectedAgentLocation;

        /// <summary>
        ///     Bound to selected agent location.  Raises property changed event.
        /// </summary>
        public EnvironmentAgent SelectedAgentLocation
        {
            get { return _selectedAgentLocation; }
            set
            {
                //this property
                _selectedAgentLocation = value;
                RaisePropertyChanged(nameof(SelectedAgentLocation));
                //propagate
                RaisePropertyChanged(nameof(AgentList));
                SelectedAgent = AgentList.First();
                RaisePropertyChanged(nameof(SelectedAgent));
                _messageBus.Publish(new AgentLocationChangedEvent());
            }
        }

        /// <summary>
        ///     Bound to agent location list.  Raises property changed event.
        /// </summary>
        public List<EnvironmentAgent> AgentLocationList =>
            SelectedEnvironment != null && _settingsSvc.UserSettings.ContainsEnvironment(SelectedEnvironment) &&
            _settingsSvc.UserSettings.GetAgents(SelectedEnvironment).Any()
                ? _settingsSvc?.UserSettings?.GetAgents(SelectedEnvironment)?.GroupBy(x => x.AgentStateCode)
                    .Select(x => x?.First()).ToList()
                : NoAgentLocations;

        #endregion

        #region Agent

        private EnvironmentAgent _selectedAgent;

        /// <summary>
        ///     Bound to selected agent.  Raises property changed event.
        /// </summary>
        public EnvironmentAgent SelectedAgent
        {
            get { return _selectedAgent; }
            set
            {
                //this property
                _selectedAgent = value;
                RaisePropertyChanged(nameof(SelectedAgent));
                //propagate
                RaisePropertyChanged(nameof(AgentPosList));
                SelectedAgentPos = AgentPosList.First();
                RaisePropertyChanged(nameof(SelectedAgentPos));
                IsAgentValid = _selectedAgent != null && _selectedAgent.AgentId != NotAvailable;
            }
        }

        private static readonly List<EnvironmentAgent> NoAgents =
            new List<EnvironmentAgent> {new EnvironmentAgent {AgentId = NotAvailable}};

        /// <summary>
        ///     Bound to list of available agents.  Raises property changed event.
        /// </summary>
        public List<EnvironmentAgent> AgentList =>
            SelectedAgentLocation != null && SelectedAgentLocation?.AgentState != NotAvailable
                ? _settingsSvc?.UserSettings?.GetAgents(SelectedEnvironment)
                    .Where(agent => agent.AgentStateCode == SelectedAgentLocation.AgentStateCode)
                    .GroupBy(agent => agent.AgentId)
                    .Select(group => group?.First()).ToList()
                : NoAgents;

        #endregion

        #region AgentPos

        private EnvironmentAgent _selectedAgentPos;

        /// <summary>
        ///     Bound to currently selected agent.  Raises property changed event
        /// </summary>
        public EnvironmentAgent SelectedAgentPos
        {
            get { return _selectedAgentPos; }
            set
            {
                //this property
                _selectedAgentPos = value;
                RaisePropertyChanged(nameof(SelectedAgentPos));
                //nothing to propagate
            }
        }

        private static readonly List<EnvironmentAgent> NoPos =
            new List<EnvironmentAgent> {new EnvironmentAgent {AgentSequence = NotAvailable}};

        /// <summary>
        ///     List of available agent POSes for selected agentId
        /// </summary>
        public List<EnvironmentAgent> AgentPosList => SelectedAgent != null && SelectedAgent?.AgentId != NotAvailable
            ? _settingsSvc?.UserSettings?.GetAgents(SelectedEnvironment)
                .Where(agent => agent.AgentStateCode == SelectedAgentLocation?.AgentStateCode &&
                                agent.AgentId == SelectedAgent?.AgentId)
                .ToList()
            : NoPos;

        #endregion
    }
}