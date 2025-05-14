using Microsoft.Extensions.DependencyInjection;

namespace FrankfurterApp.Services.Exchange;

public static class ExchangeServiceInjection
{
    public static IServiceCollection AddExchangeService(this IServiceCollection services)
    {
        services.AddScoped<IExchangeService, ExchangeService>();

        return services;
    }
}