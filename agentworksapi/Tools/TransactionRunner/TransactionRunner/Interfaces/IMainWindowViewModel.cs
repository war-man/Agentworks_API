using System;
using TransactionRunner.Helpers;

namespace TransactionRunner.Interfaces
{
    public interface IMainWindowViewModel
    {
        void Initialize();

        string AppVersion { get; }

        /// <summary>
        /// Application startup mode
        /// </summary>
        AppStartupMode StartupMode { get; set; }

        void OnClosed(object sender, EventArgs e);
    }
}