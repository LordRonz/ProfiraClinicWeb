using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupUserApiService
    {
        private readonly HttpClient _httpClient;

        public GroupUserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<UserGroup>>> GetUserGroupsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/UserGroup/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<UserGroup>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<UserGroup>>> SearchUserGroupsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/UserGroup/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<UserGroup>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<UserGroup>>>();
            return result!;
        }

        public async Task<ApiResponse<UserGroup>> GetUserGroupByCodeAsync(string id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<UserGroup>>($"api/UserGroup/GetByCode/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<UserGroup>> CreateUserGroupAsync(UserGroup userGroup)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/UserGroup/add", userGroup);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<UserGroup>((int)responseMessage.StatusCode, $"Error creating UserGroup: {errorMsg}");
            }
            var created = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<UserGroup>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateUserGroupAsync(
            string kodeGroup,
            UserGroup userGroup)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/UserGroup/edit/{kodeGroup}", userGroup);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating UserGroup: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "UserGroup updated successfully");
        }
    }
}
