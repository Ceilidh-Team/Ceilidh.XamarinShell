using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Converters
{
    public class EqualToConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
