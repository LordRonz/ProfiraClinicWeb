using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;

namespace ProfiraClinicRME.Infra
{
    public class DokterService : IDokterService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::DokterService";

        private BaseRepo<Dokter> _repo;

        public DokterService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<Dokter>();
        }

        public async Task<ServiceResult<Pagination<DokterListDto>>> GetListDokterAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            //Response<Pagination<DokterListDto>?> apiResponse = await _svcApi.SendEmpty<Pagination<DokterListDto>>("get", $"api/Dokter/GetList");

            //ServiceResult<Pagination<DokterListDto>> svcResult = _repo.ProcessResult<Pagination<DokterListDto>>(apiResponse, RepoProcessEnum.GET);

            var svcResult = new ServiceResult<Pagination<DokterListDto>>
            {
                Status = ServiceResultEnum.FOUND,
                Data = new Pagination<DokterListDto>
                {

                    Items = new List<DokterListDto> {
                        new DokterListDto { KodeKaryawan = "D001", NamaDokter = "dr. Jeanny Doe" },
                        new DokterListDto { KodeKaryawan = "D002", NamaDokter = "dr. John Smith" },
                        new DokterListDto { KodeKaryawan = "D003", NamaDokter = "dr. Alice Johnson" },
                        new DokterListDto { KodeKaryawan = "D004", NamaDokter = "dr. Michael Brown" },
                    }
                }
            };

            

            return svcResult;
        }

        public async Task<ServiceResult<Pagination<DokterListDto>>> GetListNonDokterAsync()
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            //Response<Pagination<DokterListDto>?> apiResponse = await _svcApi.SendEmpty<Pagination<DokterListDto>>("get", $"api/Dokter/GetList");

            //ServiceResult<Pagination<DokterListDto>> svcResult = _repo.ProcessResult<Pagination<DokterListDto>>(apiResponse, RepoProcessEnum.GET);

            var svcResult = new ServiceResult<Pagination<DokterListDto>>
            {
                Status = ServiceResultEnum.FOUND,
                Data = new Pagination<DokterListDto>
                {

                    Items = new List<DokterListDto> {
                        new DokterListDto { KodeKaryawan = "S001", NamaDokter = "John Doe" },
                        new DokterListDto { KodeKaryawan = "S002", NamaDokter = "Adam Smith" },
                        new DokterListDto { KodeKaryawan = "S003", NamaDokter = "Betty Johnson" },
                        new DokterListDto { KodeKaryawan = "S004", NamaDokter = "Douglas Brown" },
                    }
                }
            };



            return svcResult;
        }
    }
}
