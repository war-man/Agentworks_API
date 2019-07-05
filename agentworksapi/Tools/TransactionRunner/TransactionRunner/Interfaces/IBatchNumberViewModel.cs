using System.Windows;

namespace TransactionRunner.Interfaces
{
    public interface IBatchNumberViewModel
    {
        int BatchNumber { get; set; }
        bool Enabled { get; set; }
        void Initialize();
        Visibility ModalVisibility { get; set; }
    }
}