using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    /// Generic view model for popup windows
    /// </summary>
    public abstract class PopupViewModelBase : INotifyPropertyChanged, IPopupViewModel
    {
        /// <summary>
        /// Title
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes view model instance
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Event called when user closes window
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Checks if popup can be closed
        /// </summary>
        /// <returns></returns>
        public virtual bool CanClose()
        {
            return true;
        }

        /// <summary>
        /// Raises RequestClose event
        /// </summary>
        protected void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes code on UI thread
        /// </summary>
        /// <param name="action"></param>
        protected void InvokeOnUIThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}