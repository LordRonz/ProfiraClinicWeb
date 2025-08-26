using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;

namespace ProfiraClinicRME.Services
{

    public interface IImageService
    {

        // Retrieves all clinics.


        public  Task<ServiceResult<List<TemplateImageDto>>> GetListTemplateAsync();

        public string GetBaseUrlForUserImage();

    }
}
