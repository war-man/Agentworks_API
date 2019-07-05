using System.ComponentModel;
using System.Windows;
using TransactionRunner.Interfaces;

namespace TransactionRunner.ViewModels
{
    public class BatchNumberViewModel : IBatchNumberViewModel, INotifyPropertyChanged
    {
        public BatchNumberViewModel() {

        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (!value)
                {
                    BatchNumber = 1;
                }
                RaisePropertyChanged(nameof(Enabled));
            }
        }

        private int _batchNumber;
        public int BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged(nameof(BatchNumber));
            }

        }
        public void Initialize()
        {
            BatchNumber = 1;
            ModalVisibility = Visibility.Collapsed;
            Enabled = true;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}