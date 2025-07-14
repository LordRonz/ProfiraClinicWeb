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
    public class CustomerService: ICustomerService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::CustomerService";

        private BaseRepo<Customer> _repo;

        private Customer? _current;

        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public CustomerService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<Customer>();
        }

        public ServiceResult<bool> SetCurrent(Customer customer)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.Status = ServiceResultEnum.SUCCESS;
            _current = customer;
            LogTrace.Info("Fin", _current, _classPath);
            return result;
        }

        public async Task<ServiceResult<Customer>> GetByIdAsync(long id)
        {
            //Response<Customer?> apiResponse = await _svcApi.SendEmpty<Customer>("get","api/Patient/GetById/" + Id);

            //ServiceResult<Customer> svcResult = _repo.ProcessResult<Customer>(apiResponse, RepoProcessEnum.GET);

            ServiceResult<Customer> svcResult = await _repo.GetById(id);

            return svcResult;
        }

        public async Task<ServiceResult<Customer>> GetByCodeAsync(string code)
        {

            Response<Customer?> apiResponse = await _svcApi.SendEmpty<Customer>("get","api/Patient/GetByCode/" + code);

            ServiceResult<Customer> svcResult = _repo.ProcessResult<Customer>(apiResponse, RepoProcessEnum.GET);


            return svcResult;
        }

    }
}
