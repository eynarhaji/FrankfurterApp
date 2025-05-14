namespace FrankfurterApp.Services.CurrencyRates.Providers;

public interface ICurrencyRateProviderFactory
{
    ICurrencyRateProvider BuildPaymentProvider();
}