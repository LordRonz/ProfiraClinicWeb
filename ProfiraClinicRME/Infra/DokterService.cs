using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;

namespace ProfiraClinicRME.Infra
{
    public class DokterService : IDokterService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DokterService";

        private BaseRepo<Dokter> _repo;

        public DokterService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<Dokter>();
        }

        public async Task<ServiceResult<Pagination<DokterListDto>>> GetListAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            Response<Pagination<DokterListDto>?> apiResponse = await _svcApi.SendEmpty<Pagination<DokterListDto>>("get", $"api/Dokter/GetList");

            ServiceResult<Pagination<DokterListDto>> svcResult = _repo.ProcessResult<Pagination<DokterListDto>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

    }
}
