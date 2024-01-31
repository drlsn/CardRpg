using Core.Auth;
using Core.Basic;
using Core.Net.Http;
using Core.Security;
using Core.Unity.Storage;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : IAuthentication
    {
        private const string AccessTokenKey = "access-token";

        private IHttpClientAccessor _clientAccessor { get; set; }
        private readonly string _tokenUri;

        private TaskCompletionSource<Result> _signInCompletionSource;
        private bool _isSignInInProgress = false;

        public string UserId => PlayGamesPlatform.Instance.GetUserId();
        public string UserName => PlayGamesPlatform.Instance.GetUserDisplayName();

        private string _authCode;
        private string _accessToken;

        public PlayGamesAuthentication(string tokenUri, IHttpClientAccessor clientAccessor)
        {
            _tokenUri = tokenUri;
            _clientAccessor = clientAccessor;
        }

        static async Task<HttpResponseMessage> PostJsonAsync(IHttpClientAccessor clientAccessor, string apiUrl, string jsonBody)
        {
            var client = clientAccessor.Get("trinica-public");
            var address = client.BaseAddress + apiUrl;
            return await client.PostAsync(apiUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
        }

        class TokenPostResponse
        {
            public string AccessToken { get; init; }
        }

        public async Task<string> GetAccessToken()
        {
            if (_accessToken is not null && Jwt.ValidateAndDecodeToken(_accessToken))
                return _accessToken;

            var storedToken = SecurePlayerPrefs.GetString(AccessTokenKey);
            if (Jwt.ValidateAndDecodeToken(storedToken))
                return _accessToken = storedToken;

            await SignIn();

            _accessToken = await RetrieveAccessTokenFromServer();
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);

            return _accessToken;
        }

        private async Task<string> RetrieveAccessTokenFromServer()
        {
            try
            {
                var requestBody = new { Code = _authCode };
                var jsonBody = JsonConvert.SerializeObject(requestBody);
                var response = await PostJsonAsync(_clientAccessor, _tokenUri, jsonBody);
                response.EnsureSuccessStatusCode();

                var jsonResult = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenPostResponse>(jsonResult);

                return tokenResponse.AccessToken;
            }
            catch (HttpRequestException ex)
            {
                SecurePlayerPrefs.DeleteKey(AccessTokenKey);
                return null;
            }
        }

        public Task<Result> SignIn()
        {
            if (_isSignInInProgress)
                return _signInCompletionSource.Task;

            _isSignInInProgress = true;
            _signInCompletionSource = new();

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(signInStatus =>
            {
                if (signInStatus is not SignInStatus.Success)
                {
                    HandleSignInError("Sign In Failed");
                    return;
                }

                PlayGamesPlatform.Instance.RequestServerSideAccess(forceRefreshToken: false, authCode =>
                {
                    if (authCode is null)
                    {
                        HandleSignInError("Sign In Failed");
                        return;
                    }

                    _isSignInInProgress = false;
                    _signInCompletionSource?.SetResult(Result.Success());

                    _authCode = authCode;
                });
            });

            return _signInCompletionSource.Task;
        }

        private void HandleSignInError(string error)
        {
            _isSignInInProgress = false;
            _signInCompletionSource?.SetResult(Result.Failure(error));
        }
    }

    public record UserData(string Id, string Name, string AuthCode, string AccessToken = null);
}
