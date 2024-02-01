using Core.Auth;
using Core.Basic;
using Core.Net.Http;
using Core.Security;
using Core.Unity.Storage;
using Corelibs.BlazorShared;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : IAuthentication
    {
        private const string AccessTokenKey = "access-token";

        private readonly IHttpClientAccessor _clientAccessor;
        private readonly string _tokenUri;
        private readonly string _clientName;

        private TaskCompletionSource<Result> _signInCompletionSource;
        private bool _isSignInInProgress = false;

        public string UserId => PlayGamesPlatform.Instance.GetUserId();
        public string UserName => PlayGamesPlatform.Instance.GetUserDisplayName();

        private string _authCode;
        private string _accessToken;

        public PlayGamesAuthentication(
            string tokenUri, IHttpClientAccessor clientAccessor, string clientName)
        {
            _tokenUri = tokenUri;
            _clientAccessor = clientAccessor;
            _clientName = clientName;
        }

        public async Task<string> GetAccessToken()
        {
            if (_accessToken is not null && Jwt.ValidateAndDecodeToken(_accessToken))
                return _accessToken;

            var storedToken = SecurePlayerPrefs.GetString(AccessTokenKey);
            if (Jwt.ValidateAndDecodeToken(storedToken))
                return _accessToken = storedToken;

            await SignIn();

            var result = await _clientAccessor.PostResource<TokenPostRequest, TokenPostResponse, TokenPostResponse>(
                _clientName, _tokenUri, new(_authCode));

            if (result.Error is not null || result.Body is null)
                return null;


            _accessToken = result.Body.AccessToken;
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);

            return _accessToken;
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

    record TokenPostRequest(string Code);
    record TokenPostResponse(string AccessToken);
}
