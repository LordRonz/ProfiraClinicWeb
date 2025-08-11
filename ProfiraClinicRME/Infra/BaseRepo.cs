using Microsoft.AspNetCore.Mvc;
using Serilog;

using Microsoft.AspNetCore.Mvc;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Utils;
using ProfiraClinicRME.Services;
using ProfiraClinic.Models.Core;
using ProfiraClinic.Models.Api;

namespace ProfiraClinicRME.Infra
{
    public enum RepoProcessEnum
    {
        GET,
        GETLIST,
        ACTION
    }

    public class BaseRepo<ModelType> where ModelType : class, new()
    {

        public string StdFailMessage = "Terjadi kesalahan sistem.";

        protected List<ModelType> listItem = new();

        protected int lastFilterCount;

        protected ApiService apiService;

        protected IServiceProvider serviceProvider;

        protected AuthService svcAuth;

        protected string classPath = "App::AbstractRepo<" + typeof(ModelType).Name + ">";

        public BaseRepo(ApiService apiService)
        {
            this.apiService = apiService;
            LogTrace.Debug("trace", path: classPath);
            //var x = this.GetType().Name;
        }

        public BaseRepo(ApiService apiService, AuthService svcAuth)
        {
            this.apiService = apiService;
            this.svcAuth = svcAuth;
            LogTrace.Debug("trace", path: classPath);
            //var x = this.GetType().Name;
        }

        public BaseRepo() { }





        //e.g. : Bagian
        protected string EntityName;

        /// <example>
        /// e.g. : /api/strukturorganisasi/Bagian
        /// </example>
        protected string UrlApiPath;

        public virtual async Task<ServiceResult<ModelType>> GetById(long id)
        {
            //string logSource = "App::AbstractRepo::GetById";
            LogTrace.Info($"init: {id}", classPath);

            Response<ModelType> apiResponse = await this.apiService.SendEmpty<ModelType>("post", UrlApiPath + "/GetById/" + id);

            var repoResult = ProcessResult<ModelType>(apiResponse, RepoProcessEnum.GET, $"Data {EntityName} berhasil didapatkan.");


            LogTrace.Info("fin", repoResult, classPath);
            return repoResult;
        }

        public ServiceResult<string> ProcessEmptyResult(Response<string?> apiResponse, RepoProcessEnum mode, string msgSuccess = "")
        {
            var repoResult = ProcessResult<string>(apiResponse, mode, msgSuccess, true);
            repoResult.Data = "";
            return repoResult;
        }


        // 
        /// <summary>
        /// used to process getById, getList
        /// </summary>
        /// <typeparam name="RespType"></typeparam>
        /// <param name="apiResponse"></param>
        /// <param name="mode"></param>
        /// <param name="msgSuccess"></param>
        /// <returns></returns>
        public ServiceResult<RespType> ProcessResult<RespType>(Response<RespType?> apiResponse, RepoProcessEnum mode, string msgSuccess = "", bool emptyResult = false)
        {
            //check based on statusCode
            //process response
            /* SUCCESS - 200/0 
             * - FOUND / NOTFOUND / SUCCESS
             * SUCCESS - 400 , inv domain value, not found
             * - FAIL: inv domain value: 
             * - NOTFOUND: not found
             * UNAUTHORIZED - 401 , not auth --> FAIL
             * INV_STRUCT, FAIL --> FAIL
             */
            //ServiceResult<RespType?> repoResult = new();
            //repoResult.Status = ServiceResultEnum.FAIL;
            //repoResult.Message = "Terjadi kesalahan sistem.";
            var repoResult = new ServiceResult<RespType>();

            //for app error 
            List<int> allowedStat = [0];
            if (!allowedStat.Contains(apiResponse.StatusCode))
            {
                repoResult = ServiceResult<RespType>.Fail();
                LogTrace.Error("Fail: error response", apiResponse, classPath);
                return repoResult;
            }

            //for 200 
            if (emptyResult)
            {
                repoResult = ServiceResult<RespType>.SuccessEmpty(msgSuccess);
                return repoResult;
            }

            //for data not found
            if (apiResponse.Data is null)
            {
                LogTrace.Error("Fail: data is null", apiResponse, classPath);
                repoResult = ServiceResult<RespType>.NotFound("Data tidak dapat ditemukan.");

                return repoResult;
            }

            //for data found  get list

            if (mode == RepoProcessEnum.GETLIST)
            {
                var dataType = apiResponse.Data.GetType();
                var genericName = dataType.IsGenericType ? dataType.GetGenericTypeDefinition().Name : "";
                if (genericName.StartsWith("PagedList"))
                {
                    repoResult = ServiceResult<RespType>.Found(apiResponse.Data, msgSuccess);
                    return repoResult;
                    // apiResponse.Data is a PagedList<T>

                }

                LogTrace.Error("Fail: not list", apiResponse, classPath);
                repoResult = ServiceResult<RespType>.NotFound("Data tidak dapat ditemukan.");
                return repoResult;
            }

            if (mode == RepoProcessEnum.GET)
            {
                if (apiResponse.Data is null)
                {
                    repoResult = ServiceResult<RespType>.NotFound();
                    return repoResult;
                }

                repoResult = ServiceResult<RespType>.Found(apiResponse.Data, msgSuccess);
                return repoResult;
            }


            //mode == RepoProcessEnum.ACTION
            if (apiResponse.Data is null)
            {
                repoResult = ServiceResult<RespType>.Fail("Terjadi kesalahan sistem.");
                LogTrace.Error("Fail: data for this action is null", apiResponse, classPath);
                return repoResult;
            }
            repoResult = ServiceResult<RespType>.Success(apiResponse.Data, msgSuccess);

            return repoResult;

        }
    }
}
