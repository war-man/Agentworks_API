using System.Collections.Generic;
using System.Linq;
using TransactionRunner.ConfigProviders;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;
using System.Windows;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     ViewModel for the transaction picker
    /// </summary>
    public class TransactionPickerViewModel : BaseAgentViewModel, ITransactionPickerViewModel
    {
        private readonly ISettingsSvc _settingsSvc;
        private readonly IMessageBus _messageBus;

        /// <summary>
        ///     Instantiate a new Transaction Picker View Model.
        /// </summary>
        /// <param name="settingsSvc"></param>
        /// <param name="messageBus"></param>
        public TransactionPickerViewModel(ISettingsSvc settingsSvc, IMessageBus messageBus)
            : base(messageBus)
        {
            _settingsSvc = settingsSvc;
            _messageBus = messageBus;
        }

        /// <summary>
        ///     Initialize the viewmodel
        /// </summary>
        public new void Initialize()
        {
            ModalVisibility = Visibility.Collapsed;
            base.Initialize();
        }

        /// <summary>
        ///     Load the view model.
        /// </summary>
        public void Load()
        {
            LoadTransactionList();

            SetDefaults(_settingsSvc?.UserSettings);
            
            _messageBus.Publish(new ProductChangeEvent { ProductType = _selectedTransactionHandler.Type, TransactionName = _selectedTransactionHandler.TransactionName });
        }
        public void LoadTransactionList()
        {
            TransactionList = TransactionProvider.TransactionHandlers;
            SelectedTransaction = TransactionList.FirstOrDefault();
        }

        private void SetDefaults(UserSettings userSettings)
        {
            SelectedTransaction = TransactionList.FirstOrDefault(trans => trans.Type == userSettings.TransactionType) ??
                                  TransactionList.First();
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
        #region TransactionList

        private ITransactionHandler _selectedTransactionHandler;

        /// <summary>
        ///     Bound to transaction combobox on the UI.  Raises property changed event.
        /// </summary>
        public ITransactionHandler SelectedTransaction
        {
            get { return _selectedTransactionHandler; }

            set
            {
                //this property
                _selectedTransactionHandler = value;
                if (_selectedTransactionHandler == null)
                {
                    _selectedTransactionHandler = _transactionList.First();
                }
                //if(_selectedTransactionHandler.TransactionName == StaticTransactionNames.SendReversal)
                //{
                //    // Disable batch
                //    StaticBatchNumberVm.BatchNumberViewModel.Enabled = false;
                //}
                //else
                //{
                //    StaticBatchNumberVm.BatchNumberViewModel.Enabled = true;
                //}
                RaisePropertyChanged(nameof(SelectedTransaction));
                _messageBus.Publish(new ProductChangeEvent { ProductType = _selectedTransactionHandler.Type, TransactionName = _selectedTransactionHandler.TransactionName });
            }
        }

        private IEnumerable<ITransactionHandler> _transactionList;

        /// <summary>
        ///     Bound to transaction handler list.
        /// </summary>
        public IEnumerable<ITransactionHandler> TransactionList
        {
            get { return _transactionList; }
            private set
            {
                _transactionList = value;
                RaisePropertyChanged(nameof(TransactionList));
            }
        }

        #endregion
    }
}