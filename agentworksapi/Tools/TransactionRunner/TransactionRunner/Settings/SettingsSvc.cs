using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using MoneyGram.Common.Json;
using Newtonsoft.Json;
using TransactionRunner.ImportExport;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels;
using TransactionRunner.ViewModels.Settings;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Settings
{
    internal class SettingsSvc : ISettingsSvc, IAgentSettings
    {
        const string MoneygramPath = "MoneyGram International";
        public SettingsSvc()
        {
            UserSettings = ReadSettingsFromFile() ?? DefaultUserSettings;

            if (!Directory.Exists(ExportPath))
            {
                Directory.CreateDirectory(ExportPath);
            }            
            if (!Directory.Exists(UserConfigPath))
                Directory.CreateDirectory(UserConfigPath);
            Initialize();
        }
        
        private static string ExecutingDir => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);        
        private static string UserConfigFilePath => Path.Combine(UserConfigPath, "user.config");
        private static string AgentsDataPath => Path.Combine(ExecutingDir, "Data");
        private static string AgentsDataFilePath => Path.Combine(AgentsDataPath, "EnvironmentAgents.json");
        private static string ExportPath => Path.Combine(ExecutingDir, "Export");
        private static string UserConfigPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            MoneygramPath, System.Diagnostics.Process.GetCurrentProcess().ProcessName);
        

        private static UserSettings DefaultUserSettings { get; } = new UserSettings
        {
            AgentLocation = null,
            AgentId = null,
            AgentPosId = null,
            Environment = "Q2",
            TransactionType = SessionType.SEND,
            SendSettings = new SendSettings
            {
                CountryCode = "USA",
                CurrencyCode = "USD",
            },
            StagedSendSettings = new SendSettings
            {
                CountryCode = "USA",
                CurrencyCode = "USD",
            },
            SendReversalSettings = new SendReversalSettings
            {
                CountryCode = "USA",
                CurrencyCode = "USD"
            },
            ReceiveSettings = new ReceiveSettings
            {
                CountryCode = "USA",
                CurrencyCode = "USD",
            },
            StagedReceiveSettings = new ReceiveSettings
            {
                CountryCode = "USA",
                CurrencyCode = "USD",
            },
            BillPaySettings = new BillPaySettings
            {
                CountryCode = "USA"
            },
            StagedBillPaySettings = new BillPaySettings
            {
                CountryCode = "USA"
            },
            FileFormat = ExportFileFormat.CSV,
            LastUsedDirectory = ExportPath
        };

        public void AddAgent(IAgentManagementViewModel agentVm)
        {
            var agent = agentVm.ToEnvironmentAgent();

            UserSettings.AddAgent(agent);

            SaveSettingsFile(UserSettings);
        }

        public IAgentManagementViewModel GetAgent(string environment, string agentId, string agentPos)
        {
            var agent = UserSettings.GetAgent(environment, agentId, agentPos);

            var addAgentViewModel = agent?.ToAddAgentViewModel();

            return addAgentViewModel;
        }

        public void UpdateAgent(IAgentManagementViewModel oldAgentVm, IAgentManagementViewModel newAgentVm)
        {
            var oldAgent = oldAgentVm.ToEnvironmentAgent();
            var newAgent = newAgentVm.ToEnvironmentAgent();

            UserSettings.UpdateAgent(oldAgent, newAgent);

            SaveSettingsFile(UserSettings);
        }

        public void DeleteAgent(IAgentManagementViewModel agentVm)
        {
            UserSettings.DeleteAgent(agentVm.SelectedEnvironment, agentVm.AgentId, agentVm.AgentPos);

            SaveSettingsFile(UserSettings);
        }

        public UserSettings UserSettings { get; }

        public bool SaveViewModelSettings()
        {
            var settings = GetUserSettingsFromVm(StaticAgentSelectorVm.AgentSelectorViewModel, 
                StaticReceiveVm.ReceiveViewModel, StaticStagedReceiveVm.StagedReceiveViewModel,
                StaticSendParametersVm.SendParametersViewModel, StaticStagedSendParametersVm.StagedSendParametersViewModel,
                StaticSendReversalParametersVm.SendReversalParametersViewModel, StaticBillPayVm.BillPayViewModel,
                StaticStagedBillPayVm.StagedBillPayViewModel, StaticTransactionPickerVm.TransactionPickerViewModel);

            // Environment Agents (Agents, POS) are loaded from the default EnvironmentAgents.json configuration file.
            // The view models do not contain those settings are we will need to rehydrate from the user's current configuration settings.
            settings.EnvironmentAgents = UserSettings?.EnvironmentAgents;

            try
            {
                SaveSettingsFile(settings);
            }
            catch
            {
                // TODO: exception handling
                return false;
            }
            return true;
        }

        public bool Save()
        {
            try
            {
                SaveSettingsFile(UserSettings);
            }
            catch
            {
                // TODO: exception handling
                return false;
            }
            return true;
        }

        public void Initialize()
        {
            // If the user's config file does not contain a Agent/POS list,
            // then load them from the default EnvironmentAgents.json file.
            if(File.Exists(AgentsDataFilePath)
               && (!File.Exists(UserConfigFilePath) || !UserSettings.HasAgents))
            {
                var repo = new EnvironmentAgentRepository();
                var environmentAgents = repo.GetAllValues<List<EnvironmentAgentInfoList>>()
                    .ToDictionary(x => x.Environment, x => x.Agents);

                var settings = UserSettings;
                settings.EnvironmentAgents = environmentAgents;

                SaveSettingsFile(settings);
            }
            else if(File.Exists(UserConfigFilePath))
            {
                // Save Environments.json file with userconfig data. (this usually happens when a new release comes out and they dont want to erase custom agents)
                SaveSettingsFile(UserSettings);
            }
        }

        private static UserSettings ReadSettingsFromFile()
        {
            var userSettingsFileContents = JsonFileHelper.GetFileContents(UserConfigFilePath);
            UserSettings settings = null;
            if(userSettingsFileContents != null)
            {
                settings = JsonConvert.DeserializeObject<UserSettings>(userSettingsFileContents);
            }
            return settings;
        }

        private static UserSettings GetUserSettingsFromVm(IAgentSelectorViewModel agentSettings,
            IReceiveViewModel receiveSettings,
            IStagedReceiveViewModel stagedReceiveSettings,
            ISendParametersViewModel sendSettings,
            ISendParametersViewModel stagedSendSettings, 
            ISendReversalParametersViewModel sendReversalSettings,
            IBillPayViewModel billPaySettings, IStagedBillPayViewModel stagedBillPaySettings, ITransactionPickerViewModel transactionPicker)
        {
            return new UserSettings
            {
                Environment = agentSettings.SelectedEnvironment,
                AgentId = agentSettings.SelectedAgent.AgentId,
                AgentLocation = agentSettings.SelectedAgentLocation.AgentStateCode,
                AgentPosId = agentSettings.SelectedAgentPos.AgentSequence,
                TransactionType = transactionPicker.SelectedTransaction.Type,
                SendSettings = new SendSettings
                {
                    CountryCode = sendSettings.SelectedCountry?.CountryCode,
                    AmountRange = sendSettings.SelectedAmountRange?.Code,
                    CustomAmount = sendSettings.CustomAmount,
                    CountrySubdivisionCode = sendSettings?.SelectedCountrySubdivision.CountrySubdivisionCode,
                    CurrencyCode = sendSettings.SelectedCurrency?.CurrencyCode,
                    ItemChoice = sendSettings.SelectedItemChoice?.Code,
                    ServiceOption = sendSettings.SelectedServiceOption?.Key,
                    ThirdPartyType = sendSettings.SelectedThirdPartyType
                },
                StagedSendSettings = new SendSettings
                {
                    CountryCode = stagedSendSettings.SelectedCountry?.CountryCode,
                    AmountRange = stagedSendSettings.SelectedAmountRange?.Code,
                    CustomAmount = stagedSendSettings.CustomAmount,
                    CountrySubdivisionCode = stagedSendSettings?.SelectedCountrySubdivision.CountrySubdivisionCode,
                    CurrencyCode = stagedSendSettings.SelectedCurrency?.CurrencyCode,
                    ItemChoice = stagedSendSettings.SelectedItemChoice?.Code,
                    ServiceOption = stagedSendSettings.SelectedServiceOption?.Key,
                    ThirdPartyType = stagedSendSettings.SelectedThirdPartyType
                },
                SendReversalSettings = new SendReversalSettings
                {
                    CountryCode = sendReversalSettings.SelectedCountry?.CountryCode,
                    AmountRange = sendReversalSettings.SelectedAmountRange?.Code,
                    CustomAmount = sendReversalSettings.CustomAmount,
                    CountrySubdivisionCode = sendReversalSettings?.SelectedCountrySubdivision.CountrySubdivisionCode,
                    CurrencyCode = sendReversalSettings.SelectedCurrency?.CurrencyCode,
                    ItemChoice = sendReversalSettings.SelectedItemChoice?.Code,
                    ServiceOption = sendReversalSettings.SelectedServiceOption?.Key,
                    ThirdPartyType = sendReversalSettings.SelectedThirdPartyType,
                    RefundReasonCode = sendReversalSettings.SelectedRefundReason?.Identifier,
                    RefundFee = sendReversalSettings.RefundFee
                },
                ReceiveSettings = new ReceiveSettings
                {
                    AgentState = receiveSettings.SelectedCountrySubdivision?.CountrySubdivisionCode,
                    CountryCode = receiveSettings.SelectedCountry?.CountryCode,
                    CountrySubdivisionCode = receiveSettings.SelectedCountrySubdivision?.CountrySubdivisionCode,
                    CurrencyCode = receiveSettings.SelectedCurrency?.CurrencyCode,
                    AmountRange = receiveSettings.SelectedAmountRange?.Code,
                    CustomAmount = receiveSettings.CustomAmount,
                    ItemChoice = receiveSettings.SelectedItemChoice?.Code,
                    ServiceOption = receiveSettings.SelectedServiceOption?.Key,
                    ThirdPartyType = receiveSettings.SelectedThirdPartyType
                },
                StagedReceiveSettings = new ReceiveSettings
                {
                    AgentState = stagedReceiveSettings.SelectedCountrySubdivision?.CountrySubdivisionCode,
                    CountryCode = stagedReceiveSettings.SelectedCountry?.CountryCode,
                    CountrySubdivisionCode = stagedReceiveSettings.SelectedCountrySubdivision?.CountrySubdivisionCode,
                    CurrencyCode = stagedReceiveSettings.SelectedCurrency?.CurrencyCode,
                    AmountRange = stagedReceiveSettings.SelectedAmountRange?.Code,
                    CustomAmount = stagedReceiveSettings.CustomAmount,
                    ItemChoice = stagedReceiveSettings.SelectedItemChoice?.Code,
                    ServiceOption = stagedReceiveSettings.SelectedServiceOption?.Key,
                    ThirdPartyType = stagedReceiveSettings.SelectedThirdPartyType
                },
                BillPaySettings = new BillPaySettings
                {
                    CountryCode = billPaySettings.SelectedCountry?.CountryCode,
                    AmountRange = billPaySettings.SelectedAmountRange?.Code,
                    CustomAmount = billPaySettings.CustomAmount,
                    ThirdPartyType = billPaySettings.SelectedThirdPartyType,
                    Biller =  billPaySettings.SelectedBiller?.Code,
                    ManualBillerCode = billPaySettings.ManualBillerCode,
                    ManualBillerAccountNumber = billPaySettings.ManualBillerAccountNumber
                },
                StagedBillPaySettings = new BillPaySettings
                {
                    CountryCode = stagedBillPaySettings.SelectedCountry?.CountryCode,
                    AmountRange = stagedBillPaySettings.SelectedAmountRange?.Code,
                    CustomAmount = stagedBillPaySettings.CustomAmount,
                    ThirdPartyType = stagedBillPaySettings.SelectedThirdPartyType,
                    Biller = stagedBillPaySettings.SelectedBiller?.Code,
                    ManualBillerCode = stagedBillPaySettings.ManualBillerCode,
                    ManualBillerAccountNumber = stagedBillPaySettings.ManualBillerAccountNumber
                },
                FileFormat = StaticSettings.SettingsSvc.UserSettings.FileFormat,
                LastUsedDirectory = StaticSettings.SettingsSvc.UserSettings.LastUsedDirectory
            };
        }

        private void SaveSettingsFile(UserSettings settings)
        {
            if (!Directory.Exists(UserConfigPath))
                Directory.CreateDirectory(UserConfigPath);

            using (var file = File.Open(UserConfigFilePath, FileMode.Create))
            {
                using(var streamWriter = new StreamWriter(file))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, settings);
                }
            }
            var envAgents = settings.EnvironmentAgents;
            // Save Environments.json file
            using(var file = File.Open(AgentsDataFilePath, FileMode.Create))
            {
                using(var streamWriter = new StreamWriter(file))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, settings.EnvironmentAgents);
                }
            }
        }
    }
}