using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class CustomerRiwayatAsalService
    {
        private readonly HttpClient _httpClient;

        public CustomerRiwayatAsalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<CustomerRiwayatAsal>>> GetCustomerRiwayatAsalsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/CustomerRiwayatAsal/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<CustomerRiwayatAsal>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<CustomerRiwayatAsal>>> SearchCustomerRiwayatAsalsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/CustomerRiwayatAsal/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<CustomerRiwayatAsal>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<CustomerRiwayatAsal>>>();
            return result!;
        }

        public async Task<ApiResponse<CustomerRiwayatAsal>> GetCustomerRiwayatAsalByCodeAsync(string code)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<CustomerRiwayatAsal>>($"api/CustomerRiwayatAsal/GetByCode/{code}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<CustomerRiwayatAsal>> CreateCustomerRiwayatAsalAsync(CustomerRiwayatAsal customerRiwayatAsal)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/CustomerRiwayatAsal/add", customerRiwayatAsal);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<CustomerRiwayatAsal>((int)responseMessage.StatusCode, $"Error creating CustomerRiwayatAsal: {errorMsg}");
            }
            var created = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<CustomerRiwayatAsal>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateCustomerRiwayatAsalAsync(
            string kodeCustomer,
            CustomerRiwayatAsal customerRiwayatAsal)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/CustomerRiwayatAsal/edit/{kodeCustomer}", customerRiwayatAsal);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating CustomerRiwayatAsal: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "CustomerRiwayatAsal updated successfully");
        }
    }
}
