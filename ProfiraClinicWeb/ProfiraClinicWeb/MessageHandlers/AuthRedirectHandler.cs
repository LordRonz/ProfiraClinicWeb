using Microsoft.AspNetCore.Components;
using ProfiraClinicWeb.Services;
using System.Net;

namespace ProfiraClinicWeb.MessageHandlers
{
    public class AuthRedirectHandler : DelegatingHandler
    {
        private readonly INavigationRedirector _redirector;

        public AuthRedirectHandler(INavigationRedirector redirector)
        {
            _redirector = redirector;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                _redirector.ShouldRedirect = true;
                _redirector.TargetUrl = "/admin/login";
            }

            return response;
        }
    }

}
