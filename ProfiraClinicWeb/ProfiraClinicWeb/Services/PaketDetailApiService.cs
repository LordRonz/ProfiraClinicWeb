using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class PaketDetailApiService
    {
        private readonly HttpClient _httpClient;

        public PaketDetailApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<PaketDetail>>> GetPaketDetailsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PaketDetail>>>("api/PaketDetail/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<List<PaketDetail>>> GetPaketDetailsByHeaderAsync(string idPaketHeader)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PaketDetail>>>($"api/PaketDetail/GetByHeader/{idPaketHeader}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PaketDetail>> GetPaketDetailByCodeAsync(String id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PaketDetail>>($"api/PaketDetail/GetByCode/{id}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<PaketDetail>> CreatePaketDetailAsync(PaketDetail paket)
        {
            // POST the paket object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/PaketDetail/add", paket);

            System.Diagnostics.Debug.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<PaketDetail>((int)responseMessage.StatusCode, $"Error creating PaketDetail: {errorMsg}");
            }

            // Deserialize the created paket.
            var createdUserGroupResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<PaketDetail>>();
            return createdUserGroupResponse;
        }

        public async Task<ApiResponse<object>> UpdatePaketDetailAsync(string kodePaket, PaketDetail paketDetail)
        {
            // The endpoint expects a PUT request with the paket identifier in the URL.
            var responseMessage = await _httpClient.PostAsJsonAsync($"api/PaketDetail/edit", paketDetail);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating paketheader: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "PaketDetail updated successfully");
        }
    }
}
