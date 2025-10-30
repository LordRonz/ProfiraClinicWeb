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
    public class PerawatanService : IPerawatanService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DiagnosaService";


        private BaseRepo<PerawatanHeader> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public PerawatanService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<PerawatanHeader>();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<PerawatanHeader>>> GetListAsync(int pageNum, int pageSize, string filter)
        {

            var request = new RequestParamDto
            {
                Param = filter
            };

            Response<Pagination<PerawatanHeader>?> apiResponse = await _svcApi.Send<RequestParamDto, Pagination<PerawatanHeader>>("post", $"api/PerawatanHeader/GetListByString?page={pageNum}&pageSize={pageSize}", request);

            ServiceResult<Pagination<PerawatanHeader>> svcResult = _repo.ProcessResult(apiResponse, RepoProcessEnum.GETLIST);

            return svcResult;
        }



    }
}
