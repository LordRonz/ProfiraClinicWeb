using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using ProfiraClinic.Models.Api;
using System.Text.Json;
using ProfiraClinicRME.Utils;
using ProfiraClinic.Models.Core;
using BootstrapBlazor.Components;

namespace ProfiraClinicRME.Infra
{



    //public class ApiResult<Entity>
    //{
    //    public ApiOpResultEnum status;
    //    public string message;
    //    public List<Entity>? respObjectList;
    //}

    public class ApiService
    {
        private string ApiBaseUri = "";

        private string Token = "";

        private string Bearer = "";

        private readonly IHttpClientFactory _httpClientFactory;

        private string _classPath = "Infra::ApiService";

        private string StdFailMessage = "Terjadi kesalahan sistem.";
        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            ApiBaseUri = configuration["ApiSettings:BaseAddress"] ?? "";

            //configuring this.httpClient
        }

        public void SetBearer(string value)
        {
            Bearer = "Bearer " + value;
            //Log.Debug("{src} {stat} {msg} {data}", "Infra::ApiService::SetBearer", 0, "new bearer:" + value);
            LogTrace.Debug("fin: " + Bearer, path: _classPath);
        }

        //
        public async Task<Response<ReqType>> SendEmpty<ReqType>(string mode, string reqUrlPath)
        {
            string typeParam = $"{typeof(ReqType).Name}>";
            LogTrace.Info($"init", typeParam, _classPath);

            return await ExecRequest<ReqType>(mode, reqUrlPath, null, "std");

        }



        /// <summary>
        /// send http request
        /// </summary>
        /// <typeparam name="Entity">response type</typeparam>
        /// <param name="method"></param>
        /// <param name="reqUrlPath">request url path </param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public async Task<Response<RespType?>> Send<ReqType, RespType>(string method, string reqUrlPath, ReqType? reqType, long pageNum = 0, long pageSize = 0, string clientName = "std")
        {
            string typeParam = $"<{typeof(ReqType).Name},{typeof(RespType).Name}>";
            LogTrace.Info($"init", typeParam, _classPath);
            
            var jsonContent = JsonSerializer.Serialize(reqType);
            LogTrace.Info("request", new { reqUrlPath, jsonContent }, _classPath);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await ExecRequest<RespType>(method, reqUrlPath, content, clientName);

        }

        protected virtual async Task<Response<RespType?>> ExecRequest<RespType>(string method, string reqUrlPath, HttpContent content, string clientName)
        {
            string message = "Unknown Error";
            var apiResponse = new Response<RespType>(404, message);
            apiResponse.ErrorType = ErrorType.UNKNOWN;

            LogTrace.Info("init", new { method, reqUrlPath, clientName }, _classPath);
            //check for http status
            HttpResponseMessage response = new();
            var errCode = "";
            var serializedObj = "";
            try
            {
                //bug happened when using await
                using var client = CreateHttpClient(clientName);
                if (method == "post")
                {
                    response = await client.PostAsync(reqUrlPath, content);
                }
                else
                {
                    response = await client.GetAsync(reqUrlPath);
                }


                //Log.Debug("{src} {stat} {data}", logSource, 1, (response.ToString(), client.DefaultRequestHeaders));
                var httpStat = (int)response.StatusCode;

                serializedObj = await response.Content.ReadAsStringAsync();

                LogTrace.Info("response", new { httpStat, serializedObj, reqUrlPath }, path: _classPath);

                if (httpStat != 200)
                {
                    //apiResponse.Message = "Unknown Error";
                    //apiResponse.ErrorType = ErrorType.UNKNOWN;
                    LogTrace.Info("fin", apiResponse, _classPath);
                    return apiResponse;
                }
                

                //httpStat 200


                //var responseData = Utility.DeserializeJson<ResponseData<Entity>>(serializedObj);
                var responseData = JsonSerializer.Deserialize<ApiResponse<RespType>>(serializedObj);
                //check for returnId and returnMessage
                if (responseData == null || responseData.StatusCode != 200)
                {
                    errCode = "AS.ER.01";
                    apiResponse.Message = StdFailMessage + errCode;

                    LogTrace.Info("fin", apiResponse, _classPath);
                    return apiResponse;
                }


            }
            catch (JsonException ex)
            {
                LogTrace.Error("JsonExc", serializedObj, _classPath);
                apiResponse.Message = "Json Exception";
            }
            catch (AggregateException ex)
            {
                LogTrace.Error(message, ex, _classPath);
                apiResponse.Message = "Aggregate Exception";

            }
            catch (TaskCanceledException ex)
            {
                errCode = "AS.ER.03";
                apiResponse.Message = $"{ex.Message} {errCode}";
                
                // Handle other exceptions
                //Log.Debug("{src} {stat} {data}", logSource, message, ex.Message);
                LogTrace.Error("err: " + errCode, ex.Message, _classPath);

            }
            catch (HttpRequestException ex)
            {
                message = ex.Message;
                errCode = "AS.ER.04";

                // Handle network-related or timeout exceptions
                if (ex.InnerException is TimeoutException)
                {
                    message = ex.InnerException.Message;
                    errCode = "AS.ER.04";

                }
                //Log.Debug("{src} {stat} {data}", logSource, message, ex.Message);
                apiResponse.Message = $"{message} {errCode}";
                LogTrace.Error("err: " + errCode, apiResponse, _classPath);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                errCode = "AS.ER.05";

                // Handle other exceptions
                //Log.Debug("{src} {stat} {data}", logSource, message, ex.Message);
                apiResponse.Message = $"{message} {errCode}";
                LogTrace.Error("err: " + errCode, apiResponse, _classPath);
            }
            finally
            {
                response?.Dispose();
            }
            //return for exception
            LogTrace.Error("err: " + errCode, apiResponse, _classPath);
            return apiResponse;
        }


        private HttpClient CreateHttpClient(string clientName)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            client.BaseAddress = new Uri(ApiBaseUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (Bearer == "")
            {
                LogTrace.Info("NO_BEARER", path: _classPath);
                return client;
                //throw new Exception("no bearer fragment");
            }
            client.DefaultRequestHeaders.Add("Authorization", Bearer);
            //LogTrace.Info("SUCCESS");
            return client;
        }

    }

}