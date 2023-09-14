using System;
using System.Globalization;
using System.Windows.Data;

namespace HandNote.Converters
{
    /// <summary>
    /// Converter used to convert InkCanvas editing mode to boolean.
    /// Returns true if given editing mode name and parameter name are same or else retrn false.
    /// </summary>
    public class EditingModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null || parameter is null)
            {
                return false;
            }
            return value.ToString().Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(true))
                return parameter;

            return Binding.DoNothing;
        }
    }
}
