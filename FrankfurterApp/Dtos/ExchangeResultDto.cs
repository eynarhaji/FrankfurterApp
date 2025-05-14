namespace FrankfurterApp.Dtos;

public class ExchangeResultDto
{
    public string SourceCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal AmountInSourceCurrency { get; set; }
    public decimal AmountInTargetCurrency { get; set; }
}