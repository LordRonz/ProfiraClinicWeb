using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;

namespace ProfiraClinicRME.Services
{

    public interface IImageService
    {

        public Task<ServiceResult<FileNameDto>> UploadBlob(string blobName, string fileName = "");

        public  Task<ServiceResult<List<TemplateImageDto>>> GetListTemplateAsync();

        public string GetBaseUrlForUserImage();

    }
}
