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

        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public CustomerApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Retrieves all patients.
        public async Task<ApiResponse<List<MCustomer>>> GetPatientsAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MCustomer>>>("api/Patient/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<MCustomer>> GetPatientByCodeAsync(String code)
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<MCustomer>>($"api/Patient/GetByCode/{code}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        // Calls the create endpoint (POST: api/Patient) to create a new patient.
        public async Task<ApiResponse<MCustomer>> CreatePatientAsync(MCustomer patient)
        {
            // POST the patient object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/Patient/add", patient);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<MCustomer>((int)responseMessage.StatusCode, $"Error creating patient: {errorMsg}");
            }

            // Deserialize the created patient.
            var createdPatientResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<MCustomer>>();
            return createdPatientResponse;
        }

        // Calls the update endpoint (PUT: api/Patient/{kode}) to update an existing patient.
        public async Task<ApiResponse<object>> UpdatePatientAsync(string kodeCustomer, MCustomer patient)
        {
            // The endpoint expects a PUT request with the patient identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/Patient/edit/{kodeCustomer}", patient);

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
