using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter.Dtos;

public class FrankfurterCurrencyRateResultDto
{
    [JsonPropertyName("base")]
    public string Base { get; set; }
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; }
}