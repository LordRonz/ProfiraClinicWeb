using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class GroupPaketApiService
    {
        private readonly HttpClient _httpClient;

        public GroupPaketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<PagedResult<GroupPaket>>> GetGroupPaketsAsync(
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupPaket/GetList?Page={page}&PageSize={pageSize}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<GroupPaket>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<PagedResult<GroupPaket>>> SearchGroupPaketsAsync(
            string searchTerm,
            int page = 1,
            int pageSize = 20)
        {
            var url = $"api/GroupPaket/GetListByString?Page={page}&PageSize={pageSize}";
            var payload = new { GetParam = searchTerm ?? string.Empty };
            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);

            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<GroupPaket>>((int)respMsg.StatusCode, $"Error: {err}");
            }

            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<GroupPaket>>>();
            return result!;
        }

        /// <summary>
        /// Get GroupPaket by its KodeGroupPaket (new route: GetByCode/{code})
        /// </summary>
        public async Task<ApiResponse<GroupPaket>> GetGroupPaketByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code must be provided", nameof(code));

            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<GroupPaket>>($"api/GroupPaket/GetByCode/{code}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }


        public async Task<ApiResponse<GroupPaket>> CreateGroupPaketAsync(GroupPaket paket)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/GroupPaket/add", paket);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<GroupPaket>((int)responseMessage.StatusCode, $"Error creating GroupPaket: {errorMsg}");
            }
            var created = await responseMessage
                .Content
                .ReadFromJsonAsync<ApiResponse<GroupPaket>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdateGroupPaketAsync(
            string kodeGroup,
            GroupPaket paket)
        {
            var responseMessage = await _httpClient
                .PostAsJsonAsync($"api/GroupPaket/edit", paket);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating GroupPaket: {errorMsg}");
            }

            return new ApiResponse<object>((int)responseMessage.StatusCode, "GroupPaket updated successfully");
        }

        public async Task<ApiResponse<GroupPaket>> DeleteGroupPaketByIdAsync(string id)
        {
            var response = await _httpClient
                .DeleteFromJsonAsync<ApiResponse<GroupPaket>>($"api/GroupPaket/Del/{id}")
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
    }
}
