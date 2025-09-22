using ProfiraClinic.Models.Core;
using ProfiraClinic.Models.Api;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System.Reflection.Metadata.Ecma335;

namespace ProfiraClinicRME.Infra
{
    public class UserService:IUserService
    {
        public CurrentUser? Current { get => _current; }

        private CurrentUser? _current;

        private readonly ApiService _svcApi;

        private string _classPath = "AppointmentRepo";

        private BaseRepo<CurrentUser> _repo;



        public UserService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<CurrentUser>();

        }

        public async Task<ServiceResult<CurrentUser>> GetCurrentUserAsync()
        {
            Response<CurrentUser?> apiResponse = await _svcApi.SendEmpty<CurrentUser>("get", "api/User/me");

            ServiceResult<CurrentUser> svcResult = _repo.ProcessResult<CurrentUser>(apiResponse, RepoProcessEnum.GET);

            var thisResult = new ServiceResult<CurrentUser>()
            {
                Message = svcResult.Message,

            };

            if (svcResult.Status == ServiceResultEnum.FOUND) {
                thisResult.Data = svcResult.Data;

                thisResult.Status = ServiceResultEnum.SUCCESS;
                
            } else {

                thisResult.Status = ServiceResultEnum.FAIL;
                thisResult.Data = null;
            }

            LogTrace.Info("fin", thisResult, _classPath);
            return thisResult;
        }

    }
}
