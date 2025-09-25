using MudBlazor;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Application;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        private IAuthPublisher _authPublisher;

        private string _classPath = "Infra::ApiService";

        private string StdFailMessage = "Terjadi kesalahan sistem.";
        public ApiService(IHttpClientFactory httpClientFactory, IAuthPublisher authPublisher, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _authPublisher = authPublisher;
            _authPublisher.AuthEvent += OnAuthChanged;
            ApiBaseUri = configuration["ApiSettings:BaseAddress"] ?? "";

            //configuring this.httpClient
        }

        public void SetBearer(string value)
        {
            Bearer = "Bearer " + value;
            //Log.Debug("{src} {stat} {msg} {data}", "Infra::ApiService::SetBearer", 0, "new bearer:" + value);
            LogTrace.Debug("fin: " + Bearer, path: _classPath);
        }

        public void OnAuthChanged(object? sender, AuthEventArgs authEventArgs)
        {
            if(authEventArgs.type == "login" )
            {
                SetBearer(authEventArgs.token);
            }
            else
            {
                SetBearer("");
            }
        }

        //
        public async Task<Response<RespType?>> SendEmpty<RespType>(string method, string reqUrlPath)
        {
            string typeParam = $"{typeof(RespType).Name}>";
            LogTrace.Info($"init", new { typeParam, reqUrlPath}, _classPath);

            return await ExecRequest<RespType>(method, reqUrlPath, null, false, "std");

        }


        /// <summary>
        /// send http request
        /// </summary>
        /// <typeparam name="Entity">response type</typeparam>
        /// <param name="method"></param>
        /// <param name="reqUrlPath">request url path </param>
        /// <param name="dataList"></param>
        /// <param name="emptyResponse">is response should empty</param>
        /// <returns></returns>
        public async Task<Response<RespType?>> Send<ReqType, RespType>(string method, string reqUrlPath, ReqType? request, bool emptyResponse = false, string clientName = "std")
        {
            string typeParam = $"<{typeof(ReqType).Name},{typeof(RespType).Name}>";
            LogTrace.Info($"init", new { typeParam, reqUrlPath}, _classPath);
            
            string jsonContent = JsonSerializer.Serialize<ReqType>(request);
            LogTrace.Info("request json", jsonContent , _classPath);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await ExecRequest<RespType>(method, reqUrlPath, content, emptyResponse, clientName);

        }

        /// <summary>
        /// execute http request
        /// </summary>
        /// <typeparam name="RespType"></typeparam>
        /// <param name="method"></param>
        /// <param name="reqUrlPath"></param>
        /// <param name="content"></param>
        /// <param name="clientName"></param>
        /// <param name="emptyResponse">wether response should be empty</param>
        /// <returns></returns>
        protected virtual async Task<Response<RespType?>> ExecRequest<RespType>(string method, string reqUrlPath, HttpContent content, bool emptyResponse, string clientName )
        {
            string message = "Unknown Error";
            var apiResponse = new Response<RespType?>(1, message, default, ErrorType.UNKNOWN);


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
                serializedObj = await response.Content.ReadAsStringAsync();

                var httpStat = (int)response.StatusCode;
                if (httpStat != 200)
                {
                    //apiResponse.Message = "Unknown Error";
                    //apiResponse.ErrorType = ErrorType.UNKNOWN;
                    LogTrace.Error("fin: sys err", new { httpStat, reqUrlPath, serializedObj }, _classPath);
                    return apiResponse;
                }



                //httpStat 200
                apiResponse.StatusCode = 1;

                if (emptyResponse)
                {
                    apiResponse.StatusCode = 0;
                    return apiResponse;
                }


                LogTrace.Info("response json", new { httpStat, serializedObj, reqUrlPath }, path: _classPath);



                //var responseData = Utility.DeserializeJson<ResponseData<Entity>>(serializedObj);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip, 
                    NumberHandling = JsonNumberHandling.AllowReadingFromString 
                };

                var responseData = JsonSerializer.Deserialize<Response<RespType>>(serializedObj, options );

                //check for returnId and returnMessage
                if (responseData == null)
                {
                    errCode = "AS.ER.01";
                    apiResponse.Message = "Should not empty {errCode}";

                    LogTrace.Error("fin err: should not empty",  new { responseData, apiResponse }, _classPath);
                    return apiResponse;
                }

                //check for statusCode
                List<int> allowedStatusCode = [0, 1];
                var statusCode = responseData.StatusCode;
                if(!allowedStatusCode.Contains(statusCode) )
                {
                    errCode = "AS.ER.02";
                    apiResponse.Message = "Unknown statusCode {statusCode}";
                    apiResponse.StatusCode = 1;
                    LogTrace.Error("Unknown status code", new { responseData, apiResponse }, _classPath);
                    return apiResponse;
                }

                apiResponse.StatusCode = responseData.StatusCode;
                apiResponse.Message = responseData.Message;
                apiResponse.Data = responseData.Data;
                LogTrace.Info("FIN: success", apiResponse, _classPath);
                return apiResponse;
            }
            catch (JsonException ex)
            {
                LogTrace.Error("JsonException", new { type=typeof(RespType).Name, path= ex.Path, BytePos = ex.BytePositionInLine, line = ex.LineNumber }, _classPath);
                apiResponse.Message = "Json Exception";
            }
            catch (AggregateException ex)
            {
                apiResponse.Message = "Aggregate Exception";

            }
            catch (TaskCanceledException ex)
            {
                errCode = "AS.ER.03";
                apiResponse.Message = $"{ex.Message} {errCode}";
                
                // Handle other exceptions

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
            }
            catch (Exception ex)
            {
                message = ex.Message;
                errCode = "AS.ER.05";

                // Handle other exceptions
                //Log.Debug("{src} {stat} {data}", logSource, message, ex.Message);
                apiResponse.Message = $"{errCode} {message} ";
            }
            finally
            {
                response?.Dispose();
            }
            //return for exception
            LogTrace.Error("FIN: ERR " + errCode, apiResponse, _classPath);
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