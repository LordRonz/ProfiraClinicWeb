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
    public interface ICustomerService
    {

        
        // Retrieves all clinics.
        public Task<ServiceResult<Customer>> GetByIdAsync(long id);

        public Task<ServiceResult<Customer>> GetByCodeAsync(string code);

    }
}
