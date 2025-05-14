using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter.Dtos;

public class FrankfurterHistoricCurrencyRateResultDto
{
    [JsonPropertyName("base")]
    public string Base { get; set; }
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }
    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }
    [JsonPropertyName("rates")]
    public Dictionary<DateTime, Dictionary<string, decimal>> Rates { get; set; }
}