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
        public async Task<ServiceResult<Pagination<MasterDiagnosa>>> GetListAsync(int pageNum, int pageSize)
        {
            Response<Pagination<MasterDiagnosa>?> apiResponse = await _svcApi.SendEmpty<Pagination<MasterDiagnosa>>("get",$"api/MasterDiagnosa/GetList?page={pageNum}&pageSize={pageSize}");

            ServiceResult<Pagination<MasterDiagnosa>> svcResult = _repo.ProcessResult<Pagination<MasterDiagnosa>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<MDiagnosaExt>>> GetListByStringAsync(int pageNum, int pageSize, string searchText)
        {
            var request= new RequestParamDto
            {
                Param = searchText
            };
            LogTrace.Debug("param", request, _classPath);

            Response<Pagination<MDiagnosaExt>?> apiResponse = await _svcApi.Send<RequestParamDto, Pagination<MDiagnosaExt>>("post", $"api/MasterDiagnosa/GetListByString?page={pageNum}&pageSize={pageSize}", request);

            ServiceResult<Pagination<MDiagnosaExt>> svcResult = _repo.ProcessResult<Pagination<MDiagnosaExt>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

    }
}
