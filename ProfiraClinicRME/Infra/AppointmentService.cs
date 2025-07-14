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
    public class AppointmentService: IAppointmentService
    {
        private readonly ApiService _svcApi;

        private string _classPath = "Infra::AppointmentService";

        private TRMAppointment? _current; 

        private BaseRepo<Appointment> _repo;
        // Inject the HttpClient (assuming it is configured in Program.cs or Startup.cs)
        public AppointmentService(ApiService svcApi)
        {
            _svcApi = svcApi;
            _repo = new BaseRepo<Appointment>();
        }

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
            result.Status = ServiceResultEnum.SUCCESS;
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

            return svcResult;
        }

    }
}
