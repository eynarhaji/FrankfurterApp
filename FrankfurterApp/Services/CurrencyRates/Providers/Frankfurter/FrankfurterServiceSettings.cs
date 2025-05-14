using System.ComponentModel.DataAnnotations;

namespace FrankfurterApp.Services.CurrencyRates.Providers.Frankfurter;

public class FrankfurterServiceSettings
{
    [Required]
    [Url]
    public string Url { get; set; }
    [Required]
    public int Timeout { get; set; }
}