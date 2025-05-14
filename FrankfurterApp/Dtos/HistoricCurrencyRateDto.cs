using System;
using System.Collections.Generic;

namespace FrankfurterApp.Dtos;

public class HistoricCurrencyRateDto
{
    public DateTime Date { get; set; }
    public List<CurrencyRateDto> Rates { get; set; }
}