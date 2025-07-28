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

    public interface IDiagnosaService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<TRMDiagnosa>>> GetListAsync(string kodeCustomer);


        public Task<ServiceResult<AddDiagnosaResponseDTO>> AddDiagnosa(TRMDiagnosa item);
    }
}
