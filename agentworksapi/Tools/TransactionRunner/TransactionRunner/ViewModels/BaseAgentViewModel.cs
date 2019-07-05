using System.ComponentModel;
using TransactionRunner.Interfaces;
using TransactionRunner.Transactions;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    ///     Base Agent View Model
    /// </summary>
    public abstract class BaseAgentViewModel : INotifyPropertyChanged
    {
        private readonly IMessageBus _messageBus;

        private bool _isAgentValid = true;

        /// <summary>
        ///     Instantiate a new Base Agent View Model.
        /// </summary>
        /// <param name="messageBus"></param>
        protected BaseAgentViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        /// <summary>
        ///     Get or sets the value determining if the parameters form should be enabled or not.
        /// </summary>
        public virtual bool IsAgentValid
        {
            get { return _isAgentValid; }
            set
            {
                _isAgentValid = value;
                RaisePropertyChanged(nameof(IsAgentValid));
            }
        }

        /// <summary>
        ///     Event when property is change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initialize the base view model.
        /// </summary>
        public void Initialize()
        {
            _messageBus.Subscribe<AgentChangedEvent>(CheckIfAgentIsValid);
        }

        private void CheckIfAgentIsValid(AgentChangedEvent obj)
        {
            IsAgentValid = obj.AgentIsValid;
        }

        /// <summary>
        ///     Property has been changed.
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}