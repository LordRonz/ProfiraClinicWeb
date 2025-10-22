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
    public class RiwayatService : IRiwayatService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";

        private TRMAppointment? _current;

        private BaseRepo<TRMRiwayat> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public RiwayatService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMRiwayat>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<RiwayatAdaptor>>> GetListAsync(string kodeCustomer)
        {

            var request = new KodeCustomerDto
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<TRMRiwayat>?> apiResponse = await _svcApi.Send<KodeCustomerDto, Pagination<TRMRiwayat>>("post", $"api/Riwayat/GetListTrm", request);

            ServiceResult<Pagination<TRMRiwayat>> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GETLIST);

            return TransformPaged(svcResult);
        }

        private ServiceResult<Pagination<RiwayatAdaptor>>  TransformPaged(ServiceResult<Pagination<TRMRiwayat>> svc)
        {

            LogTrace.Info($"init", svc, _classPath);

            Pagination<RiwayatAdaptor> paginatedData = new Pagination<RiwayatAdaptor>();
            paginatedData.TotalCount = 0;
            paginatedData.Page = 0;
            paginatedData.PageSize = 0;
            paginatedData.Items = [];


            ServiceResult<Pagination<RiwayatAdaptor>> transformedSvc = new();
            transformedSvc.Data = paginatedData;
            transformedSvc.Status = svc.Status;
            transformedSvc.Message = svc.Message;
            if (svc.Status == ServiceResultEnum.FOUND)
            {
                List<RiwayatAdaptor> items = svc.Data.Items.Select(item => new RiwayatAdaptor(item)).ToList();
                paginatedData.Items = items;
                paginatedData.TotalCount = svc.Data.TotalCount;
                paginatedData.Page = svc.Data.Page;
                paginatedData.PageSize = svc.Data.PageSize;

            }

            return transformedSvc;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Add(RiwayatAdaptor newEntityAdaptor)
        {
            LogTrace.Info($"init", newEntityAdaptor, _classPath);

            TRMRiwayat newEntity = newEntityAdaptor.Entity;

            //generate dto
            var newEntityDto = new AddRiwayatDto
            {
                KodeLokasi = newEntity.KodeLokasi,
                TanggalTransaksi = newEntity.TanggalTransaksi,
                NomorAppointment = newEntity.NomorAppointment,
                KodeCustomer = newEntity.KodeCustomer,
                KodeKaryawan = newEntity.KodeKaryawan,
                PenyakitDahulu = newEntity.PenyakitDahulu,
                chkPenyakit = newEntity.chkPenyakit,
                PenyakitSekarang = newEntity.PenyakitSekarang,
                chkAlergiObat = newEntity.chkAlergiObat,
                KetAlergiObat = newEntity.KetAlergiObat,
                chkAlergiMakanan = newEntity.chkAlergiMakanan,
                KetAlergiMakanan = newEntity.KetAlergiMakanan,
                chkResiko = newEntity.chkResiko,
                KetResiko = newEntity.KetResiko
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<AddRiwayatDto, NomorTransaksiDto>("post", "api/Riwayat/AddRiwayat", newEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<RiwayatAdaptor>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<TRMRiwayat?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, TRMRiwayat>("post", "api/Riwayat/GetByNomorAppointment", dto);

            ServiceResult<TRMRiwayat> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);

            return  TransformSingle(svcResult);
        }

        private ServiceResult<RiwayatAdaptor> TransformSingle(ServiceResult<TRMRiwayat> svc)
        {

            LogTrace.Info($"init", svc, _classPath);

            ServiceResult<RiwayatAdaptor> transformedSvc = new();
            transformedSvc.Data = null;
            transformedSvc.Status = svc.Status;
            transformedSvc.Message = svc.Message;
            if (svc.Status == ServiceResultEnum.FOUND)
            {
                RiwayatAdaptor item = new(svc.Data);
                transformedSvc.Data= item;
            }

            return transformedSvc;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(RiwayatAdaptor updatedAdaptor)
        {

            LogTrace.Info($"init", updatedAdaptor, _classPath);

            TRMRiwayat updatedEntity = updatedAdaptor.Entity;

            var updatedEntityDto = new EditRiwayatDto
            {
                KodeLokasi = updatedEntity.KodeLokasi,
                TanggalTransaksi = updatedEntity.TanggalTransaksi,
                NomorAppointment = updatedEntity.NomorAppointment,
                KodeCustomer = updatedEntity.KodeCustomer,
                KodeKaryawan = updatedEntity.KodeKaryawan,
                PenyakitDahulu = updatedEntity.PenyakitDahulu,
                chkPenyakit = updatedEntity.chkPenyakit,
                PenyakitSekarang = updatedEntity.PenyakitSekarang,
                chkAlergiObat = updatedEntity.chkAlergiObat,
                KetAlergiObat = updatedEntity.KetAlergiObat,
                chkAlergiMakanan = updatedEntity.chkAlergiMakanan,
                KetAlergiMakanan = updatedEntity.KetAlergiMakanan,
                chkResiko = updatedEntity.chkResiko,
                KetResiko = updatedEntity.KetResiko,
                NomorTransaksi = updatedEntity.NomorTransaksi
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<EditRiwayatDto, NomorTransaksiDto>("post", "api/Riwayat/EditRiwayat", updatedEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

    }
}
