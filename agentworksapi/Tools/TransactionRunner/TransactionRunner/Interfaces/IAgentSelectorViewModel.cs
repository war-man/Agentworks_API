using System.Collections.Generic;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using System.Windows;

namespace TransactionRunner.Interfaces
{
    public interface IAgentSelectorViewModel
    {
        void Initialize();

        string SelectedEnvironment { get; set; }
        List<string> EnvironmentList { get; }

        EnvironmentAgent SelectedAgentLocation { get; set; }
        List<EnvironmentAgent> AgentLocationList { get; }

        EnvironmentAgent SelectedAgent { get; set; }
        List<EnvironmentAgent> AgentList { get; }

        EnvironmentAgent SelectedAgentPos { get; set; }
        List<EnvironmentAgent> AgentPosList { get; }

        bool IsAgentValid { get; set; }
        Visibility ModalVisibility { get; set; }
    }
}