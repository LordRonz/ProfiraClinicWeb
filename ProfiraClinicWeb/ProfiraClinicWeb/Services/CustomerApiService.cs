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

        public async Task<ApiResponse<PagedResult<Customer>>> GetPatientsAsync(
    int page = 1, int pageSize = 20, params string[]? sort)
        {
            var qs = new List<string>
    {
        $"Page={page}",
        $"PageSize={pageSize}"
    };

            if (sort is not null)
            {
                foreach (var s in sort.Where(x => !string.IsNullOrWhiteSpace(x)))
                    qs.Add($"sort={Uri.EscapeDataString(s)}");   // e.g. "NamaCustomer:desc" or "-NamaCustomer"
            }

            var url = $"api/Patient/GetList?{string.Join("&", qs)}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<PagedResult<Customer>>>(url)
                ?? throw new HttpRequestException("No response payload");
            return response;
        }

        public async Task<ApiResponse<PagedResult<Customer>>> SearchPatientsAsync(
            string searchTerm, int page = 1, int pageSize = 20, params string[]? sort)
        {
            var qs = new List<string>
    {
        $"Page={page}",
        $"PageSize={pageSize}"
    };

            if (sort is not null)
            {
                foreach (var s in sort.Where(x => !string.IsNullOrWhiteSpace(x)))
                    qs.Add($"sort={Uri.EscapeDataString(s)}");
            }

            var url = $"api/Patient/GetListByString?{string.Join("&", qs)}";

            // BaseCrudController.Search expects body.GetParam
            var payload = new { Param = searchTerm ?? string.Empty };

            var respMsg = await _httpClient.PostAsJsonAsync(url, payload);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<PagedResult<Customer>>((int)respMsg.StatusCode, $"Error: {err}");
            }

            var result = await respMsg.Content.ReadFromJsonAsync<ApiResponse<PagedResult<Customer>>>();
            return result!;
        }



        public async Task<ApiResponse<Customer>> GetPatientByCodeAsync(string code)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Customer>>($"api/Patient/GetByCode/{code}")
                           ?? throw new HttpRequestException("No response payload");
            return response;
        }

        public async Task<ApiResponse<Customer>> CreatePatientAsync(Customer patient)
        {
            var respMsg = await _httpClient.PostAsJsonAsync("api/Patient/add", patient);
            if (!respMsg.IsSuccessStatusCode)
            {
                var err = await respMsg.Content.ReadAsStringAsync();
                return new ApiResponse<Customer>((int)respMsg.StatusCode, $"Error: {err}");
            }
            var created = await respMsg.Content.ReadFromJsonAsync<ApiResponse<Customer>>();
            return created!;
        }

        public async Task<ApiResponse<object>> UpdatePatientAsync(string kodeCustomer, Customer patient)
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
