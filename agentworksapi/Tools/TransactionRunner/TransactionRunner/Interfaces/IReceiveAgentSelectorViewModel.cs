using System.Collections.Generic;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using System.Windows;

namespace TransactionRunner.Interfaces
{
    public interface IReceiveAgentSelectorViewModel
    {
        void Initialize();
	    EnvironmentAgent SelectedAgent { get; set; }
	    EnvironmentAgent SelectedAgentPos { get; set; }
        EnvironmentAgent SelectedReceiveAgentLocation { get; set; }
		string SelectedThirdPartyType { get; set; }
		List<string> ThirdPartyTypeList { get; set; }
        List<EnvironmentAgent> ReceiveAgentLocationList { get; }
        Visibility ModalVisibility { get; set; }
    }
}