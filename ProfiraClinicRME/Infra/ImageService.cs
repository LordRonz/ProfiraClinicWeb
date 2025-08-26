using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Infra
{
    public class ImageService : IImageService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::ImageService";

        private BaseRepo<PenandaanGambarFull> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public ImageService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<PenandaanGambarFull>();
        }

        //retrieve all template
        public async Task<ServiceResult<List<TemplateImageDto>>> GetListTemplateAsync()
        {


            Response<List<TemplateImageDto>?> apiResponse = await _svcApi.SendEmpty<List<TemplateImageDto>>("get", $"api/Images/MasterGambar/GetList");

            ServiceResult<List<TemplateImageDto>> svcResult = _repo.ProcessResult<List<TemplateImageDto>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public string GetBaseUrlForUserImage()
        {
            return "http://116.68.252.26:2032/api/Images/";
        }

    }
}
