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

    public interface ICPPTService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<TRMCPPT>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<NomorTransaksiDto>> Add(TRMCPPT diagnosa);

        public Task<ServiceResult<TRMCPPT>> GetByNomorAppointment(string nomorAppointment);

        public Task<ServiceResult<NomorTransaksiDto>> Edit(TRMCPPT item);
    }
}
