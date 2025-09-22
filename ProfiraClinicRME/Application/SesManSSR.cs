using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Utils;
using System.Net.Http;

namespace ProfiraClinicRME.Application
{
    public class AuthEventArgs : EventArgs
    {
        public string type { get; set; }

        public string token { get; set; }
        public AuthEventArgs(string type, string token)
        {
            this.type = type;
            this.token = token;
        }
    }

    /// <summary>
    /// implementation session manager for server side blazor
    /// </summary>
    public class SesManSSR: ISessionManager, IAuthPublisher
    {
        public event EventHandler<AuthEventArgs>? AuthEvent;

        private readonly ProtectedSessionStorage _protectedSessionStore;

        private bool _isAuthenticated;
        private string _classPath;
        private TRMAppointment? _appointment;
        private CurrentUser? _user;
        private string? _token;
        private bool _needLoadState;
        public bool NeedLoadState { get=>_needLoadState; }

        public SesManSSR(ProtectedSessionStorage protectedSessionStore)
        {
            _isAuthenticated = false;
            _needLoadState = true;
            _protectedSessionStore = protectedSessionStore;
            _classPath = "Infra::SesManSSR";
        }

        public async Task ClearAppointment()
        {
            await _protectedSessionStore.DeleteAsync("appointment");
        }

        public async Task ClearUser()
        {
            await _protectedSessionStore.DeleteAsync("user");
        }

        public async Task ClearSession()
        {
            await ClearAppointment();
            await ClearUser();
        }

        public  TRMAppointment? GetCurrentAppointment()
        {
            LogTrace.Info("start", path: _classPath);

            //if(_appointment == null ) throw new AppException("Invalid call: appointment not choosen.");

            //var result = await _protectedSessionStore.GetAsync<TRMAppointment>("appointment");
            //if (!result.Success) throw new AppException("Invalid call: not authenticated");
            
            return _appointment;
            
        }

        public CurrentUser GetCurrentUser()
        {
            //LogTrace.Info("start", path: _classPath);

            if (_user == null) throw new AppException("Invalid call: not authenticated.");

            return _user!;

        }

        public string GetToken()
        {
            LogTrace.Info("start", path: _classPath);

            if (_token == null) throw new AppException("Invalid call: not authenticatd.");

            return _token;
        }

        public bool IsAuthenticated()
        {
            return _isAuthenticated;
        }

        public async Task LoadState()
        {
            LogTrace.Info("start", path: _classPath);
            //get user
            var resUser = await _protectedSessionStore.GetAsync<CurrentUser>("user");
            _user = resUser.Success ? resUser.Value : null;

            //get token
            var resToken = await _protectedSessionStore.GetAsync<string>("token");
            _token = resToken.Success ? resToken.Value ?? "" : "";

            //set authentication state
            _isAuthenticated = !(_token == "") && (_user is null ? false : true);
            if (_isAuthenticated)
            {
                AuthEvent?.Invoke(this, new AuthEventArgs("login", _token));
            }

            //get current appointment
            var resAppo = await _protectedSessionStore.GetAsync<TRMAppointment>("appointment");
            _appointment = resAppo.Success ? resAppo.Value : null;

            _needLoadState = false;
            LogTrace.Info("fin", new { _isAuthenticated, _user, _token } , _classPath);
        }


        public async Task Logout()
        {
            await ClearSession();
            _isAuthenticated = false;
            AuthEvent?.Invoke(this, new AuthEventArgs("logout", _token));
        }

        public async Task SetCurrentAppointment(TRMAppointment appointment)
        {
            LogTrace.Info("start", path: _classPath);

            await _protectedSessionStore.SetAsync("appointment", appointment);
            _appointment = appointment;
        }

        public async Task SetCurrentUser(CurrentUser user)
        {
            LogTrace.Info("start", path: _classPath);

            await _protectedSessionStore.SetAsync("user", user);
            _user = user;
        }

        public async Task SetToken(string token)
        {
            LogTrace.Info("start", path: _classPath);
            await _protectedSessionStore.SetAsync("token", token);
            //_svcApi.SetBearer(authResult.Data.Token);
            _token = token;
            _isAuthenticated = true;
            AuthEvent?.Invoke(this, new AuthEventArgs("login", token));
        }

    }
}
