using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupPerawatanApiService
    {
        private readonly HttpClient _httpClient;

        public GroupPerawatanApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<GroupPerawatan>>> GetGroupPerawatansAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<GroupPerawatan>>>("api/GroupPerawatan/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupPerawatan>> GetGroupPerawatanByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<GroupPerawatan>>($"api/GroupPerawatan/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupPerawatan>> CreateGroupPerawatanAsync(GroupPerawatan paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupPerawatan/add", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupPerawatan>((int)responseMessage.StatusCode, $"Error creating GroupPerawatan: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<GroupPerawatan>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateGroupPerawatanAsync(string kodeGroup, UserGroup userGroup)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/GroupPerawatan/edit/{kodeGroup}", userGroup);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating group perawatan: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupPerawatan updated successfully");
        }
    }
}
