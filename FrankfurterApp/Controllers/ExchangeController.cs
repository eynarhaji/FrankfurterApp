using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Asp.Versioning;
using FrankfurterApp.Authentication;
using FrankfurterApp.Dtos;
using FrankfurterApp.Services.CurrencyRates;
using FrankfurterApp.Services.Exchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrankfurterApp.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ExchangeController : BaseController
{
    private readonly ICurrencyRateService _currencyRateService;
    private readonly IExchangeService _exchangeService;

    public ExchangeController(ICurrencyRateService currencyRateService, IExchangeService exchangeService)
    {
        _currencyRateService = currencyRateService ?? throw new ArgumentNullException(nameof(currencyRateService));
        _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
    }

    /// <summary>
    /// Get the latest exchange rates.
    /// </summary>
    /// <param name="baseCurrency">The base currency for the exchange rates.</param>
    /// <returns>A list of the latest exchange rates.</returns>
    [Authorize]
    [HttpGet("rates/latest")]
    public async Task<ActionResult<CurrencyRateResultDto>> GetLatestExchangeRates(
        [FromQuery] string baseCurrency = null)
    {
        var result = await _currencyRateService.GetLatestCurrencyRates(baseCurrency);

        return Ok(result);
    }

    /// <summary>
    /// Get exchange rates for a specific date.
    /// </summary>
    /// <param name="baseCurrency">The base currency for the exchange rates.</param>
    /// <param name="date">The date for which to get the exchange rates.</param>
    /// <returns>A list of exchange rates for the specified date.</returns>
    [Authorize]
    [HttpGet("rates/{date}")]
    public async Task<ActionResult<CurrencyRateResultDto>> GetExchangeRates(
        [FromRoute] [DefaultValue("2025-05-01")]
        DateTime date,
        [FromQuery] string baseCurrency = null)
    {
        var result = await _currencyRateService.GetHistoricCurrencyRate(date, baseCurrency);

        return Ok(result);
    }

    /// <summary>
    /// Get the historic exchange rates.
    /// </summary>
    /// <param name="startDate">The start date of historic rates.</param>
    /// <param name="baseCurrency">The base currency for the exchange rates.</param>
    /// <param name="endDate">The end date of historic rates.</param>
    /// <param name="pageSize">Count of dates for single page.</param>
    /// <param name="page">Number of page.</param>
    /// <returns>A list of the historic exchange rates.</returns>
    [Authorize(Roles = UserRole.Administrator)]
    [HttpGet("rates/historic")]
    public async Task<ActionResult<HistoricCurrencyRatesResultDto>> GetHistoricCurrencyRates(
        [FromQuery] [DefaultValue("2025-05-01")]
        DateTime startDate,
        [FromQuery] string baseCurrency = null, [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageSize = 10, [FromQuery] int page = 1)
    {
        var result =
            await _currencyRateService.GetHistoricCurrencyRates(startDate, baseCurrency, endDate, pageSize, page);

        return Ok(result);
    }

    /// <summary>
    /// Get converted amount to a specific currency.
    /// </summary>
    /// <param name="amount">The amount to be converted.</param>
    /// <param name="sourceCurrency">The currency of the amount.</param>
    /// <param name="targetCurrency">The target currency for conversion.</param>
    /// <returns>A list of exchange rates for the specified date.</returns>
    [Authorize(Roles = UserRole.User)]
    [HttpGet("amountConversion")]
    public async Task<ActionResult<ExchangeResultDto>> ConvertAmount([FromQuery] [DefaultValue(100)] decimal amount,
        [FromQuery] [DefaultValue("EUR")] string sourceCurrency,
        [FromQuery] [DefaultValue("USD")] string targetCurrency)
    {
        var result = await _exchangeService.ConvertAmount(amount, sourceCurrency, targetCurrency);

        return Ok(result);
    }
}