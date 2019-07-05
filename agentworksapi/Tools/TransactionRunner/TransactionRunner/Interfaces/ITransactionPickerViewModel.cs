using System.Collections.Generic;
using System.Windows;

namespace TransactionRunner.Interfaces
{
    public interface ITransactionPickerViewModel
    {
        void Initialize();
        void Load();
        void LoadTransactionList();
        IEnumerable<ITransactionHandler> TransactionList { get; }
        ITransactionHandler SelectedTransaction { get; set; }
        bool IsAgentValid { get; set; }
        Visibility ModalVisibility { get; set; }
    }
}