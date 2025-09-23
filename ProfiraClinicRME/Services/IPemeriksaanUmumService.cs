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

    public interface IPemeriksaanUmumService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<TRMPemeriksaanUmum>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<NomorTransaksiDto>> Add(TRMPemeriksaanUmum diagnosa);

        public Task<ServiceResult<TRMPemeriksaanUmum>> GetByNomorAppointment(string nomorAppointment);

        public Task<ServiceResult<NomorTransaksiDto>> Edit(TRMPemeriksaanUmum item);
    }
}
