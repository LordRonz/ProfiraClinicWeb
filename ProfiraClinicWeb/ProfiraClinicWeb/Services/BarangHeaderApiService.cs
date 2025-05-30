using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class BarangHeaderApiService
    {
        private readonly HttpClient _httpClient;

        public BarangHeaderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<BarangHeaderList>>> GetBarangHeadersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BarangHeaderList>>>("api/BarangHeader/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<BarangHeader>> GetBarangHeaderByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<BarangHeader>>($"api/BarangHeader/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<BarangHeader>> CreateBarangHeaderAsync(BarangHeader paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/BarangHeader/add", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<BarangHeader>((int)responseMessage.StatusCode, $"Error creating BarangHeader: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<BarangHeader>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdateBarangHeaderAsync(string kodeBarang, BarangHeader paketHeader)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/BarangHeader/edit/{kodeBarang}", paketHeader);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating paketheader: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "BarangHeader updated successfully");
        }
    }
}
