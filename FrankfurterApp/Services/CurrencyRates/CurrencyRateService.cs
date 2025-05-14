using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FrankfurterApp.Cache;
using FrankfurterApp.Dtos;
using FrankfurterApp.ErrorHandling.Exceptions;
using FrankfurterApp.Localization;
using FrankfurterApp.Services.CurrencyRates.Providers;
using Microsoft.Extensions.Options;

namespace FrankfurterApp.Services.CurrencyRates;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRateProviderFactory _currencyRateProviderFactory;
    private CurrencyRateServiceSettings _currencyRateServiceSettings;
    private readonly ICacheStorage _cacheStorage;
    private readonly ConcurrentDictionary<string, DateTime> _cachedValues;

    public CurrencyRateService(
        IOptionsMonitor<CurrencyRateServiceSettings> currencyRateServiceSettings, ICacheStorage cacheStorage,
        ICurrencyRateProviderFactory currencyRateProviderFactory)
    {
        _currencyRateProviderFactory = currencyRateProviderFactory 
                                       ?? throw new ArgumentNullException(nameof(currencyRateProviderFactory));
        _cacheStorage = cacheStorage ?? throw new ArgumentNullException(nameof(cacheStorage));
        _currencyRateServiceSettings = currencyRateServiceSettings?.CurrentValue
                                       ?? throw new ArgumentNullException(nameof(currencyRateServiceSettings));

        currencyRateServiceSettings.OnChange(options => { _currencyRateServiceSettings = options; });
        _cachedValues = new ConcurrentDictionary<string, DateTime>();
    }

    public async Task<CurrencyRateResultDto> GetLatestCurrencyRates(string baseCurrency = null)
    {
        baseCurrency ??= _currencyRateServiceSettings.BaseCurrency;
        if (!CurrencyExists(baseCurrency))
        {
            throw new BusinessLogicException(LocalizationStrings.CurrencyNotFound, baseCurrency);
        }

        var service = _currencyRateProviderFactory.BuildPaymentProvider();

        // Caching not implemented to provide always fresh rates.

        return await service.GetLatestCurrencyRates(baseCurrency);
    }

    public async Task<CurrencyRateResultDto> GetHistoricCurrencyRate(DateTime date, string baseCurrency = null)
    {
        baseCurrency ??= _currencyRateServiceSettings.BaseCurrency;

        if (!CurrencyExists(baseCurrency))
        {
            throw new BusinessLogicException(LocalizationStrings.CurrencyNotFound, baseCurrency);
        }

        var service = _currencyRateProviderFactory.BuildPaymentProvider();

        var result = await service.GetHistoricCurrencyRates(date, baseCurrency, date);

        return new CurrencyRateResultDto()
        {
            Date = date,
            BaseCurrency = baseCurrency,
            Rates = result.Data.FirstOrDefault(x => x.Date.Date == date.Date)?.Rates ?? []
        };
    }

    public async Task<HistoricCurrencyRatesResultDto> GetHistoricCurrencyRates(DateTime startDate,
        string baseCurrency = null, DateTime? endDate = null, int pageSize = 10, int page = 1)
    {
        baseCurrency ??= _currencyRateServiceSettings.BaseCurrency;
        endDate ??= DateTime.UtcNow.Date;
        var key = $"HistoricCurrencyRate_{baseCurrency}";
        var startKey = "HistoricCurrencyRate_StartDate";
        var endKey = "HistoricCurrencyRate_EndDate";

        var result = await _cacheStorage.GetAsync<HistoricCurrencyRatesResultDto>(key);

        // Check if the cache is empty. If so, we need to fetch the data.
        if (result == null)
        {
            if (!CurrencyExists(baseCurrency))
            {
                throw new BusinessLogicException(LocalizationStrings.CurrencyNotFound, baseCurrency);
            }

            var service = _currencyRateProviderFactory.BuildPaymentProvider();

            result = await service.GetHistoricCurrencyRates(startDate, baseCurrency, endDate);

            CacheResult();
        }
        else
        {
            // Read previously cached date range
            var hasStart = _cachedValues.TryGetValue(startKey, out var cachedStart);
            var hasEnd = _cachedValues.TryGetValue(endKey, out var cachedEnd);

            if (!hasStart || !hasEnd)
            {
                // If cache is corrupt or incomplete, invalidate and re-fetch
                await _cacheStorage.RemoveAsync(key);
                return await GetHistoricCurrencyRates(startDate, baseCurrency, endDate);
            }

            var serviceToUse = _currencyRateProviderFactory.BuildPaymentProvider();

            var needUpdate = false;

            // Fetch missing range BEFORE cached start
            if (startDate < cachedStart)
            {
                var missingBefore =
                    await serviceToUse.GetHistoricCurrencyRates(startDate, baseCurrency, cachedStart.AddDays(-1));
                result.StartDate = startDate;
                result.Data.AddRange(missingBefore.Data);

                needUpdate = true;
            }

            // Fetch missing range AFTER cached end
            if (endDate > cachedEnd)
            {
                var missingAfter =
                    await serviceToUse.GetHistoricCurrencyRates(cachedEnd.AddDays(1), baseCurrency, endDate);
                result.EndDate = endDate.Value;
                result.Data.AddRange(missingAfter.Data);

                needUpdate = true;
            }

            if (needUpdate)
            {
                CacheResult();
            }
        }

        // Extract only requested date range from merged result
        var filteredRates = result.Data
            .Where(x => x.Date >= startDate && x.Date <= endDate)
            .OrderByDescending(x => x.Date)
            .Skip((page - 1) * pageSize).Take(pageSize);

        return new HistoricCurrencyRatesResultDto
        {
            BaseCurrency = result.BaseCurrency,
            StartDate = startDate,
            EndDate = endDate.Value,
            Data = filteredRates.ToList(),
            Page = page,
            PageSize = pageSize,
            PageCount = (int)Math.Ceiling((double)result.Data.Count / pageSize)
        };

        void CacheResult()
        {
            _cacheStorage.SetAsync(key, result);
            _cachedValues.AddOrUpdate(startKey, startDate, (_, _) => startDate);
            _cachedValues.AddOrUpdate(endKey, endDate.Value, (_, _) => endDate.Value);
        }
    }

    private bool CurrencyExists(string currency)
    {
        return _currencyRateServiceSettings.SupportedCurrencies.Contains(currency);
    }
}