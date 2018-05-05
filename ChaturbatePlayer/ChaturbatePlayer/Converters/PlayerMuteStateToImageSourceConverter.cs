using System;
using System.Globalization;
using System.Windows.Data;

namespace ChaturbatePlayer.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class PlayerMuteStateToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return string.Format(GenderToImageConverter.IMAGE_URL, "PlayerUnmute.png");
            else
                return string.Format(GenderToImageConverter.IMAGE_URL, "PlayerMute.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
