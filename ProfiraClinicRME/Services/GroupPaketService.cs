using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;

namespace ProfiraClinicRME.Services
{
    public class GroupPaketApiService
    {
        private readonly HttpClient _httpClient;

        public GroupPaketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<GroupPaket>>> GetGroupPaketsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<GroupPaket>>>("api/GroupPaket/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupPaket>> GetGroupPaketByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<GroupPaket>>($"api/GroupPaket/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<GroupPaket>> CreateGroupPaketAsync(GroupPaket paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupPaket/add", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupPaket>((int)responseMessage.StatusCode, $"Error creating GroupPaket: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<GroupPaket>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateGroupPaketAsync(string kodeGroup, UserGroup userGroup)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/GroupPaket/edit/{kodeGroup}", userGroup);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating group paket: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupPaket updated successfully");
        }
    }
}
