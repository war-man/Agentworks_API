using System;

namespace TransactionRunner.Timer
{
    public interface IElapsedTimerViewModel
    {
        TimeSpan ElapsedTime { get; set; }
        string Display { get; }
        void Start();
        void Stop();
    }
}