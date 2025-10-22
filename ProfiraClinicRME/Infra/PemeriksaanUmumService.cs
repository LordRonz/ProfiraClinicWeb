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
    public class PemeriksaanUmumService : IPemeriksaanUmumService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";

        private TRMAppointment? _current;

        private BaseRepo<TRMPemeriksaanUmum> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public PemeriksaanUmumService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMPemeriksaanUmum>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<TRMPemeriksaanUmum>>> GetListAsync(string kodeCustomer)
        {

            var request = new KodeCustomerDto
            {
                KodeCustomer = kodeCustomer
            };

            Response<Pagination<TRMPemeriksaanUmum>?> apiResponse = await _svcApi.Send<KodeCustomerDto, Pagination<TRMPemeriksaanUmum>>("post", $"api/PemeriksaanUmum/GetListTrm", request);

            ServiceResult<Pagination<TRMPemeriksaanUmum>> svcResult = _repo.ProcessResult<Pagination<TRMPemeriksaanUmum>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Add(TRMPemeriksaanUmum newEntity)
        {
            LogTrace.Info($"init", newEntity, _classPath);

            //generate dto
            var newEntityDto = new AddPemeriksaanUmumDto
            {
                KodeLokasi = newEntity.KodeLokasi,
                TanggalTransaksi = newEntity.TanggalTransaksi,
                NomorAppointment = newEntity.NomorAppointment,
                KodeCustomer = newEntity.KodeCustomer,
                KodeKaryawan = newEntity.KodeKaryawan,
                Keadaan_Umum = newEntity.KeadaanUmum,
                Tingkat_Kesadaran = newEntity.TingkatKesadaran,
                Sistolik = newEntity.Sistolik ?? 0,
                Distolik = newEntity.Distolik ?? 0,
                Suhu = newEntity.Suhu ?? 0,
                Saturasi = newEntity.Saturasi ?? 0,
                Frekuensi_Nadi = newEntity.FrekuensiNadi ?? 0,
                Frekuensi_Nafas = newEntity.FrekuensiNapas ?? 0,
                BeratBadan = newEntity.BeratBadan ?? 0,
                TinggiBadan = newEntity.TinggiBadan ?? 0,
                IndexTubuh = newEntity.IndexTubuh ?? 0,
                LingkarKepala = newEntity.LingkarKepala ?? 0
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<AddPemeriksaanUmumDto, NomorTransaksiDto>("post", "api/PemeriksaanUmum/AddPemeriksaanUmum", newEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

        public async Task<ServiceResult<TRMPemeriksaanUmum>> GetByNomorAppointment(string nomorAppointment)
        {
            LogTrace.Info($"init", nomorAppointment, _classPath);

            GetByNomorAppointmentDto dto = new()
            {
                NomorAppointment = nomorAppointment
            };

            Response<TRMPemeriksaanUmum?> apiResponse = await _svcApi.Send<GetByNomorAppointmentDto, TRMPemeriksaanUmum>("post", "api/PemeriksaanUmum/GetByNomorAppointment", dto);

            ServiceResult<TRMPemeriksaanUmum> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GET);
            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(TRMPemeriksaanUmum updatedEntity)
        {
            LogTrace.Info($"init", updatedEntity, _classPath);

            var updatedEntityDto = new EditPemeriksaanUmumDto
            {
                KodeLokasi = updatedEntity.KodeLokasi,
                TanggalTransaksi = updatedEntity.TanggalTransaksi,
                NomorAppointment = updatedEntity.NomorAppointment,
                KodeCustomer = updatedEntity.KodeCustomer,
                KodeKaryawan = updatedEntity.KodeKaryawan,
                Keadaan_Umum = updatedEntity.KeadaanUmum,
                Tingkat_Kesadaran = updatedEntity.TingkatKesadaran,
                Sistolik = updatedEntity.Sistolik ?? 0,
                Distolik = updatedEntity.Distolik ?? 0,
                Suhu = updatedEntity.Suhu ?? 0,
                Saturasi = updatedEntity.Saturasi ?? 0,
                Frekuensi_Nadi = updatedEntity.FrekuensiNadi ?? 0,
                Frekuensi_Nafas = updatedEntity.FrekuensiNapas ?? 0,
                BeratBadan = updatedEntity.BeratBadan ?? 0,
                TinggiBadan = updatedEntity.TinggiBadan ?? 0,
                IndexTubuh = updatedEntity.IndexTubuh ?? 0,
                LingkarKepala = updatedEntity.LingkarKepala ?? 0,
                NomorTransaksi = updatedEntity.NomorTransaksi ?? ""
            };

            Response<NomorTransaksiDto?> apiResponse = await _svcApi.Send<EditPemeriksaanUmumDto, NomorTransaksiDto>("post", "api/PemeriksaanUmum/EditPemeriksaanUmum", updatedEntityDto);

            ServiceResult<NomorTransaksiDto> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.ACTION);

            return svcResult;
        }

    }
}
