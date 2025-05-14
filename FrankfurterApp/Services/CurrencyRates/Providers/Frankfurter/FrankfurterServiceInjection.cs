using System;
using FrankfurterApp.Extensions;
using FrankfurterApp.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter;

public static class FrankfurterServiceInjection
{
    public static IServiceCollection AddFrankfurterService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.ExtractSettings<FrankfurterServiceSettings>("FrankfurterService");
        
        services.AddHttpClient(CurrencyRateServiceNames.Frankfurter, client =>
            {
                client.BaseAddress = new Uri(settings.Url);
                client.Timeout = TimeSpan.FromSeconds(settings.Timeout);
            })
            .AddPolicyHandler(RetryPolicyFactory.GetRetryPolicy())
            .AddPolicyHandler(RetryPolicyFactory.GetCircuitBreakerPolicy());

        return services;
    }
}