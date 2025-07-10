using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Infra
{
    public class ClinicService : IClinicService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::ClinicService";

        private BaseRepo<MKlinik> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public ClinicService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<MKlinik>();
        }

        // Retrieves all clinics.
        public async Task<ServiceResult<PagedList<MKlinik>>> GetListClinicsAsync(int pageNum, int pageSize)
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            Response<PagedList<MKlinik>?> apiResponse = await _svcApi.SendEmpty<PagedList<MKlinik>>("get", $"api/Klinik/GetList?page={pageNum}&pageSize={pageSize}");

            ServiceResult<PagedList<MKlinik>> svcResult = _repo.ProcessResult<PagedList<MKlinik>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

    }
}
