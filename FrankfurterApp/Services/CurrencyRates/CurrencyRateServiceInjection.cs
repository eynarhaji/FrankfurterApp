using FrankfurterApp.Extensions;
using FrankfurterApp.Services.CurrencyRates.Providers;
using FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Services.CurrencyRates;

public static class CurrencyRateServiceInjection
{
    public static IServiceCollection AddCurrencyRateService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSettingsToOptions(configuration, "CurrencyRatesService", out CurrencyRateServiceSettings _);

        services.AddFrankfurterService(configuration);
        
        services.AddSingleton<ICurrencyRateService, CurrencyRateService>();
        services.AddSingleton<ICurrencyRateProviderFactory, CurrencyRateProviderFactory>();

        return services;
    }
}