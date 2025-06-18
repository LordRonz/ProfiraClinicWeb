using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class DokterApiService
    {
        private readonly HttpClient _httpClient;

        public DokterApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<DokterListDto>>> GetDoktersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DokterListDto>>>("api/Dokter/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<Dokter>> GetDokterByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Dokter>>($"api/Dokter/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<Dokter>> CreateDokterAsync(Dokter dokter)
        {
            // POST the dokter object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/Dokter/add", dokter);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<Dokter>((int)responseMessage.StatusCode, $"Error creating Dokter: {errorMsg}");
            }

            // Deserialize the created dokter.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<Dokter>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateDokterAsync(string kodeGroup, Dokter dokter)
        {
            // The endpoint expects a PUT request with the dokter identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/Dokter/edit/{kodeGroup}", dokter);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating group perawatan: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "Dokter updated successfully");
        }
    }
}
