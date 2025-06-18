using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Components.Pages.Clinic;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class ClinicApiService
    {
        private readonly HttpClient _httpClient;

        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public ClinicApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Retrieves all clinics.
        public async Task<ApiResponse<List<MKlinik>>> GetClinicsAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MKlinik>>>("api/klinik/GetList");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        public async Task<ApiResponse<MKlinik>> GetClinicByCodeAsync(String code)
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<MKlinik>>($"api/klinik/GetByCode/{code}");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }

        // Calls the create endpoint (POST: api/Clinics) to create a new Clinics.
        public async Task<ApiResponse<MKlinik>> CreateClinicsAsync(MKlinik clinic)
        {
            // POST the Clinics object as JSON to the API.
            var responseMessage = await _httpClient.PostAsJsonAsync("api/klinik/add", clinic);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Retrieve the error message from the response.
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<MKlinik>((int)responseMessage.StatusCode, $"Error creating klinik: {errorMsg}");
            }

            // Deserialize the created patient.
            var createdPatientResponse = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<MKlinik>>();
            return createdPatientResponse;
        }

        // Calls the update endpoint (PUT: api/clinic/{kode}) to update an existing patient.
        public async Task<ApiResponse<object>> UpdatClinicAsync(string kodeCustomer, MKlinik clinic)
        {
            // The endpoint expects a PUT request with the Clinic identifier in the URL.
            var responseMessage = await _httpClient.PutAsJsonAsync($"api/klinik/edit/{kodeCustomer}", clinic);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                return new ApiResponse<object>((int)responseMessage.StatusCode, $"Error updating klinik: {errorMsg}");
            }

            // If no content is returned from the update call, we can return a successful ApiResponse.
            return new ApiResponse<object>((int)responseMessage.StatusCode, "Clinic updated successfully");
        }
    }
}
