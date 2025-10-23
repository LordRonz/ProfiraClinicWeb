using ProfiraClinic.Models.Core;
using ProfiraClinicWeb.Helpers;

namespace ProfiraClinicWeb.Services
{
    public class RiwayatTransaksiApiService
    {
        private readonly HttpClient _httpClient;

        public RiwayatTransaksiApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<SaldoPaket>>> GetSaldoPaket(
            string kodeCustomer)
        {
            var url = $"api/RiwayatTransaksi/GetSaldoPaket/{kodeCustomer}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<List<SaldoPaket>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<List<SaldoPiutang>>> GetSaldoPiutang(
            string kodeCustomer)
        {
            var url = $"api/RiwayatTransaksi/GetSaldoPiutang/{kodeCustomer}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<List<SaldoPiutang>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }

        public async Task<ApiResponse<List<PenjualanPaket>>> GetPenjualanPaket(
            string kodeCustomer)
        {
            var url = $"api/RiwayatTransaksi/GetPenjualanPaket/{kodeCustomer}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<List<PenjualanPaket>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
        public async Task<ApiResponse<List<PenjualanPerawatan>>> GetPenjualanPerawatan(
            string kodeCustomer)
        {
            var url = $"api/RiwayatTransaksi/GetPenjualanPerawatan/{kodeCustomer}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<List<PenjualanPerawatan>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
        public async Task<ApiResponse<List<PenjualanProduk>>> GetSaldoPakets(
            string kodeCustomer)
        {
            var url = $"api/RiwayatTransaksi/GetPenjualanProduk/{kodeCustomer}";
            var response = await _httpClient
                .GetFromJsonAsync<ApiResponse<List<PenjualanProduk>>>(url)
                ?? throw new HttpRequestException("Failed to retrieve response from API.");
            return response;
        }
    }
}
