using System;
using System.Globalization;
using ProjectCeilidh.Ceilidh.Standard.Localization;
using ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh;
using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Localization
{
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return CeilidhLoader.Instance.TryGetSingleton(out ILocalizationController localization)
                ? localization.Translate(System.Convert.ToString(value), parameter)
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
