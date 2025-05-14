using System.Threading.Tasks;

namespace FrankfurterApp.Cache;

public interface ICacheStorage
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
    Task RemoveAsync(string key);
}