using System;
using System.Collections.Generic;
using System.Linq;
using FrankfurterApp.Dtos;
using FrankfurterApp.Services.CurrencyRates.Providers;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FrankfurterApp.Tests.Configuration;

public static class MockServices
{
    public static void AddMockFrankfurterApp(this IServiceCollection collection)
    {
        var currencies = new[] { MockData.MockCurrencyUsd };
        var today = MockData.MockDate;
        var random = new Random();
        var data = new List<HistoricCurrencyRateDto>();
        var days = (MockData.MockEndDate - MockData.MockStartDate).Days + 1;

        for (int i = 0; i < days; i++)
        {
            var date = today.AddDays(-i);
            var rates = currencies.Select(c => new CurrencyRateDto
            {
                Currency = c,
                Rate = Math.Round((decimal)(random.NextDouble() * (1.5 - 0.5) + 0.5), 4)
            }).ToList();

            data.Add(new HistoricCurrencyRateDto
            {
                Date = date,
                Rates = rates
            });
        }

        var currencyRateProviderFactory = new Mock<ICurrencyRateProvider>();

        currencyRateProviderFactory.Setup(x => x.GetLatestCurrencyRates(MockData.MockCurrencyEur))
            .ReturnsAsync(new CurrencyRateResultDto
            {
                BaseCurrency = MockData.MockCurrencyEur,
                Date = MockData.MockDate,
                Rates = new List<CurrencyRateDto>
                {
                    new CurrencyRateDto { Currency = MockData.MockCurrencyUsd, Rate = MockData.MockRateEurToUsd }
                }
            });
        currencyRateProviderFactory.Setup(x => x.GetLatestCurrencyRates(MockData.MockCurrencyUsd))
            .ReturnsAsync(new CurrencyRateResultDto
            {
                BaseCurrency = MockData.MockCurrencyUsd,
                Date = MockData.MockDate,
                Rates = new List<CurrencyRateDto>
                {
                    new CurrencyRateDto { Currency = MockData.MockCurrencyEur, Rate = MockData.MockRateUsdToEur }
                }
            });
        currencyRateProviderFactory.Setup(x => x.GetHistoricCurrencyRates(MockData.MockStartDate,
                It.IsAny<string>(), MockData.MockEndDate))
            .ReturnsAsync(() => new HistoricCurrencyRatesResultDto()
            {
                StartDate = MockData.MockStartDate,
                EndDate = MockData.MockEndDate,
                BaseCurrency = MockData.MockCurrencyEur,
                Data = data
            });
        currencyRateProviderFactory.Setup(x => x.GetHistoricCurrencyRates(MockData.MockCustomDate,
                It.IsAny<string>(), MockData.MockCustomDate))
            .ReturnsAsync(() => new HistoricCurrencyRatesResultDto()
            {
                StartDate = MockData.MockCustomDate,
                EndDate = MockData.MockCustomDate,
                BaseCurrency = MockData.MockCurrencyEur,
                Data = [data.FirstOrDefault(x => x.Date == MockData.MockCustomDate)]
            });

        var mock = new Mock<ICurrencyRateProviderFactory>();
        mock.Setup(x => x.BuildPaymentProvider())
            .Returns(currencyRateProviderFactory.Object);

        collection.AddSingleton(mock.Object);
    }
}