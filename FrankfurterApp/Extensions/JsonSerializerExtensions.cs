using System.Text.Json;

namespace FrankfurterApp.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static string JsonSerialize<T>(this T stringToSerialize)
    {
        return JsonSerializer.Serialize(stringToSerialize);
    }
    
        public static string JsonSerializeToCamelCase<T>(this T stringToSerialize)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(stringToSerialize, options);
    }
    
        public static string JsonSerializeToSnakeCase<T>(this T stringToSerialize)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
        };

        return JsonSerializer.Serialize(stringToSerialize, options);
    }
    
        public static string JsonSerializeToCustomOptions<T>(this T stringToSerialize, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(stringToSerialize, options);
    }

        public static T JsonDeserialize<T>(this string json)
    {       
        return JsonSerializer.Deserialize<T>(json);
    }

        public static T JsonDeserializeFromCamelCase<T>(this string json)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        return JsonSerializer.Deserialize<T>(json, options);
    }

        public static T JsonDeserializeFromSnakeCase<T>(this string json)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
        };
        
        return JsonSerializer.Deserialize<T>(json, options);
    }

        public static T JsonDeserializeFromCustomOptions<T>(this string json, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }
    }
}