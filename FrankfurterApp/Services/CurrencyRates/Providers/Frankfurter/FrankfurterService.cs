using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FrankfurterApp.Dtos;
using FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter.Dtos;

namespace FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter;

public class FrankfurterService : ICurrencyRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly List<string> _supportedCurrencies;

    public FrankfurterService(IHttpClientFactory httpClientFactory, List<string> supportedCurrencies)
    {
        _supportedCurrencies = supportedCurrencies ?? throw new ArgumentNullException(nameof(supportedCurrencies));
        _httpClient =
            (httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory))).CreateClient(
                CurrencyRateServiceNames.Frankfurter);
    }

    public async Task<CurrencyRateResultDto> GetLatestCurrencyRates(string baseCurrency = null)
    {
        var url = "v1/latest";

        if (baseCurrency != null)
            url += $"?base={baseCurrency}";

        if (_supportedCurrencies is { Count: > 0 })
            url += (baseCurrency != null ? "&" : "?") + $"&symbols={string.Join(",", _supportedCurrencies)}";

        using (var response = await _httpClient.GetAsync(url))
        {
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(message: body, inner: null, statusCode: response.StatusCode);

            var result = JsonSerializer.Deserialize<FrankfurterCurrencyRateResultDto>(body);

            if (result == null)
                return new CurrencyRateResultDto()
                {
                    BaseCurrency = baseCurrency,
                    Date = DateTime.UtcNow.Date,
                    Rates = new List<CurrencyRateDto>()
                };

            return new CurrencyRateResultDto()
            {
                Date = result.Date,
                BaseCurrency = result.Base,
                Rates = result.Rates?.Select(x =>
                    new CurrencyRateDto()
                    {
                        Currency = x.Key,
                        Rate = x.Value
                    }).ToList() ?? new List<CurrencyRateDto>()
            };
        }
    }

    public async Task<HistoricCurrencyRatesResultDto> GetHistoricCurrencyRates(DateTime startDate,
        string baseCurrency = null, DateTime? endDate = null)
    {
        var url = $"v1/{startDate:yyyy-MM-dd}..";

        if (endDate != null)
            url += $"{endDate:yyyy-MM-dd}";

        if (baseCurrency != null)
            url += $"?base={baseCurrency}";

        if (_supportedCurrencies is { Count: > 0 })
            url += (baseCurrency != null ? "&" : "?") + $"&symbols={string.Join(",", _supportedCurrencies)}";

        using (var response = await _httpClient.GetAsync(url))
        {
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(message: body, inner: null, statusCode: response.StatusCode);

            var result = JsonSerializer.Deserialize<FrankfurterHistoricCurrencyRateResultDto>(body);

            if (result == null)
                return new HistoricCurrencyRatesResultDto()
                {
                    BaseCurrency = baseCurrency,
                    StartDate = startDate,
                    EndDate = endDate ?? DateTime.UtcNow,
                    Data = new List<HistoricCurrencyRateDto>()
                };

            return new HistoricCurrencyRatesResultDto()
            {
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                BaseCurrency = result.Base,
                Data = result.Rates?.Select(x => new HistoricCurrencyRateDto()
                {
                    Date = x.Key,
                    Rates = x.Value.Select(s => new CurrencyRateDto()
                    {
                        Currency = s.Key,
                        Rate = s.Value
                    }).ToList()
                }).ToList() ?? new List<HistoricCurrencyRateDto>()
            };
        }
    }
}