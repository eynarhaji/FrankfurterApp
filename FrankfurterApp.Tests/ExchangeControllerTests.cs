using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FrankfurterApp.Authentication;
using FrankfurterApp.Dtos;
using FrankfurterApp.Tests.Configuration;
using NUnit.Framework;

namespace FrankfurterApp.Tests
{
    [Order(2)]
    public class ExchangeControllerTests
    {
        private readonly HttpClient _client;

        public ExchangeControllerTests()
        {
            _client = HttpClientHelper.GenerateHttpClient();
        }

        [Test, Order(1)]
        public async Task GET_GetLatestExchangeRates()
        {
            MockData.TestUserRole = UserRole.User;

            var response =
                await _client.GetAsync($"/api/v1/exchange/rates/latest");

            response.IsSuccessStatusCode.Should().BeTrue();

            var model = await HttpClientHelper.EvaluateResponse<CurrencyRateResultDto>(response);

            model.BaseCurrency.Should().Be(MockData.MockCurrencyEur);
            model.Date.Should().BeOnOrBefore(MockData.MockDate);
            model.Rates.Should().HaveCount(1);
            model.Rates.First().Currency.Should().Be(MockData.MockCurrencyUsd);
        }

        [Test, Order(1)]
        public async Task GET_GetLatestExchangeRates_With_Non_Default_Base_Currency()
        {
            MockData.TestUserRole = UserRole.User;

            var response =
                await _client.GetAsync($"/api/v1/exchange/rates/latest?baseCurrency=USD");

            response.IsSuccessStatusCode.Should().BeTrue();

            var model = await HttpClientHelper.EvaluateResponse<CurrencyRateResultDto>(response);

            model.BaseCurrency.Should().Be(MockData.MockCurrencyUsd);
            model.Date.Should().BeOnOrBefore(MockData.MockDate);
            model.Rates.Should().HaveCount(1);
            model.Rates.First().Currency.Should().Be(MockData.MockCurrencyEur);
        }

        [Test, Order(1)]
        public async Task GET_GetExchangeRates()
        {
            MockData.TestUserRole = UserRole.Administrator;

            var date = DateTime.UtcNow.Date.AddDays(-7);
            var response =
                await _client.GetAsync($"/api/v1/exchange/rates/{date:yyyy-MM-dd}");

            response.IsSuccessStatusCode.Should().BeTrue();

            var model = await HttpClientHelper.EvaluateResponse<CurrencyRateResultDto>(response);

            model.BaseCurrency.Should().Be(MockData.MockCurrencyEur);
            model.Date.Should().BeOnOrBefore(date);
            model.Rates.Should().HaveCount(1);
            model.Rates.First().Currency.Should().Be(MockData.MockCurrencyUsd);
        }

        [Test, Order(1)]
        public async Task GET_GetHistoricCurrencyRates()
        {
            MockData.TestUserRole = UserRole.Administrator;

            var startDate = MockData.MockStartDate;
            var endDate = MockData.MockEndDate;
            var response =
                await _client.GetAsync(
                    $"/api/v1/exchange/rates/historic?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&pageSize=10&page=2");

            response.IsSuccessStatusCode.Should().BeTrue();

            var model = await HttpClientHelper.EvaluateResponse<HistoricCurrencyRatesResultDto>(response);

            model.BaseCurrency.Should().Be("EUR");
            model.StartDate.Should().BeOnOrAfter(startDate);
            model.EndDate.Should().BeOnOrBefore(endDate);
            model.PageSize.Should().Be(10);
            model.Page.Should().Be(2);
        }

        [Test, Order(1)]
        public async Task GET_ConvertAmount()
        {
            MockData.TestUserRole = UserRole.User;

            var amount = 100;
            var sourceCurrency = MockData.MockCurrencyEur;
            var destinationCurrency = MockData.MockCurrencyUsd;
            var response =
                await _client.GetAsync(
                    $"/api/v1/exchange/amountConversion?amount={amount}&sourceCurrency={sourceCurrency}&targetCurrency={destinationCurrency}");

            response.IsSuccessStatusCode.Should().BeTrue();

            var model = await HttpClientHelper.EvaluateResponse<ExchangeResultDto>(response);
            
            model.SourceCurrency.Should().Be(sourceCurrency);
            model.TargetCurrency.Should().Be(destinationCurrency);
            model.AmountInSourceCurrency.Should().Be(amount);
            model.AmountInTargetCurrency.Should().Be(amount * model.ExchangeRate);
        }
    }
}