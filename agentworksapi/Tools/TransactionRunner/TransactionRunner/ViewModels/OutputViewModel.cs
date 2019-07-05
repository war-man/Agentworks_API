using System;
using System.ComponentModel;
using System.Linq;

namespace TransactionRunner.ViewModels
{
    public class OutputViewModel : INotifyPropertyChanged
    {
        private string _PathForView;
        private string _Path;
        private string _ElapsedTime;
        private TimeSpan _ElapsedTimeSpan;
        public string PathForView
        {
            get { return _PathForView; }
            set
            {
                var directories = value.Split('\\');
                var fileName = directories.Last();
                var parentDir = directories[directories.Length - 2];
                _PathForView = $"...{System.IO.Path.Combine(parentDir, fileName)}";
                NotifyPropertyChanged("PathForView");
            }
        }

        public string Path
        {
            get { return _Path; }
            set { _Path = value; NotifyPropertyChanged("Path"); }
        }
        public string ElapsedTime
        {
            get { return _ElapsedTime; }
            set { _ElapsedTime = value; NotifyPropertyChanged("ElapsedTime"); }
        }

        public TimeSpan ElapsedTimeSpan
        {
            get { return _ElapsedTimeSpan; }
            set { _ElapsedTimeSpan = value; NotifyPropertyChanged("ElapsedTimeSpan"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}