using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using System.Windows;

namespace TransactionRunner.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IReceiveParametersViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		void Initialize();

		/// <summary>
		/// 
		/// </summary>
		ReceiveRequest ReceiveRequest { get; set; }
		/// <summary>
		/// 
		/// </summary>
		bool IsAgentValid { get; set; }

        Visibility ModalVisibility { get; set; }

    }
}