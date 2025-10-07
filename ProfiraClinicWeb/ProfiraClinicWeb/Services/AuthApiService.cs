using ProfiraClinic.Models.Api;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
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

    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        // Inject the HttpClient (configured in Program.cs or Startup.cs)
        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Logs in a user and returns the JWT token.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>The JWT token string.</returns>
        public async Task<string> LoginAsync(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Login failed");
            }

            var authResult = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            if (authResult == null || string.IsNullOrEmpty(authResult?.Data?.Token))
            {
                throw new HttpRequestException("Authentication succeeded but token was not returned.");
            }

            if (authResult.StatusCode != 0)
            {
                throw new HttpRequestException("Login failed, check your credentials.");
            }

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

            var authResult = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            if (authResult == null || string.IsNullOrEmpty(authResult.Data.Token))
            {
                throw new HttpRequestException("Registration succeeded but token was not returned.");
            }

            return authResult.Data.Token;
        }

        public async Task<string> ChangePasswordAsync(ChangeOwnPasswordDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Change password failed: {response.StatusCode} - {error}");
            }

            var authResult = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            if (authResult == null || string.IsNullOrEmpty(authResult.Data.Token))
            {
                throw new HttpRequestException("Registration succeeded but token was not returned.");
            }

            return "Success";
        }
    }
}
