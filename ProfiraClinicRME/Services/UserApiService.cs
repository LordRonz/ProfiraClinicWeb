using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Infra;
using ProfiraClinic.Models.Api;

namespace ProfiraClinicRME.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Response<CurrentUser>> GetCurrentUserAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<CurrentUser>>("api/User/me");

            if (response == null)
            {
                throw new HttpRequestException("Failed to retrieve response from API.");
            }

            return response;
        }



    }
}
