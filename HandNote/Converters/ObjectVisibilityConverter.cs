using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandNote.Converters
{
    /// <summary>
    /// Converter used to convert object value to visibility in inverse mode.
    /// Returns visibility mode to visible if value is null or else return collapsed.
    /// </summary>
    public class ObjectVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }
}
