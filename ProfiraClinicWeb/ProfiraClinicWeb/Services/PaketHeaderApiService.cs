using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class PaketHeaderApiService
    {
        private readonly HttpClient _httpClient;

        public PaketHeaderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<PaketHeader>>> GetPaketHeadersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PaketHeader>>>("api/PaketHeader/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PaketHeader>> GetPaketHeaderByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PaketHeader>>($"api/PaketHeader/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PaketHeader>> CreatePaketHeaderAsync(PaketHeader paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/PaketHeader/add", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<PaketHeader>((int)responseMessage.StatusCode, $"Error creating PaketHeader: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<PaketHeader>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdatePaketHeaderAsync(string kodePaket, PaketHeader paketHeader)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/PaketHeader/edit/{kodePaket}", paketHeader);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating paketheader: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "PaketHeader updated successfully");
        }
    }
}
