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

    public interface IRiwayatService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<RiwayatAdaptor>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<NomorTransaksiDto>> Add(RiwayatAdaptor diagnosa);

        public Task<ServiceResult<RiwayatAdaptor>> GetByNomorAppointment(string nomorAppointment);

        public Task<ServiceResult<NomorTransaksiDto>> Edit(RiwayatAdaptor item);
    }
}
