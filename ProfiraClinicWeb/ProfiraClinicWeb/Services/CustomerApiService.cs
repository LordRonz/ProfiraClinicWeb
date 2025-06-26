using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _httpClient;

        public CustomerApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<MCustomer>>> GetPatientsAsync(int page = 1, int pageSize = 20)
        {
            var url = $"api/Patient/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<MCustomer>>>(url)
                           ?? throw new HttpRequestException("No response payload");
            return response;
        }

        public async Task<ApiResponse<PagedResult<MCustomer>>> SearchPatientsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/Patient/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { Param = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);

            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<MCustomer>>((int)respMsg.StatusCode, $"Error: {err}");
            }

            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<MCustomer>>>();
            return result!;
        }

        public async Task<ApiResponse<MCustomer>> GetPatientByCodeAsync(string code)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<MCustomer>>($"api/Patient/GetByCode/{code}")
                           ?? throw new HttpRequestException("No response payload");
            return response;
        }

        public async Task<ApiResponse<MCustomer>> CreatePatientAsync(MCustomer patient)
        {
            var respMsg = await _httpClient.PostAsJsonAsync("api/Patient/add", patient);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<MCustomer>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var created = await respMsg.Content.ReadFromJsonAsync<ApiResponse<MCustomer>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdatePatientAsync(string kodeCustomer, MCustomer patient)
        {
            var respMsg = await _httpClient.PutAsJsonAsync($"api/Patient/edit/{kodeCustomer}", patient);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)respMsg.StatusCode, $"Error: {err}");
            }
            return new ApiResponse<object>((int)respMsg.StatusCode, "Patient updated successfully");
        }
    }
}
