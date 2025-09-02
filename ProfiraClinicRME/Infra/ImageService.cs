using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProfiraClinicRME.Infra
{
    public class ImageService : IImageService
    {
        private readonly ApiService _svcApi;

        private IJSRuntime _js;

        private string _classPath = "Infra::ImageService";

        private string _baseUrl = "";

        private BaseRepo<PenandaanGambarFull> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public ImageService(ApiService svcApi, IJSRuntime js, IConfiguration configuration)
        {
            _js = js;
            _svcApi = svcApi;
            _repo = new BaseRepo<PenandaanGambarFull>();
            _baseUrl = configuration["ApiSettings:BaseAddress"] ?? "";


        }


        /// <summary>
        /// upload blob from global scope variable to server
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ServiceResult<FileNameDto>> UploadBlob(string blobName, string fileName)
        {
            LogTrace.Info("start", new { blobName, fileName}, _classPath);
            ServiceResult<FileNameDto> svcResult = new();
            svcResult.Status = ServiceResultEnum.FAIL;
            var uploadUrl = _baseUrl + "api/Images/Upload";

            OpStatus opStatus = new OpStatus();
            try
            {

            //invoke js function to upload blob
                opStatus = await _js.InvokeAsync<OpStatus>( "ImageService.uploadBlob", uploadUrl, "file", blobName, "file.jpg");
                //opStatus = await _js.InvokeAsync<OpStatus>("hello", "john");

            }
            catch (Microsoft.JSInterop.JSException ex)
            {
                // Catch a specific exception for JavaScript interop errors
                opStatus.Status = 1; // Set a failure status
                opStatus.Message = $"JavaScript error: {ex.Message}";
                // Log the exception for debugging
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions
                opStatus.Status = 1; // Set a failure status
                opStatus.Message = $"C# error: {ex.Message}";
                // Log the exception
            }


            if (opStatus.Status != 0)
            {
                svcResult.Message = opStatus.Message;
                LogTrace.Error(svcResult.Message, new { opStatus }, _classPath);
                return svcResult;
            }


            var jsonString = JsonSerializer.Serialize(opStatus.Data);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            var apiResponse = JsonSerializer.Deserialize<Response<FileNameDto>>(jsonString, options);
            if(apiResponse is null)
            {
                svcResult.Message = "Unknown Response data struct";
                LogTrace.Error(svcResult.Message, new { jsonString }, _classPath);
                return svcResult;
            }
            svcResult.Data = apiResponse.Data;

            //var jsonData = opStatus.Data;
            //LogTrace.Info("jsonData", jsonData, _classPath);
            // too strict conversion, too fragile
            //if (jsonData is Response<FileNameDto> apiResponse)
            //{
            //    LogTrace.Info("jsonData is apiresponse", apiResponse, _classPath);
            //    if(apiResponse.StatusCode != 0)
            //    {
            //        svcResult.Message = apiResponse.Message;
            //        LogTrace.Error(svcResult.Message, new { apiResponse }, _classPath);
            //        return svcResult;
            //    }
            //    else
            //    {
            //        svcResult.Data = apiResponse.Data;
            //    }
            //}
            if (svcResult.Data == null)
            {
                svcResult.Message = opStatus.Message;
                LogTrace.Error(svcResult.Message, new { opStatus }, _classPath);
                return svcResult;
            }


            svcResult.Status = ServiceResultEnum.SUCCESS;
            svcResult.Message = "Gambar berhasil diunggah.";
            return svcResult;

        }
        
        //retrieve  template url
        public async Task<ServiceResult<List<TemplateImageDto>>> GetListTemplateAsync()
        {
            Response<List<TemplateImageDto>?> apiResponse = await _svcApi.SendEmpty<List<TemplateImageDto>>("get", $"api/Images/MasterGambar/GetList");

            ServiceResult<List<TemplateImageDto>> svcResult = _repo.ProcessResult<List<TemplateImageDto>>(apiResponse, RepoProcessEnum.GET);

            return svcResult;
        }

        public string GetBaseUrlForUserImage()
        {
            return _baseUrl + "api/Images/";
        }

    }
}
