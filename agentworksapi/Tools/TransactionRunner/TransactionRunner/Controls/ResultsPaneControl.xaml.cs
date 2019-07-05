using System.Windows;
using System.Windows.Controls;
using TransactionRunner.Helpers;
using TransactionRunner.Interfaces;
using TransactionRunner.Messaging;
using TransactionRunner.ViewModels;

namespace TransactionRunner.Controls
{
    /// <summary>
    ///     Interaction logic for ResultsPaneControl.xaml
    /// </summary>
    public partial class ResultsPaneControl : UserControl
    {
        private readonly IResultsPaneViewModel _vm;

        /// <summary>
        /// </summary>
        public ResultsPaneControl()
        {
            InitializeComponent();
            _vm = new ResultsPaneViewModel(StaticMessageBus.MessageBus);
            btnOpenDir.IsEnabled = false;
            btnOpenFile.IsEnabled = false;
        }


        private void ResultsPaneControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm.Initialize();
            DataContext = _vm;
        }

        private void btnOpenDir_Click(object sender, RoutedEventArgs e)
        {
            if (LstTransactionOutputList.SelectedIndex != -1)
            {
                var outputModel = LstTransactionOutputList.SelectedItem as OutputViewModel;
                FileHelper.OpenDirectory(outputModel.Path);
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (LstTransactionOutputList.SelectedIndex != -1)
            {
                var outputModel = LstTransactionOutputList.SelectedItem as OutputViewModel;
                FileHelper.OpenFile(outputModel.Path);
            }
        }

        private void LstTransactionOutputList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpenFile.IsEnabled = true;
            btnOpenDir.IsEnabled = true;
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (LstTransactionOutputList.SelectedIndex != -1)
                {
                    var outputModel = LstTransactionOutputList.SelectedItem as OutputViewModel;
                    FileHelper.OpenFile(outputModel.Path);
                }
            }
        }
    }
}