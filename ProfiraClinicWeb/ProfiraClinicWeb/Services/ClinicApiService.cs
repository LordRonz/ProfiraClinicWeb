using ProfiraClinic.Models;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProfiraClinicWeb.Services
{
    public class ClinicApiService
    {
        private readonly HttpClient _httpClient;

        public ClinicApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<MKlinik>>> GetClinicsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/klinik/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<MKlinik>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<MKlinik>>> SearchClinicsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/klinik/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<MKlinik>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<MKlinik>>>();
            return result!;
        }

        public async Task<ApiResponse<MKlinik>> GetClinicByCodeAsync(string code)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<MKlinik>>($"api/klinik/GetByCode/{code}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<MKlinik>> CreateClinicAsync(MKlinik clinic)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/klinik/add", clinic);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<MKlinik>((int)responseMessage.StatusCode, $"Error creating klinik: {errorMsg}");
            }
            var created = await responseMessage
                .Content
                .ReadFromJsonAsync<ApiResponse<MKlinik>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateClinicAsync(
            string kodeCustomer,
            MKlinik clinic)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/klinik/edit/{kodeCustomer}", clinic);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating klinik: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "Clinic updated successfully");
        }
    }
}
