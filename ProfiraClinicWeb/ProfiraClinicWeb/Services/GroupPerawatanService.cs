using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupPerawatanApiService
    {
        private readonly HttpClient _httpClient;

        public GroupPerawatanApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<GroupPerawatan>>> GetGroupPerawatansAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupPerawatan/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<GroupPerawatan>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<GroupPerawatan>>> SearchGroupPerawatansAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupPerawatan/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<GroupPerawatan>>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<GroupPerawatan>>>();
            return result!;
        }

        public async Task<ApiResponse<GroupPerawatan>> GetGroupPerawatanByCodeAsync(string id)
        {
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<GroupPerawatan>>($"api/GroupPerawatan/GetByCode/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<GroupPerawatan>> CreateGroupPerawatanAsync(GroupPerawatan paket)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupPerawatan/add", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupPerawatan>((int)responseMessage.StatusCode, $"Error creating GroupPerawatan: {errorMsg}");
            }
            var created = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<GroupPerawatan>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateGroupPerawatanAsync(
            string kodeGroup,
            GroupPerawatan paket)
        {
            var responseMessage = await _httpClient
                .PostAsJsonAsync($"api/GroupPerawatan/edit", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating GroupPerawatan: {errorMsg}");
            }
            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupPerawatan updated successfully");
        }

        public async Task<ApiResponse<GroupPerawatan>> DeleteGroupPerawatanByIdAsync(string id)
        {
            var response = await _httpClient
                .DeleteFromJsonAsync<ApiResponse<GroupPerawatan>>($"api/GroupPerawatan/Del/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
    }
}
