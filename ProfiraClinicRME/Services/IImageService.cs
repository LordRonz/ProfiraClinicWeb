using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;

namespace ProfiraClinicRME.Services
{

    public interface IImageService
    {

        public Task<ServiceResult<FileNameDto>> UploadBlob(string blobName, string fileName = "");

        public  Task<ServiceResult<List<TemplateImageDto>>> GetListTemplateAsync();

        /// <summary>
        /// get base url with slash ending
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrlForUserImage();

    }
}
