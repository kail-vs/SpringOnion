using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SpringOnion.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://<your-func-app-name>.azurewebsites.net/api";
        // replace with your actual Function App URL

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> RegisterAsync(string userId, string password)
        {
            var payload = new { userId, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/register", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string userId, string password)
        {
            var payload = new { userId, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);
            return response.IsSuccessStatusCode;
        }
    }
}
