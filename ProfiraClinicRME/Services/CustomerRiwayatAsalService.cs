using Azure;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;

namespace ProfiraClinicRME.Services
{
    public class CustomerRiwayatAsalService
    {
        private readonly HttpClient _httpClient;

        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public CustomerRiwayatAsalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Retrieves all patients.
        public async Task<ApiResponse<List<CustomerRiwayatAsal>>> GetCustomerRiwayatAsalsAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/CustomerRiwayatAsal"
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CustomerRiwayatAsal>>>("api/CustomerRiwayatAsal/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<CustomerRiwayatAsal>> GetCustomerRiwayatAsalByCodeAsync(String code)
        {
            ApiResponse<CustomerRiwayatAsal> response;
            try { 
            // Replace the URL with your actual endpoint, e.g., "api/CustomerRiwayatAsal"
            response = await _httpClient.GetFromJsonAsync<ApiResponse<CustomerRiwayatAsal>>($"api/CustomerRiwayatAsal/GetByCode/{code}");
            } catch
            {
                return null;
            }

            return response;
        }

        // Calls the create endpoint (POST: api/CustomerRiwayatAsal) to create a new CustomerRiwayatAsal.
        public async Task<ApiResponse<CustomerRiwayatAsal>> CreateCustomerRiwayatAsalAsync(CustomerRiwayatAsal customerRiwayatAsal)
        {
            // POST the patient object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/CustomerRiwayatAsal/add", customerRiwayatAsal);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<CustomerRiwayatAsal>((int)responseMessage.StatusCode, $"Error creating CustomerRiwayatAsal: {errorMsg}");
            }

            // Deserialize the created patient.
            var createdCustomerRiwayatAsalResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<CustomerRiwayatAsal>>();
            return createdCustomerRiwayatAsalResponse;
        }

        // Calls the update endpoint (PUT: api/CustomerRiwayatAsal/{kode}) to update an existing CustomerRiwayatAsal.
        public async Task<ApiResponse<object>> UpdateCustomerRiwayatAsalAsync(string kodeCustomer, CustomerRiwayatAsal customerRiwayatAsal)
        {
            // The endpoint expects a PUT request with the CustomerRiwayatAsal identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/CustomerRiwayatAsal/edit/{kodeCustomer}", customerRiwayatAsal);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating patient: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "Patient updated successfully");
        }
    }
}
