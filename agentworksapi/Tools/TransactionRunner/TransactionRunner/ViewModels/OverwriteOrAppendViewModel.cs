using TransactionRunner.Controls;
using TransactionRunner.Helpers;
using TransactionRunner.ImportExport;

namespace TransactionRunner.ViewModels
{
    [Template(typeof(OverwriteOrAppendControl))]
    public class OverwriteOrAppendViewModel : PopupViewModelBase
    {
        public override string Title => string.Empty;

        public ExportMode? Result { get; private set; }

        public RelayCommand OverwriteCommand => new RelayCommand(x =>
        {
            Result = ExportMode.Overwrite;
            OnRequestClose();
        });

        public RelayCommand AppendCommand => new RelayCommand(x =>
        {
            Result = ExportMode.Append;
            OnRequestClose();
        });
    }
}