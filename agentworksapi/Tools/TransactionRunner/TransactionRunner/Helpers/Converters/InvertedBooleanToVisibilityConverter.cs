using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TransactionRunner.Helpers.Converters
{
    /// <summary>
    /// Converter for Boolean -> Visibility conversion (inverted version)
    /// </summary>
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts Boolean to Visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}