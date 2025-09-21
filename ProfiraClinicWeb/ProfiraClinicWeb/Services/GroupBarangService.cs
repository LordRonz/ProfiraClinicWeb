using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupBarangApiService
    {
        private readonly HttpClient _httpClient;

        public GroupBarangApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<GroupBarang>>> GetGroupBarangsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupBarang/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<GroupBarang>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<GroupBarang>>> SearchGroupBarangsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupBarang/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<GroupBarang>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<GroupBarang>>>();
            return result!;
        }

        public async Task<ApiResponse<GroupBarang>> GetGroupBarangByCodeAsync(string id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<GroupBarang>>($"api/GroupBarang/GetByCode/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<GroupBarang>> CreateGroupBarangAsync(GroupBarang paket)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupBarang/add", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupBarang>((int)responseMessage.StatusCode, $"Error creating GroupBarang: {errorMsg}");
            }
            var created = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<GroupBarang>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateGroupBarangAsync(
            string kodeGroup,
            GroupBarang paket)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/GroupBarang/edit/{kodeGroup}", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating GroupBarang: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupBarang updated successfully");
        }

        public async Task<ApiResponse<GroupBarang>> DeleteGroupBarangByIdAsync(string id)
        {
            var response = await _httpClient
                .DeleteFromJsonAsync<ApiResponse<GroupBarang>>($"api/GroupBarang/Del/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
    }
}
