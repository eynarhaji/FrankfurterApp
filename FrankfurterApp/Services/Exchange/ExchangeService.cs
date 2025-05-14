using System;
using System.Linq;
using System.Threading.Tasks;
using FrankfurterApp.Dtos;
using FrankfurterApp.ErrorHandling.Exceptions;
using FrankfurterApp.Services.CurrencyRates;

namespace FrankfurterApp.Services.Exchange;

public class ExchangeService : IExchangeService
{
    private readonly ICurrencyRateService _currencyRateService;

    public ExchangeService(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService ?? throw new ArgumentNullException(nameof(currencyRateService));
    }

    public async Task<ExchangeResultDto> ConvertAmount(decimal amountInSourceCurrency, string sourceCurrency, string targetCurrency)
    {
        var rates = await _currencyRateService.GetLatestCurrencyRates(sourceCurrency);

        var targetCurrencyRateToBaseCurrency = rates.Rates.FirstOrDefault(x => x.Currency == targetCurrency);
        
        if (targetCurrencyRateToBaseCurrency == null)
        {
            throw new BusinessLogicException($"Currency {targetCurrency} is not supported");
        }
        
        var exchangeRate = targetCurrencyRateToBaseCurrency.Rate;
        
        return new ExchangeResultDto
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            ExchangeRate = exchangeRate,
            AmountInSourceCurrency = amountInSourceCurrency,
            AmountInTargetCurrency = amountInSourceCurrency * exchangeRate
        };
    }
}