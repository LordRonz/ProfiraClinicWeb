using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Infra;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Services
{

    public interface IPenandaanGambarService
    {

        /// <summary>
        /// Get list by kode pelanggan
        /// </summary>
        /// <param name="kodeCustomer"></param>
        /// <returns></returns>
        public Task<ServiceResult<Pagination<PenandaanGambarFull>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<NomorTransaksiDto>> AddHeader(TRMPenandaanGambarHeader newHeader);

        public Task<ServiceResult<IdDetailDto>> AddDetail(TRMPenandaanGambarDetail newDetail);

        public Task<ServiceResult<NomorTransaksiDto>> EditHeader(TRMPenandaanGambarHeader updHeader);

        public Task<ServiceResult<IdDetailDto>> EditDetail(TRMPenandaanGambarDetail updDetail);

        public Task<ServiceResult<PenandaanGambarFull>> GetByNomorAppointment(string nomorAppointment);

    }
}
