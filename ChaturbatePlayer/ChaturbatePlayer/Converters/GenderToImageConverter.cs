using ChaturbatePlayer.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChaturbatePlayer.Converters
{
    [ValueConversion(typeof(Gender), typeof(string))]
    public class GenderToImageConverter : IValueConverter
    {
        internal const string IMAGE_URL = "/ChaturbatePlayer;component/Images/{0}";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((Gender)value)
            {
                case Gender.Male:
                    return string.Format(IMAGE_URL, "GenderMale.png");

                case Gender.Female:
                    return string.Format(IMAGE_URL, "GenderFemale.png");

                case Gender.Couple:
                    return string.Format(IMAGE_URL, "GenderCouple.png");

                case Gender.Trans:
                    return string.Format(IMAGE_URL, "GenderTrans.png");

                default:
                    return string.Format(IMAGE_URL, "Star.png");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
