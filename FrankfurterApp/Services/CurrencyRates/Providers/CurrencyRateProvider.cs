using System;
using System.Net.Http;
using FrankfurterApp.ErrorHandling.Exceptions;
using FrankfurterApp.Localization;
using FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter;
using Microsoft.Extensions.Options;

namespace FrankfurterApp.Services.CurrencyRates.Providers;

public class CurrencyRateProviderFactory : ICurrencyRateProviderFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private CurrencyRateServiceSettings _currencyRateServiceSettings;
    
    public CurrencyRateProviderFactory(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<CurrencyRateServiceSettings> currencyRateServiceSettings)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _currencyRateServiceSettings = currencyRateServiceSettings?.CurrentValue
                                       ?? throw new ArgumentNullException(nameof(currencyRateServiceSettings));

        currencyRateServiceSettings.OnChange(options => { _currencyRateServiceSettings = options; });
    }
    
    public ICurrencyRateProvider BuildPaymentProvider()
    {
        if (_currencyRateServiceSettings.PreferredService == "Frankfurter")
        {
            return new FrankfurterService(_httpClientFactory, _currencyRateServiceSettings.SupportedCurrencies);
        }

        throw new BusinessLogicException(LocalizationStrings.CurrencyRateServiceNotFound);
    }
}