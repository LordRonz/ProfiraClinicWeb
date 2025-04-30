using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<User>>> GetUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<User>>>("api/User");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> GetUserByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<User>>($"api/User/code/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<MCustomer>> CreateUserAsync(MCustomer patient)
        {
            // POST the patient object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/User", patient);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<MCustomer>((int)responseMessage.StatusCode, $"Error creating patient: {errorMsg}");
            }

            // Deserialize the created patient.
            var createdUserResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<MCustomer>>();
            return createdUserResponse;
        }

        public async Task<ApiResponse<object>> UpdateUserAsync(string kodeCustomer, MCustomer patient)
        {
            // The endpoint expects a PUT request with the patient identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/User/{kodeCustomer}", patient);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating patient: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "User updated successfully");
        }
    }
}
