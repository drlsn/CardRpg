using Core.Auth;
using Core.Basic;
using Core.Net.Http;
using System;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public class CustomServerAuthentication : IAuthentication
    {
        public IAuthentication ProviderAuthentication { get; }
        private readonly IHttpClientAccessor _clientAccessor;
        private readonly string _clientName;

        public CustomServerAuthentication(
            IAuthentication providerAuthentication,
            IHttpClientAccessor clientAccessor,
            string clientName)
        {
            ProviderAuthentication = providerAuthentication;
            _clientAccessor = clientAccessor;
            _clientName = clientName;
        }

        public string UserId => ProviderAuthentication.UserId;

        public string UserName => ProviderAuthentication.UserName;

        public Task<Result<string>> GetAccessToken() => ProviderAuthentication.GetAccessToken();

        public async Task<Result> SignIn()
        {
            var providerSignInResult = await ProviderAuthentication.SignIn();
            if (!providerSignInResult.IsSuccess)
                return providerSignInResult;
            
            return await CreateUser();
        }

        private async Task<Result> CreateUser()
        {
            try
            {
                var client = _clientAccessor.Get(_clientName);
                var createUserResponse = await client.Post("/api/v1/users");
                if (!createUserResponse.IsSuccessStatusCode)
                    return Result.Failure("Create user failed");
            }
            catch (Exception ex)
            {
                return Result.Failure("Server not running");
            }

            return Result.Success();
        }
    }
}
