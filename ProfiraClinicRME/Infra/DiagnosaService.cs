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
    public class DiagnosaService : IDiagnosaService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";

        private TRMAppointment? _current;

        private BaseRepo<TRMDiagnosa> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public DiagnosaService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMDiagnosa>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<TRMDiagnosa>>> GetListAsync(string kodeCustomer)
        {

            var request = new ListDiagnosaRequestDTO
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<TRMDiagnosa>?> apiResponse = await _svcApi.Send<ListDiagnosaRequestDTO, Pagination<TRMDiagnosa>>("post", $"api/Diagnosa/GetListTrm", request);

            ServiceResult<Pagination<TRMDiagnosa>> svcResult = _repo.ProcessResult<Pagination<TRMDiagnosa>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public async Task<ServiceResult<AddDiagnosaResponseDTO>> AddDiagnosa(TRMDiagnosa diagnosa)
        {
            LogTrace.Info($"init", diagnosa, _classPath);

            Response<AddDiagnosaResponseDTO?> apiResponse = await _svcApi.Send<TRMDiagnosa, AddDiagnosaResponseDTO>("post", "api/Diagnosa/AddDiagnosa", diagnosa);

            ServiceResult<AddDiagnosaResponseDTO> svcResult = _repo.ProcessResult<AddDiagnosaResponseDTO>(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }
    }
}
