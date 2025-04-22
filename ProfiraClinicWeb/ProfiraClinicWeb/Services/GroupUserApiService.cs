using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupUserApiService
    {
        private readonly HttpClient _httpClient;

        public GroupUserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<UserGroup>>> GetUserGroupsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<UserGroup>>>("api/UserGroup");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<MCustomer>> GetUserGroupByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<MCustomer>>($"api/UserGroup/code/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<MCustomer>> CreateUserGroupAsync(MCustomer patient)
        {
            // POST the patient object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/UserGroup", patient);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<MCustomer>((int)responseMessage.StatusCode, $"Error creating patient: {errorMsg}");
            }

            // Deserialize the created patient.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<MCustomer>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateUserGroupAsync(string kodeCustomer, MCustomer patient)
        {
            // The endpoint expects a PUT request with the patient identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/UserGroup/{kodeCustomer}", patient);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating patient: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "UserGroup updated successfully");
        }
    }
}
