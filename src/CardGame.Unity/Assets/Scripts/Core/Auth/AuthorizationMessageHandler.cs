using Core.Collections;
using Core.Security;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IAuthentication _authentication;

        private AuthenticationHeaderValue _cachedHeader;
        private string _token;

        public AuthorizationMessageHandler(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_token == null || !Jwt.ValidateAndDecodeToken(_token))
            {
                _token = await _authentication.GetAuthCode();
                if (_token.IsNullOrEmpty())
                    throw new Exception("Authorization token couldn't be obtained.");

                _cachedHeader = new AuthenticationHeaderValue("Bearer", _token);
            }

            request.Headers.Authorization = _cachedHeader;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
