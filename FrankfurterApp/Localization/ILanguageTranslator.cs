using System.Collections.Generic;
using System.Globalization;

namespace FrankfurterApp.Localization
{
    public interface ILanguageTranslator
    {
        CultureInfo DefaultCulture { get; }
        CultureInfo CurrentCulture { get; }
        List<string> SupportedCultures { get; }
        bool IsSupported(CultureInfo cultureInfo);
        string Translate(string key);
    }
}