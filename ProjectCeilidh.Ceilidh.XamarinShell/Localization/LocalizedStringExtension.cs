using System;
using ProjectCeilidh.Ceilidh.Standard.Localization;
using ProjectCeilidh.Ceilidh.XamarinShell.Ceilidh;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Localization
{
    public class LocalizedStringExtension : BindableObject, IMarkupExtension<string>
    {
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(string), typeof(LocalizedStringExtension));
        public static readonly BindableProperty ArgumentProperty = BindableProperty.Create(nameof(Argument), typeof(object), typeof(LocalizedStringExtension));

        public string Key
        {
            get => (string)GetValue(KeyProperty);
            set => SetValue(KeyProperty, value);
        }

        public object Argument
        {
            get => GetValue(ArgumentProperty);
            set => SetValue(ArgumentProperty, value);
        }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return CeilidhLoader.Instance.TryGetSingleton(out ILocalizationController localization)
                ? localization.Translate(Key, Argument)
                : null;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
