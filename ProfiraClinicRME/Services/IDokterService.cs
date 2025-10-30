using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;

namespace ProfiraClinicRME.Services
{

    public interface IDokterService
    {

        // Retrieves all clinics.
        public Task<ServiceResult<Pagination<DokterListDto>>> GetListDokterAsync();

        public Task<ServiceResult<Pagination<DokterListDto>>> GetListNonDokterAsync();

    }
}
