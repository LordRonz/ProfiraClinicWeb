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
    public class PenandaanGambarService : IPenandaanGambarService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";

        private TRMAppointment? _current;

        private BaseRepo<PenandaanGambarFull> _repo;


        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public PenandaanGambarService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<PenandaanGambarFull>();
        }


        public async Task<ServiceResult<NomorTransaksiDto>> AddHeader(PenandaanGambarHeader newHeader)
        {
            LogTrace.Info($"init", newHeader, _classPath);

            //generate dto

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<PenandaanGambarHeader, NomorTransaksiDto>("post", "api/PenandaanGambar/AddPenandaanGambarHeader", newHeader);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<IdDetailDto>> AddDetail(PenandaanGambarDetail newDetail)
        {
            LogTrace.Info($"init", newDetail, _classPath);

            //generate dto

            Response<IdDetailDto?> apiResponse = await _svcApi.Send<PenandaanGambarDetail, IdDetailDto>("post", "api/PenandaanGambar/AddPenandaanGambarDetail", newDetail);

            ServiceResult<IdDetailDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> EditHeader(PenandaanGambarHeader updHeader)
        {
            LogTrace.Info($"init", updHeader, _classPath);

            //generate dto

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<PenandaanGambarHeader, NomorTransaksiDto>("post", "api/PenandaanGambar/EditPenandaanGambarHeader", updHeader);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<IdDetailDto>> EditDetail(PenandaanGambarDetail updDetail)
        {
            LogTrace.Info($"init", updDetail, _classPath);

            //generate dto

            Response<IdDetailDto?> apiResponse = await _svcApi.Send<PenandaanGambarDetail, IdDetailDto>("post", "api/PenandaanGambar/EditPenandaanGambarDetail", updDetail);

            ServiceResult<IdDetailDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<PenandaanGambarFull>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<PenandaanGambarFull?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, PenandaanGambarFull>("post", "api/PenandaanGambar/GetByNomorAppointment", dto);

            ServiceResult<PenandaanGambarFull> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);
            return svcResult;
        }


        public async Task<ServiceResult<Pagination<PenandaanGambarFull>>> GetListAsync(string kodeCustomer)
        {

            var request = new KodeCustomerDTO
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<PenandaanGambarFull>?> apiResponse = await _svcApi.Send<KodeCustomerDTO, Pagination<PenandaanGambarFull>>("post", $"api/PemeriksaanUmum/GetListTrm", request);

            ServiceResult<Pagination<PenandaanGambarFull>> svcResult = _repo.ProcessResult<Pagination<PenandaanGambarFull>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

    }
}
