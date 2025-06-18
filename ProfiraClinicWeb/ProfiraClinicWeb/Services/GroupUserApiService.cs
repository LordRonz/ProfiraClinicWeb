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
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<UserGroup>>>("api/UserGroup/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<UserGroup>> GetUserGroupByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<UserGroup>>($"api/UserGroup/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<UserGroup>> CreateUserGroupAsync(UserGroup userGroup)
        {
            // POST the userGroup object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/UserGroup/add", userGroup);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<UserGroup>((int)responseMessage.StatusCode, $"Error creating user group: {errorMsg}");
            }

            // Deserialize the created userGroup.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<UserGroup>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateUserGroupAsync(string kodeGroup, UserGroup userGroup)
        {
            // The endpoint expects a PUT request with the userGroup identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/UserGroup/edit/{kodeGroup}", userGroup);

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
