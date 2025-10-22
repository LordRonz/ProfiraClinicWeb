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
    public class CPPTService : ICPPTService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";

        private TRMAppointment? _current;

        private BaseRepo<TRMCPPT> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public CPPTService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMCPPT>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<TRMCPPT>>> GetListAsync(string kodeCustomer)
        {

            var request = new KodeCustomerDto
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<TRMCPPT>?> apiResponse = await _svcApi.Send<KodeCustomerDto, Pagination<TRMCPPT>>("post", $"api/CPPT/GetListTrm", request);

            ServiceResult<Pagination<TRMCPPT>> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Add(TRMCPPT newEntity)
        {
            LogTrace.Info($"init", newEntity, _classPath);

            //generate dto
            var newEntityDto = new AddCPPTDto
            {
                KodeLokasi = newEntity.KodeLokasi,
                TanggalTransaksi = newEntity.TanggalTransaksi,
                NomorAppointment = newEntity.NomorAppointment,
                KodeCustomer = newEntity.KodeCustomer,
                KodeKaryawan = newEntity.KodeKaryawan,
                SUBYEKTIF = newEntity.Subyektif,
                OBYEKTIF = newEntity.Obyektif,
                ASSESTMENT = newEntity.Assestment,
                PLANNING = newEntity.Planning,
                INSTRUKSI = newEntity.Instruksi,
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<AddCPPTDto, NomorTransaksiDto>("post", "api/CPPT/AddCPPT", newEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<TRMCPPT>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<TRMCPPT?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, TRMCPPT>("post", "api/CPPT/GetByNomorAppointment", dto);

            ServiceResult<TRMCPPT> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);
            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(TRMCPPT updatedEntity)
        {
            LogTrace.Info($"init", updatedEntity, _classPath);

            var updatedEntityDto = new EditCPPTDto
            {
                KodeLokasi = updatedEntity.KodeLokasi,
                TanggalTransaksi = updatedEntity.TanggalTransaksi,
                NomorAppointment = updatedEntity.NomorAppointment,
                KodeCustomer = updatedEntity.KodeCustomer,
                KodeKaryawan = updatedEntity.KodeKaryawan,
                SUBYEKTIF = updatedEntity.Subyektif,
                OBYEKTIF = updatedEntity.Obyektif,
                ASSESTMENT = updatedEntity.Assestment,
                PLANNING = updatedEntity.Planning,
                INSTRUKSI = updatedEntity.Instruksi,
                NomorTransaksi = updatedEntity.NomorTransaksi
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<EditCPPTDto, NomorTransaksiDto>("post", "api/CPPT/EditCPPT", updatedEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

    }
}
