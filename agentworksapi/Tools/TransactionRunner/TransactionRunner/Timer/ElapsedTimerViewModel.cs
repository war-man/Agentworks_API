using System;
using System.ComponentModel;
using System.Diagnostics;
using TransactionRunner.Interfaces;

namespace TransactionRunner.Timer
{
    /// <summary>
    ///     Viewmodel for timer
    /// </summary>
    public class ElapsedTimerViewModel : IElapsedTimerViewModel, INotifyPropertyChanged
    {
        private readonly Stopwatch _stopwatch;
        private readonly System.Timers.Timer _timer;
        private readonly IMessageBus _messageBus;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="messageBus"></param>
        public ElapsedTimerViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _messageBus.Subscribe<TimerStartEvent>(Start);
            _messageBus.Subscribe<TimerStopEvent>(Stop);
            _timer = new System.Timers.Timer {Interval = 127, AutoReset = true};
            _timer.Elapsed += delegate { UpdateDisplayWithTime(_stopwatch.Elapsed); };
            _stopwatch = new Stopwatch();
            ElapsedTime = new TimeSpan(0,0,0,0);
        }

        /// <summary>
        ///     Get the display text for the timer
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        ///     Reference to the elapsed time for easy calculations
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        ///     Reset and start the timer
        /// </summary>
        public void Start()
        {
            _timer.Start();
            _stopwatch.Reset();
            _stopwatch.Start();
            ElapsedTime = new TimeSpan(0, 0, 0, 0);
        }

        /// <summary>
        ///     Stop the timer and update display
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
            _timer.Stop();
            UpdateDisplayWithTime(_stopwatch.Elapsed);
        }

        /// <summary>
        ///     Raises events when property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateDisplayWithTime(TimeSpan elapsed)
        {
            Display = $"{elapsed.Minutes}:{elapsed.Seconds:00}:{elapsed.Milliseconds.ToString("000").Substring(0, 2)}";
            ElapsedTime = elapsed;
            RaisePropertyChanged(nameof(Display));
        }

        private void Start(TimerStartEvent e)
        {
            Start();
        }

        private void Stop(TimerStopEvent e)
        {
            Stop();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}