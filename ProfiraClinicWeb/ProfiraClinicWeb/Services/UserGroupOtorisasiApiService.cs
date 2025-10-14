using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class UserGroupOtorisasiApiService
    {
        private readonly HttpClient _httpClient;

        public UserGroupOtorisasiApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<UserGroupOtorisasi>>> GetUserGroupsOtorisasiAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/UserGroupOtorisasi/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<UserGroupOtorisasi>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<UserGroupOtorisasi>>> SearchUserGroupsOtorisasiAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/UserGroupOtorisasi/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<UserGroupOtorisasi>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<UserGroupOtorisasi>>>();
            return result!;
        }

        public async Task<ApiResponse<UserGroupOtorisasi>> GetUserGroupOtorisasiByCodeAsync(string id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<UserGroupOtorisasi>>($"api/UserGroupOtorisasi/GetByCode/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<UserGroupOtorisasi>> CreateUserGroupAsync(UserGroupOtorisasi userGroup)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/UserGroupOtorisasi/add", userGroup);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<UserGroupOtorisasi>((int)responseMessage.StatusCode, $"Error creating UserGroup: {errorMsg}");
            }
            var created = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<UserGroupOtorisasi>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateUserGroupAsync(
            string kodeGroup,
            UserGroupOtorisasi userGroupOtorisasi)
        {
            var responseMessage = await _httpClient
                .PutAsJsonAsync($"api/UserGroupOtorisasi/edit/{kodeGroup}", userGroupOtorisasi);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating UserGroupOtorisasi: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "UserGroupOtorisasi updated successfully");
        }
    }
}
