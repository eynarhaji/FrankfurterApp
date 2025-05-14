using System;
using System.Threading.Tasks;
using FrankfurterApp.Dtos;

namespace FrankfurterApp.Services.CurrencyRates;

public interface ICurrencyRateService
{
    Task<CurrencyRateResultDto> GetLatestCurrencyRates(string baseCurrency = null);
    Task<CurrencyRateResultDto> GetHistoricCurrencyRate(DateTime date, string baseCurrency = null);
    Task<HistoricCurrencyRatesResultDto> GetHistoricCurrencyRates(DateTime startDate,
        string baseCurrency = null, DateTime? endDate = null, int pageSize = 10, int page = 1);
}