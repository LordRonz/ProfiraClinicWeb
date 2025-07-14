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
    public interface IAppointmentService
    {

        
        // Retrieves all clinics.
        public Task<ServiceResult<PagedList<TRMAppointment>>> GetListOnWaitAsync(string KodeLokasi, DateTime tglAppointment, string KodeKaryawan);

        public ServiceResult<bool> SetCurrent(TRMAppointment appointment);
        public ServiceResult<TRMAppointment> GetCurrent();

    }
}
