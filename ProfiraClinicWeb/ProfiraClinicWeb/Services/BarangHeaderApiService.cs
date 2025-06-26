using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProfiraClinicWeb.Services
{
    public class BarangHeaderApiService
    {
        private readonly HttpClient _httpClient;

        public BarangHeaderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<BarangHeaderList>>> GetBarangHeadersAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/BarangHeader/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<BarangHeaderList>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<BarangListDto>>> GetBarangItemsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/BarangHeader/GetListItem?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<BarangListDto>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<BarangHeader>> GetBarangHeaderByCodeAsync(string id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<BarangHeader>>($"api/BarangHeader/GetByCode/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<BarangHeader>> CreateBarangHeaderAsync(BarangHeader paket)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/BarangHeader/add", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<BarangHeader>((int)responseMessage.StatusCode, $"Error creating BarangHeader: {errorMsg}");
            }
            var created = await responseMessage
                .Content
                .ReadFromJsonAsync<ApiResponse<BarangHeader>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateBarangHeaderAsync(
            string kodeBarang,
            BarangHeader paketHeader)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/BarangHeader/edit/{kodeBarang}", paketHeader);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating BarangHeader: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "BarangHeader updated successfully");
        }
    }
}
