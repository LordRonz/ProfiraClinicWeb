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

        // 
        /// <summary>
        /// used to process getById, getList
        /// </summary>
        /// <typeparam name="RespType"></typeparam>
        /// <param name="apiResponse"></param>
        /// <param name="mode"></param>
        /// <param name="msgSuccess"></param>
        /// <returns></returns>
        public ServiceResult<RespType> ProcessResult<RespType>(Response<RespType?> apiResponse, RepoProcessEnum mode = RepoProcessEnum.ACTION, string msgSuccess = "")
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
            ServiceResult<RespType> repoResult = new();
            repoResult.Status = ServiceResultEnum.FAIL;
            repoResult.Message = "Terjadi kesalahan sistem.";

            //four 404
            if (apiResponse.StatusCode != 200)
            {
                LogTrace.Error("Fail", apiResponse, classPath);
                return repoResult;
            }

            //for 200 
            //for data not found
            if (apiResponse.Data is null)
            {
                LogTrace.Error("Fail", apiResponse, classPath);
                repoResult.Message = "Data tidak dapat ditemukan.";
                return repoResult;
            }

            //for data found  get list
            
            if(mode == RepoProcessEnum.GETLIST )
            {
                var dataType = apiResponse.Data.GetType();

                if (dataType.IsGenericType == false)
                {
                    LogTrace.Error("Fail: not list", apiResponse, classPath);

                    return repoResult;
                }

                var genericName = dataType.IsGenericType ? dataType.GetGenericTypeDefinition().Name:"";
                if (genericName.StartsWith("PagedList"))
                {
                    // apiResponse.Data is a PagedList<T>
                    repoResult.Status = ServiceResultEnum.FOUND;
                    repoResult.Message = msgSuccess == "" ? apiResponse.Message : msgSuccess;
                    repoResult.Data = apiResponse.Data;

                }

            }
            // for data found get by id
            repoResult.Status = ServiceResultEnum.FOUND;
            repoResult.Message = msgSuccess == "" ? apiResponse.Message : msgSuccess;
            repoResult.Data = apiResponse.Data;

            return repoResult;

        }
    }
}
