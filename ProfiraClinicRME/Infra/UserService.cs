using ProfiraClinic.Models.Core;
using ProfiraClinic.Models.Api;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;

namespace ProfiraClinicRME.Infra
{
    public class UserService:IUserService
    {
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


            return svcResult;
        }


    }
}
