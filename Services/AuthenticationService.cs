using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpringOnion.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Environment.GetEnvironmentVariable("AuthApiBaseUrl")
                       ?? "http://localhost:7143/api";

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string userId, string password)
        {
            var payload = new { userId, password };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/register", payload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return (result?.Success ?? false, result?.Message ?? "Unknown error");
            }

            return (false, "Server error");
        }

        public async Task<(bool Success, string Message)> LoginAsync(string userId, string password)
        {
            var payload = new { userId, password };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/login", payload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return (result?.Success ?? false, result?.Message ?? "Unknown error");
            }

            return (false, "Server error");
        }

        private class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }
}
