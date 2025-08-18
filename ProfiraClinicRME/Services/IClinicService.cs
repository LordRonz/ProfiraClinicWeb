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
    public interface IClinicService
    {

        
        // Retrieves all clinics.
        //public Task<ServiceResult<Pagination<Appointment>>> GetListDokter(string KodeLokasi, DateTime tglAppointment, string KodeKaryawan);

        public Task<ServiceResult<Pagination<MKlinik>>> GetListClinicsAsync(int pageNum, int pageSize);
    }
}
