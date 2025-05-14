using System.Threading.Tasks;
using FrankfurterApp.Dtos;

namespace FrankfurterApp.Services.Exchange;

public interface IExchangeService
{
    Task<ExchangeResultDto> ConvertAmount(decimal amountInSourceCurrency, string sourceCurrency, string targetCurrency);
}