using AutoBogus;
using Bogus;
using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;
using ProfiraClinicRME.Infra;

namespace ProfiraClinicRME.Test.Infra
{
    public class MockDiagnosaService : IDiagnosaService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Test::Infra::DiagnosaService";

        private List<TRMDiagnosa> _store;

        private Faker<TRMDiagnosa> _faker;



        private BaseRepo<TRMDiagnosa> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public MockDiagnosaService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMDiagnosa>();
            _faker = new AutoFaker<TRMDiagnosa>()
            .RuleFor(o => o.NomorTransaksi, f =>
            {
                string year = f.Random.Number(2000, 2010).ToString(); // Random year between 2000 and 2010
                string month = f.Random.Number(1, 12).ToString("D2"); // Random month, zero-padded
                string number = f.Random.Number(1, 9000).ToString("D4"); // Random number, zero-padded to 4 digits
                return $"DIAG/{year}/{month}/{number}";
            });

            _store = _faker.Generate(20).ToList();
        }


        // Retrieves all clinics.
        public async Task<ServiceResult<Pagination<TRMDiagnosa>>> GetListAsync(string kodeCustomer)
        {
            ServiceResult<Pagination<TRMDiagnosa>> svcResult = new()
            {
                Status = ServiceResultEnum.FOUND,
                Data = new Pagination<TRMDiagnosa>
                {
                    Items = _store,
                    TotalCount = _store.Count,
                    PageSize = 10,
                    Page = 1
                },
            };

            return svcResult;
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Add(TRMDiagnosa diagnosa)
        {
            var _list = _faker.Generate(1);
            diagnosa.NomorTransaksi = _list.First().NomorTransaksi;
            _store.Add(diagnosa);

            ServiceResult<NomorTransaksiDto> svcResult = new()
            {

                Status = ServiceResultEnum.SUCCESS,
                Data = new NomorTransaksiDto()
                {
                    NomorTransaksi = diagnosa.NomorTransaksi
                }
            };

            return svcResult;
        }

        public async Task<ServiceResult<TRMDiagnosa>> GetByNomorAppointment(string nomorAppointment)
        {
            var _item = _store.FirstOrDefault(item => item.NomorTransaksi == nomorAppointment);

            ServiceResult<TRMDiagnosa> svcResult = new()
            {
                Status = ServiceResultEnum.NOT_FOUND,
                Message = "Data diagnosa tidak dapat ditemukan."
            };

            if (_item == null)
            {
                return svcResult;
            } 
            else
            {
                svcResult.Status = ServiceResultEnum.FOUND;
                svcResult.Message = "Data diagnosa ditemukan.";
                return svcResult;
            }
        }

        public async Task<ServiceResult<NomorTransaksiDto>> Edit(TRMDiagnosa item)
        {
            ServiceResult<NomorTransaksiDto> svcResult = new()
            {
                Status = ServiceResultEnum.SUCCESS,
                Message = "Diagnosa berhasil diperbarui."
            };

            return svcResult;
        }
    }
}
