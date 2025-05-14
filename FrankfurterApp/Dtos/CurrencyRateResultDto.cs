using System;
using System.Collections.Generic;

namespace FrankfurterApp.Dtos;

public class CurrencyRateResultDto
{
    public string BaseCurrency { get; set; }
    public DateTime Date { get; set; }
    public List<CurrencyRateDto> Rates { get; set; }
}