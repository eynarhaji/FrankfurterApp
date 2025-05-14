using FrankfurterApp.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Localization
{
    public static class LocalizationInjection
    {
        public static IServiceCollection AddLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettingsToOptions(configuration, "Localization", out LocalizationSettings settings);
            
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(settings.DefaultCulture);
                options.AddSupportedCultures(settings.SupportedCultures);
            });

            services.AddSingleton(settings);
            services.AddSingleton<ILanguageTranslator, LanguageTranslator>();
            return services;
        }
    }
}