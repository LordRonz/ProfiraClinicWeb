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
    public class MasterDiagnosaService: IMasterDiagnosaService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::MasterDiagnosaService";

        private TRMAppointment? _current; 

        private BaseRepo<MasterDiagnosa> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public MasterDiagnosaService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<MasterDiagnosa>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<PagedList<MasterDiagnosa>>> GetListAsync(int pageNum, int pageSize)
        {
            Response<PagedList<MasterDiagnosa>?> apiResponse = await _svcApi.SendEmpty<PagedList<MasterDiagnosa>>("get",$"api/MasterDiagnosa/GetList?page={pageNum}&pageSize={pageSize}");

            ServiceResult<PagedList<MasterDiagnosa>> svcResult = _repo.ProcessResult<PagedList<MasterDiagnosa>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

    }
}
