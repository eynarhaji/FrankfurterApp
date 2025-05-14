using System.Collections.Generic;
using System.Globalization;

namespace FrankfurterApp.Localization
{
    public class LocalizationSettings
    {
        public string DefaultCulture { get; set; }
        public string[] SupportedCultures { get; set; }

        internal HashSet<CultureInfo> GetSupportedCultureInfos()
        {
            var cultureInfoSet = new HashSet<CultureInfo>();
            foreach (var supportedCulture in SupportedCultures)
                cultureInfoSet.Add(new CultureInfo(supportedCulture));
            return cultureInfoSet;
        }
    }
}