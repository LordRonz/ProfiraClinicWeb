using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class PerawatanHeaderApiService
    {
        private readonly HttpClient _httpClient;

        public PerawatanHeaderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<PerawatanHeader>>> GetTindakansAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<PerawatanHeader>>>("api/Tindakan/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PerawatanHeader>> GetTindakanByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PerawatanHeader>>($"api/Tindakan/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PerawatanHeader>> CreateTindakanAsync(PerawatanHeader perawatan)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/Tindakan/add", perawatan);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<PerawatanHeader>((int)responseMessage.StatusCode, $"Error creating Tindakan: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<PerawatanHeader>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateTindakanAsync(string kodePaket, PerawatanHeader perawatan)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/Tindakan/edit/{kodePaket}", perawatan);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating perawatan: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "Tindakan updated successfully");
        }

        public async Task<ApiResponse<PerawatanHeader>> DeletePerawatanHeaderByIdAsync(string id)
        {
            var response = await _httpClient
                .DeleteFromJsonAsync<ApiResponse<PerawatanHeader>>($"api/Tindakan/Del/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
    }
}
