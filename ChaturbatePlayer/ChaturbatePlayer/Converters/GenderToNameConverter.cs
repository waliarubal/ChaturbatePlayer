using ChaturbatePlayer.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChaturbatePlayer.Converters
{
    [ValueConversion(typeof(Gender), typeof(string))]
    public class GenderToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Gender)value)
            {
                case Gender.Male:
                    return "MALE";

                case Gender.Female:
                    return "FEMALE";

                case Gender.Couple:
                    return "COUPLE";

                case Gender.Trans:
                    return "TRANS";

                default:
                    return "FEATURED";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
