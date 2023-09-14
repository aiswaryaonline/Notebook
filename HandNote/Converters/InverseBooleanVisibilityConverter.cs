using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandNote.Converters
{
    /// <summary>
    /// Converter used to convert boolean to visibility in inverse mode.
    /// Returns visibility mode to visible if value is false or else return collapsed.
    /// </summary>
    public class InverseBooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(false) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }
}
