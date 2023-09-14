using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandNote.Converters
{
    /// <summary>
    /// Converter used to convert boolean to visibility.
    /// Returns visibility mode to visible if value is true or else return collapsed.
    /// </summary>
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }
}
