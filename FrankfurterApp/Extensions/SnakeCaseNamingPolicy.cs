using System.Text.Json;

namespace FrankfurterApp.Extensions
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new();

        public override string ConvertName(string name)
        {
            return name.FromCamelToSnakeCase();
        }
    }
}