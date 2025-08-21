using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.Configuration;

namespace SpringOnion.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        private const string TokenKey = "auth_token";

        public string? Token { get; private set; }

        public string BaseUrl => _baseUrl;

        public AuthenticationService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _baseUrl = configuration["AuthApiBaseUrl"]
                       ?? throw new InvalidOperationException("AuthApiBaseUrl not configured.");
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string userId, string password)
        {
            var payload = new { userId, password };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", payload);

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
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", payload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
                {
                    Token = result.Token;
                    await SecureStorage.SetAsync(TokenKey, Token);
                    return (true, result.Message);
                }

                return (false, result?.Message ?? "Unknown error");
            }

            return (false, "Server error");
        }

        public async Task<bool> LoadTokenAsync()
        {
            Token = await SecureStorage.GetAsync(TokenKey);
            return !string.IsNullOrEmpty(Token);
        }

        public async Task<(bool Success, string? Data)> GetProfileAsync()
        {
            if (string.IsNullOrEmpty(Token))
                return (false, "No token available");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            var response = await _httpClient.GetAsync($"{BaseUrl}/profile");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return (result?.Success ?? false, result?.Message);
            }

            return (false, "Failed to fetch profile");
        }

        private class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        private class LoginResponse : ApiResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
