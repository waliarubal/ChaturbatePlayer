using ChaturbatePlayer.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChaturbatePlayer.Converters
{
    [ValueConversion(typeof(LiveStreamPlayerState), typeof(Visibility))]
    public class LiveStreamPlayerStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((LiveStreamPlayerState)value)
            {
                case LiveStreamPlayerState.Playing:
                    return Visibility.Visible;

                default:
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
