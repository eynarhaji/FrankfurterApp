using System;
using System.Threading.Tasks;
using FrankfurterApp.Dtos;

namespace FrankfurterApp.Services.CurrencyRates.Providers;

public interface ICurrencyRateProvider
{
    Task<CurrencyRateResultDto> GetLatestCurrencyRates(string baseCurrency = null);
    Task<HistoricCurrencyRatesResultDto> GetHistoricCurrencyRates(DateTime startDate,
        string baseCurrency = null, DateTime? endDate = null);
}