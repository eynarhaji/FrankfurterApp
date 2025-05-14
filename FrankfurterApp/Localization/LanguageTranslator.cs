using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FrankfurterApp.Localization.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace FrankfurterApp.Localization
{
    internal class LanguageTranslator : ILanguageTranslator
    {
        private readonly IStringLocalizer _baseLocalizer;
        private readonly LocalizationSettings _settings;

        public CultureInfo DefaultCulture => new(_settings.DefaultCulture);
        public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;
        public List<string> SupportedCultures => _settings.SupportedCultures.ToList();

        public LanguageTranslator(IStringLocalizerFactory localizer, IOptions<LocalizationSettings> settings)
        {
            _settings = (settings ?? throw new ArgumentNullException(nameof(settings))).Value;
            _baseLocalizer = (localizer ?? throw new ArgumentNullException(nameof(localizer))).Create(
                "Localization.Resources.SharedResource", typeof(SharedResourceModel).Assembly.FullName ?? "");
        }

        public string Translate(string key) => !string.IsNullOrEmpty(key)
            ? (string)_baseLocalizer.GetString(key)
            : throw new ArgumentNullException(nameof(key), "Key can not be null!");

        public bool IsSupported(CultureInfo cultureInfo) =>
            cultureInfo != null && _settings.GetSupportedCultureInfos().Contains(cultureInfo);
    }
}