using System;
using System.Collections.Generic;

namespace FrankfurterApp.Dtos;

public class HistoricCurrencyRatesResultDto
{
    public string BaseCurrency { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public List<HistoricCurrencyRateDto> Data { get; set; }
}