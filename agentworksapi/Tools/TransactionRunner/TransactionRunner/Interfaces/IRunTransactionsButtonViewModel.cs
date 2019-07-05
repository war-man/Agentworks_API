using System.Threading.Tasks;

namespace TransactionRunner.Interfaces
{
    internal interface IRunTransactionsButtonViewModel
    {
        string RunButtonContent { get; set; }
        bool StopButtonEnabled { get; set; }
        bool RunButtonEnabled { get; set; }
        Task SendClick();
        void StopClick();
        void Initialize();
        bool IsAgentValid { get; set; }
    }
}