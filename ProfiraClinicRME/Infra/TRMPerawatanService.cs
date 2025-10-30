using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using StatusDTO = ProfiraClinicRME.Model.EditStatusTindakanDTO;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Infra
{
    public class TRMPerawatanService : ITRMPerawatanService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::TRMPerawatanService";


        private BaseRepo<TRMPerawatanHeader> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public TRMPerawatanService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMPerawatanHeader>();
        }


        public async Task<ServiceResult<Pagination<TRMPerawatanHeader>>> GetListAsync(string KodeCustomer)
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            var request = new KodeCustomerDto
            {
                KodeCustomer = KodeCustomer
            };
            Response<Pagination<TRMPerawatanHeader>?> apiResponse = await _svcApi.Send<KodeCustomerDto, Pagination<TRMPerawatanHeader>>("post", "api/TRMPerawatanHeader/GetListTRM", request);

            ServiceResult<Pagination<TRMPerawatanHeader>> svcResult = _repo.ProcessResult<Pagination<TRMPerawatanHeader>>(apiResponse, RepoProcessEnum.GET);
            LogTrace.Info("fin", svcResult, _classPath);
            return svcResult;
        }

        public void CopyHeader(TRMPerawatanHeader source, TRMPerawatanHeader target)
        {
            target.NomorTransaksi = source.NomorTransaksi;
            target.TRCD = source.TRCD;
            target.TRSC = source.TRSC;
            target.KodeLokasi = source.KodeLokasi;
            target.TahunTransaksi = source.TahunTransaksi;
            target.BulanTransaksi = source.BulanTransaksi;
            target.TanggalTransaksi = source.TanggalTransaksi;
            target.NomorAppointment = source.NomorAppointment;
            target.KodeCustomer = source.KodeCustomer;
            target.KodePoli = source.KodePoli;
            target.Keterangan = source.Keterangan;
            target.StatusKonfirmasi = source.StatusKonfirmasi;
            target.UPDDT = source.UPDDT;
        }

        public void CopyDetail(TRMPerawatanDetail source, TRMPerawatanDetail target)
        {
            target.IdDetail = source.IdDetail;
            target.NomorTransaksi = source.NomorTransaksi;
            target.JenisPerawatan = source.JenisPerawatan;
            target.KodePaket = source.KodePaket;
            target.NomorFakturPaket = source.NomorFakturPaket;
            target.KodePerawatan = source.KodePerawatan;
            target.KodePerawatanPengganti = source.KodePerawatanPengganti;
            target.NamaPerawatan = source.NamaPerawatan;
            target.Qty = source.Qty;
            target.KeteranganDetail = source.KeteranganDetail;
            target.KodeDokter = source.KodeDokter;
            target.NamaDokter = source.NamaDokter;
            target.KodePerawat1 = source.KodePerawat1;
            target.KodePerawat2 = source.KodePerawat2;
        }

    }
}
