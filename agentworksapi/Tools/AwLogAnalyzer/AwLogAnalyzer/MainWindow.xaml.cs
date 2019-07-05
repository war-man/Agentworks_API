using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AwLogAnalyzer.LogParsers;
using AwLogAnalyzer.Models;
using Microsoft.Win32;
using AwLogAnalyzer.ReportAggregators;

namespace AwLogAnalyzer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogParser logParser;
        private readonly IReportAggregator reportAggregator;

        public MainWindow()
        {
            InitializeComponent();
            logParser = new LogParser();
            reportAggregator = new ExcelReportAggregator();

            endDate.SelectedDate = DateTime.Now;
            endTime.Text = DateTime.Now.ToString("HH:mm:ss");
            DataContext = new AnalyzerVersion();
        }

        private void Select_File(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            var result = dlg.ShowDialog();

            if (result == true)
            {
                textBox.Text = string.Join(";", dlg.FileNames);
            }
        }

        private async void Run_Report(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == string.Empty)
            {
                return;
            }

            loadingText.Visibility = Visibility.Visible;

            DateTime start, end;
            var startTimeString = startDate.Text + " " + startTime.Text;
            var endTimeString = endDate.Text + " " + endTime.Text;

            var startTimeSet = DateTime.TryParse(startTimeString, out start);
            var endTimeSet = DateTime.TryParse(endTimeString, out end);

            var reportResults = await logParser.ParseLogFile(
                textBox.Text,
                (msg) => UpdateLoadingText(msg),
                startTimeSet ? (DateTime?) start : null,
                endTimeSet ? (DateTime?) end : null);

            await reportAggregator.AggregateReport(
                reportResults,
                (msg) => UpdateLoadingText(msg));

            loadingText.Visibility = Visibility.Hidden;
        }
        
        private void UpdateLoadingText(string msg)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // do whatever you want to do with shared object.
                loadingText.Content = msg;
                loadingText.UpdateLayout();
            }
            else
            {
                //Other wise re-invoke the method with UI thread access
                Application.Current.Dispatcher.Invoke(new System.Action(() => UpdateLoadingText(msg)));
            }
        }

        private void EndTime_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TimeTextChanged(endTime);
        }

        private void StartTime_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TimeTextChanged(startTime);
        }

        private void TimeTextChanged(TextBox textBox)
        {
            var validationRegex = new Regex(@"[\d]{2}:[\d]{2}:[\d]{2}");

            textBox.Background = (textBox.Text != string.Empty && !validationRegex.IsMatch(textBox.Text))
                ? new SolidColorBrush(Color.FromRgb(212, 125, 125))
                : new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}