using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using FrankfurterApp.ErrorHandling;
using FrankfurterApp.Tests.Authorization;

namespace FrankfurterApp.Tests.Configuration
{
    public static class HttpClientHelper
    {
        private static HttpClient Client { get; set; } = null;
        
        public static HttpClient GenerateHttpClient()
        {
            if (Client != null)
                return Client;
            
            var application = new ApiWebApplicationFactory();
            var client = application.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

            Client = client;
            
            return client;
        }

        public static async Task<TDto> EvaluateResponse<TDto>(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStringAsync();
                
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<TDto>(stream, options);
        }

        public static async Task<ErrorResponse> EvaluateErrorResponse(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStringAsync();
                
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<ErrorResponse>(stream, options);
        }
    }
}