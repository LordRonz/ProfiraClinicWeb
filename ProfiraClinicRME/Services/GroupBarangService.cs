using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;

namespace ProfiraClinicRME.Services
{
    public class GroupBarangApiService
    {
        private readonly HttpClient _httpClient;

        public GroupBarangApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<GroupBarang>>> GetGroupBarangsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<GroupBarang>>>("api/GroupBarang/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupBarang>> GetGroupBarangByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<GroupBarang>>($"api/GroupBarang/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupBarang>> CreateGroupBarangAsync(GroupBarang paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupBarang/add", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupBarang>((int)responseMessage.StatusCode, $"Error creating GroupBarang: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<GroupBarang>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateGroupBarangAsync(string kodeGroup, UserGroup userGroup)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/GroupBarang/edit/{kodeGroup}", userGroup);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating group perawatan: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupBarang updated successfully");
        }
    }
}
