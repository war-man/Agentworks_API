using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TransactionRunner.Helpers.Controls
{
    /// <summary>
    ///     TextBox for number input.
    /// </summary>
    /// <remarks>
    ///     Very simple class for the time being.
    ///     If it is necessary to have more functionalities it may be considered to use one of the free control libraries.
    /// </remarks>
    public class NumberTextBox : TextBox
    {
        /// <summary>
        ///     Number of decimal places
        /// </summary>
        public int DecimalPlaces { get; set; } = 2;

        /// <summary>
        ///     Minimum decimal value to be used
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        ///     Maximum decimal value to be used
        /// </summary>
        public decimal? MaxValue { get; set; }

        private string NumberFormat => $"N{DecimalPlaces}";

        private decimal CurrentValue
        {
            get
            {
                if (decimal.TryParse(Text, NumberStyles.Any, Thread.CurrentThread.CurrentUICulture, out var d))
                {
                    return d;
                }

                return default(decimal);
            }
            set => Text = value.ToString(NumberFormat, Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        ///     Adjustments of value when MinValue or MaxValue are set
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (MinValue != null && CurrentValue < MinValue)
            {
                CurrentValue = MinValue.Value;
            }

            if (MaxValue != null && CurrentValue > MaxValue)
            {
                CurrentValue = MaxValue.Value;
            }

            base.OnLostFocus(e);
        }

        /// <summary>
        ///     Preventing user from typing wrong format string
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            var wholeText = GetWholeText(e.Text);
            var regex =
                $"^(-)?\\d*(\\{Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator}\\d{{0,{DecimalPlaces}}})?$";
            e.Handled = !Regex.IsMatch(wholeText, regex);
        }

        private string GetWholeText(string input)
        {
            var selectionStart = SelectionStart;
            if (Text.Length < selectionStart)
            {
                selectionStart = Text.Length;
            }

            var selectionLength = SelectionLength;
            if (Text.Length < selectionStart + selectionLength)
            {
                selectionLength = Text.Length - selectionStart;
            }

            var currentText = Text.Remove(selectionStart, selectionLength);

            var caretIndex = CaretIndex;
            if (currentText.Length < caretIndex)
            {
                caretIndex = currentText.Length;
            }

            var wholeText = currentText.Insert(caretIndex, input);

            return wholeText;
        }
    }
}