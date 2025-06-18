using ProfiraClinic.Models;
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

        public async Task<ApiResponse<List<PPerawatanH>>> GetTindakansAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PPerawatanH>>>("api/Tindakan/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PPerawatanH>> GetTindakanByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PPerawatanH>>($"api/Tindakan/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PPerawatanH>> CreateTindakanAsync(PPerawatanH perawatan)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/Tindakan/add", perawatan);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<PPerawatanH>((int)responseMessage.StatusCode, $"Error creating Tindakan: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<PPerawatanH>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateTindakanAsync(string kodePaket, PPerawatanH perawatan)
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
    }
}
