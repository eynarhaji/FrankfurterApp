using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrankfurterApp.Services.CurrencyRates;

public class CurrencyRateServiceSettings
{
    [Required]
    public string PreferredService { get; set; }
    [Required]
    public string BaseCurrency { get; set; }
    [Required]
    public List<string> SupportedCurrencies { get; set; }
}