using ProfiraClinic.Models.Core;

namespace ProfiraClinicRME.Application
{
    public interface ISessionManager
    {

        public Task ClearAppointment();

        public Task ClearUser();

        public Task ClearSession();

        public TRMAppointment? GetCurrentAppointment();

        public bool IsAuthenticated();

        /// <summary>
        /// indicate if we need to fetch data from storage, ussualy after load state in afterrenderasync data is ready
        /// </summary>
        /// <returns></returns>
        public bool NeedLoadState { get; }

        public Task Logout();
        public CurrentUser GetCurrentUser();

        public Task SetCurrentAppointment(TRMAppointment appointment);

        public Task SetCurrentUser(CurrentUser user);

        public Task SetToken(string token);

        public string GetToken();

        public Task LoadState();
    }
}
