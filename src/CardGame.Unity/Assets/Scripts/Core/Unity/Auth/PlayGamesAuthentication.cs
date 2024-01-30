using Core.Auth;
using Core.Basic;
using Core.Security;
using Core.Unity.Storage;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : IAuthentication
    {
        private const string TokenKey = "token";

        private UserData _userData;

        private TaskCompletionSource<Result> _signInCompletionSource;
        private bool _isSignInInProgress = false;

        public string UserId => _userData.Id;
        public string UserName => _userData.Name;
        public string AuthCode => _userData.AuthCode;

        public async Task<string> GetAuthCode()
        {
            if (_userData?.AuthCode is not null)
            {
                if (Jwt.ValidateAndDecodeToken(_userData.AuthCode))
                    return _userData.AuthCode;

                var token = SecurePlayerPrefs.GetString(TokenKey);
                if (Jwt.ValidateAndDecodeToken(token))
                {
                    _userData = _userData with { AuthCode = token };
                    return _userData.AuthCode;
                }
            }
            
            await SignIn();

            return _userData.AuthCode;
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

                PlayGamesPlatform.Instance.RequestServerSideAccess(forceRefreshToken: false, token =>
                {
                    if (token is null)
                    {
                        HandleSignInError("Sign In Failed");
                        return;
                    }

                    var userData = new UserData(
                        PlayGamesPlatform.Instance.GetUserId(),
                        PlayGamesPlatform.Instance.GetUserDisplayName(),
                        token);

                    HandleSignInSuccess(userData);
                });
            });

            return _signInCompletionSource.Task;
        }

        private void HandleSignInSuccess(UserData userData)
        {
            _isSignInInProgress = false;
            _signInCompletionSource?.SetResult(Result.Success());
            
            _userData = userData;

            SecurePlayerPrefs.SetString(TokenKey, _userData.AuthCode);
        }

        private void HandleSignInError(string error)
        {
            _isSignInInProgress = false;
            _signInCompletionSource?.SetResult(Result.Failure(error));

            SecurePlayerPrefs.DeleteKey(TokenKey);
        }
    }

    public record UserData(string Id, string Name, string AuthCode);
}
