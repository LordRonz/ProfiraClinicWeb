using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinicWeb.Helpers;
using ProfiraClinic.Models.Core;

namespace ProfiraClinicWeb.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _httpClient;

        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public CustomerApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Modify the URL path as per your API endpoint routing.
        public async Task<ApiResponse<List<MCustomer>>> GetPatientsAsync()
        {
            // Replace the URL with your actual endpoint
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MCustomer>>>("api/Patient");

            if (response == null)
            {
                // Handle the null scenario appropriately; you might throw an exception or return a default value.
                throw new HttpRequestException("Failed to retrieve response from API.");
            }
            return response;
        }
    }
}
