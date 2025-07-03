using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ProfiraClinicRME.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ProfiraClinicRME.MessageHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public BearerTokenHandler(ITokenProvider tokenProvider)
          => _tokenProvider = tokenProvider;

        protected override Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_tokenProvider.Token))
            {
                request.Headers.Authorization =
                  new AuthenticationHeaderValue("Bearer", _tokenProvider.Token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
