using System.Collections.Generic;
using System.Globalization;
using ProjectCeilidh.Ceilidh.Standard.Localization;
using ProjectCeilidh.Ceilidh.XamarinShell.Cobble;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Localization
{
    [CobbleExport]
    public class BaseXamarinLocalizationPhraseProvider : ILocalizationPhraseProvider
    {
        public IReadOnlyDictionary<string, string[]> GetPhrases(CultureInfo culture)
        {
            switch (culture)
            {
                case var c when c.TwoLetterISOLanguageName == "en" || c.Equals(CultureInfo.InvariantCulture):
                    return new Dictionary<string, string[]>
                    {
                        ["xamarin.addComponent"] = new [] { "Add Component" }
                    };
                default:
                    return null;
            }
        }
    }
}
