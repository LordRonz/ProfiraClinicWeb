using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class PerawatanDetailApiService
    {
        private readonly HttpClient _httpClient;

        public PerawatanDetailApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<PerawatanDetail>>> GetPerawatanDetailsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<PerawatanDetail>>>("api/PerawatanDetail/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<List<PerawatanDetail>>> GetPerawatanDetailsByHeaderAsync(string idPerawatanHeader)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PerawatanDetail>>>($"api/PerawatanDetail/GetByHeader/{idPerawatanHeader}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PerawatanDetail>> GetPerawatanDetailByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PerawatanDetail>>($"api/PerawatanDetail/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PerawatanDetail>> CreatePerawatanDetailAsync(PerawatanDetail perawatan)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/PerawatanDetail/add", perawatan);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<PerawatanDetail>((int)responseMessage.StatusCode, $"Error creating PerawatanDetail: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<PerawatanDetail>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdatePerawatanDetailAsync(string kodePaket, PerawatanDetail perawatan)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/PerawatanDetail/edit/{kodePaket}", perawatan);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating perawatan: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "PerawatanDetail updated successfully");
        }
    }
}
