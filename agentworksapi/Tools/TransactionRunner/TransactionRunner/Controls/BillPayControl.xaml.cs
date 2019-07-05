using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using TransactionRunner.Interfaces;
using TransactionRunner.ViewModels.Static;

namespace TransactionRunner.Controls
{
    /// <summary>
    /// Interaction logic for StagedBillPayControl.xaml
    /// </summary>
    public partial class BillPayControl : UserControl
    {
        private IBillPayViewModel _vm;
        public BillPayControl()
        {
            InitializeComponent();
        }

        private void BillPayControl_Initialized(object sender, EventArgs e)
        {
            _vm = StaticBillPayVm.BillPayViewModel;
            _vm.Initialize();
            DataContext = _vm;

        }

        private void CbxBiller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBiller = (sender as ComboBox).SelectedItem as TestBiller;
            if (selectedBiller.Name == "Manual Entry")
            {
                ToggleCustomBillerVisibility(System.Windows.Visibility.Visible);

            }
            else
            {
                ToggleCustomBillerVisibility(System.Windows.Visibility.Collapsed);
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void ToggleCustomBillerVisibility(System.Windows.Visibility visible)
        {
            lblCustomBiller.Visibility = visible;
            lblCustomBillerCode.Visibility = visible;
            lblCustomBillerValidAccountNumber.Visibility = visible;
            txtCustomBillerCode.Visibility = visible;
            txtCustomBillerValidAccountNumber.Visibility = visible;
        }

    }
}