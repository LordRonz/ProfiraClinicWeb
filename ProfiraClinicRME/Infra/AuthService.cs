using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinicRME.Utils;
using ProfiraClinic.Models.Api;

namespace ProfiraClinicRME.Infra
{
    // Model for login/register requests
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? KodeLokasi { get; set; }
    }

    // Model for response containing the JWT token
    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class AuthService
    {
        private readonly HttpClient _httpClient;

        private string _classPath = "AuthApiService";

        private ApiService _svcApi;

        // Inject the HttpClient (configured in Program.cs or Startup.cs)
        public AuthService(HttpClient httpClient, ApiService svcApi)
        {
            _httpClient = httpClient;
            _svcApi = svcApi;
        }

        /// <summary>
        /// Logs in a user and returns the JWT token.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>The JWT token string.</returns>
        public async Task<string> LoginAsync(LoginModel model)
        {
            LogTrace.Info("init", model, _classPath);
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Login failed: {response.StatusCode} - {error}");
            }

            var authResult = await response.Content.ReadFromJsonAsync<Response<AuthResponse>>();
            if (authResult == null || string.IsNullOrEmpty(authResult.Data.Token))
            {
                throw new HttpRequestException("Authentication succeeded but token was not returned.");
            }
            _svcApi.SetBearer(authResult.Data.Token);
            return authResult.Data.Token;
        }

        /// <summary>
        /// Registers a new user and returns the JWT token.
        /// </summary>
        /// <param name="model">The registration credentials.</param>
        /// <returns>The JWT token string.</returns>
        public async Task<string> RegisterAsync(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Registration failed: {response.StatusCode} - {error}");
            }

            var authResult = await response.Content.ReadFromJsonAsync<Response<AuthResponse>>();
            if (authResult == null || string.IsNullOrEmpty(authResult.Data.Token))
            {
                throw new HttpRequestException("Registration succeeded but token was not returned.");
            }

            return authResult.Data.Token;
        }
    }
}
