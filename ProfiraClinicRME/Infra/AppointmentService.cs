using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using StatusDTO = ProfiraClinicRME.Model.EditStatusTindakanDTO;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Services;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Infra
{
    public class AppointmentService: IAppointmentService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::AppointmentService";

        private TRMAppointment? _current; 

        private BaseRepo<TRMAppointment> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public AppointmentService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<TRMAppointment>();
        }

        /// <summary>
        /// set current appointment on progress for the session
        /// </summary>
        /// <param name="apo"></param>
        /// <returns></returns>
        public ServiceResult<bool> SetCurrent(TRMAppointment apo)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.Status = ServiceResultEnum.SUCCESS;
            _current = apo;
            LogTrace.Info("Fin", _current, _classPath);
            return result;
        }

        public ServiceResult<TRMAppointment> GetCurrent()
        {
            var result = new ServiceResult<TRMAppointment>();

            if (_current == null)
            {
                result.Status = ServiceResultEnum.NOT_FOUND;
                result.Message = "Current appointment not set.";
                LogTrace.Error("Current appointment not set", _classPath);
                return result;
            }
            result.Status = ServiceResultEnum.FOUND;
            result.Data = _current;
            LogTrace.Info("Fin", _current, _classPath);
            return result;
        }

        // Retrieves all clinics.
        public async Task<ServiceResult<PagedList<TRMAppointment>>> GetListOnWaitAsync(string KodeLokasi, DateTime tglAppointment, string KodeKaryawan)
        {
            // Replace the URL with your actual endpoint, e.g., "api/Patient"

            var request = new AppointmentRequest
            {
                KodeLokasi = KodeLokasi,
                TanggalAppointment = tglAppointment,
                KodeKaryawan = KodeKaryawan
            };
            Response<PagedList<TRMAppointment>?> apiResponse = await _svcApi.Send<AppointmentRequest,PagedList<TRMAppointment>>("post","api/Appointment/GetListDokter", request);

            ServiceResult<PagedList<TRMAppointment>> svcResult = _repo.ProcessResult<PagedList<TRMAppointment>>(apiResponse, RepoProcessEnum.GET);
            LogTrace.Info("fin", svcResult, _classPath);
            return svcResult;
        }

        public async Task<ServiceResult<string>> SetAppointmentOnProgress(TRMAppointment apo)
        {
            var request = new StatusDTO
            {
                NomorAppointment = apo.NomorAppointment,
                Status = "2"
            };

            Response<string?> apiResponse = await _svcApi.Send<StatusDTO, string>("post", "api/Appointment/EditStatusTindakan", request, true);

            var svcResult = _repo.ProcessEmptyResult(apiResponse, RepoProcessEnum.GET);
            LogTrace.Info("fin", svcResult, _classPath);

            return svcResult;
        }

    }
}
