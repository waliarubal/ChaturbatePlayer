using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChaturbatePlayer.Converters
{
    /// <summary>
    /// This converter returns Visibility.Visible if value passed is 0.
    /// </summary>
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class IntegerToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) > 0)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
