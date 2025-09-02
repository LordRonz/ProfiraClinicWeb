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
    public class AnamnesisService : IAnamnesisService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::AnamnesisService";

        private TRMAppointment? _current;

        private BaseRepo<TRMAnamnesis> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public AnamnesisService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMAnamnesis>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<TRMAnamnesis>>> GetListAsync(string kodeCustomer)
        {

            var request = new KodeCustomerDTO
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<TRMAnamnesis>?> apiResponse = await _svcApi.Send<KodeCustomerDTO, Pagination<TRMAnamnesis>>("post", $"api/Anamnesis/GetListTrm", request);

            ServiceResult<Pagination<TRMAnamnesis>> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Add(TRMAnamnesis newEntity)
        {
            LogTrace.Info($"init", newEntity, _classPath);

            //generate dto
            var newEntityDto = new AddAnamnesisDto
            {
                KodeLokasi = newEntity.KodeLokasi,
                TanggalTransaksi = newEntity.TanggalTransaksi,
                NomorAppointment = newEntity.NomorAppointment,
                KodeCustomer = newEntity.KodeCustomer,
                KodeKaryawan = newEntity.KodeKaryawan,
                KeteranganAnamnesis = newEntity.KeteranganAnamnesis
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<AddAnamnesisDto, NomorTransaksiDto>("post", "api/Anamnesis/AddAnamnesis", newEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<TRMAnamnesis>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<TRMAnamnesis?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, TRMAnamnesis>("post", "api/Anamnesis/GetByNomorAppointment", dto);

            ServiceResult<TRMAnamnesis> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);
            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(TRMAnamnesis updatedEntity)
        {
            LogTrace.Info($"init", updatedEntity, _classPath);

            var updatedEntityDto = new EditAnamnesisDto
            {
                KodeLokasi = updatedEntity.KodeLokasi,
                TanggalTransaksi = updatedEntity.TanggalTransaksi,
                NomorAppointment = updatedEntity.NomorAppointment,
                KodeCustomer = updatedEntity.KodeCustomer,
                KodeKaryawan = updatedEntity.KodeKaryawan,
                KeteranganAnamnesis = updatedEntity.KeteranganAnamnesis,
                NomorTransaksi = updatedEntity.NomorTransaksi
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<EditAnamnesisDto, NomorTransaksiDto>("post", "api/Anamnesis/EditAnamnesis", updatedEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

    }
}
