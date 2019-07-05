using System;

namespace TransactionRunner.ViewModels
{
    /// <summary>
    /// Interface for generic Popup view models
    /// </summary>
    public interface IPopupViewModel
    {
        /// <summary>
        /// Initializes view model before view is displayed
        /// </summary>
        void Initialize();

        /// <summary>
        /// Title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Event called when user closes window
        /// </summary>
        event EventHandler RequestClose;

        /// <summary>
        /// Checks if popup can be closed
        /// </summary>
        /// <returns></returns>
        bool CanClose();
    }
}