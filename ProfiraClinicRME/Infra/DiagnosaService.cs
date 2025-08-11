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

        public async Task<ServiceResult<NomorTransaksiDto>> Add(TRMDiagnosa newEntity)
        {
            LogTrace.Info($"init", newEntity, _classPath);

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<TRMDiagnosa, NomorTransaksiDto>("post", "api/Diagnosa/AddDiagnosa", newEntity);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult<NomorTransaksiDto>(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<TRMDiagnosa>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<TRMDiagnosa?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, TRMDiagnosa>("post", "api/Diagnosa/GetByNomorAppointment", dto);

            ServiceResult<TRMDiagnosa> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);
            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(TRMDiagnosa updatedEntity)
        {
            LogTrace.Info($"init", updatedEntity, _classPath);

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<TRMDiagnosa, NomorTransaksiDto>("post", "api/Diagnosa/EditDiagnosa", updatedEntity);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }
    }
}
