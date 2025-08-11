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

    public interface IAnamnesisService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<TRMAnamnesis>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<NomorTransaksiDto>> Add(TRMAnamnesis diagnosa);

        public Task<ServiceResult<TRMAnamnesis>> GetByNomorAppointment(string nomorAppointment);

        public Task<ServiceResult<NomorTransaksiDto>> Edit(TRMAnamnesis item);
    }
}
