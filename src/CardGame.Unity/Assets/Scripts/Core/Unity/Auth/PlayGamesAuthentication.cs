using Core.Auth;
using Core.Basic;
using Core.Net.Http;
using Core.Security;
using Core.Threads;
using Core.Unity.Storage;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : IAuthentication
    {
        private const string AccessTokenKey = "access-token";

        private readonly IHttpClientAccessor _clientAccessor;
        private readonly string _tokenUri;
        private readonly string _clientName;

        private AuthResult _authResult;

        private TaskCompletionSource<Result> _signInCompletionSource;
        private bool _isSignInInProgress = false;
         
        public string UserId => PlayGamesPlatform.Instance.GetUserId();
        public string UserName => PlayGamesPlatform.Instance.GetUserDisplayName();

        private string _authCode;
        private string _accessToken;

        private bool _firebaseInitialized;

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

            //var storedToken = SecurePlayerPrefs.GetString(AccessTokenKey);
            //if (Jwt.ValidateAndDecodeToken(storedToken))
                //return _accessToken = storedToken;

            await SignIn();

            //var result = await _clientAccessor.PostResource<TokenPostRequest, TokenPostResponse, TokenPostResponse>(
            //    _clientName, _tokenUri, new(_authCode));

            //if (result.Error is not null || result.Body is null)
            //    return null;

            //_accessToken = result.Body.AccessToken;

            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);

            return _accessToken;
        }

        private async void OnFirebaseAuthChanged(object sender, EventArgs args)
        {
            var credential = PlayGamesAuthProvider.GetCredential(_authCode);
            _authResult = await FirebaseAuth.DefaultInstance.SignInAndRetrieveDataWithCredentialAsync(credential);
            //_authResult.User.TokenAsync
            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);
        }

        public void SignIn2()
        {
            //Firebase.Auth.FirebaseUser user = auth.CurrentUser;
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

                    _authCode = authCode;

                    if (!_firebaseInitialized)
                        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                        {
                            FirebaseAuth.DefaultInstance.StateChanged += (sender, args) =>
                            {
                                var credential = PlayGamesAuthProvider.GetCredential(_authCode);
                                FirebaseAuth.DefaultInstance.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
                                {
                                    _authResult = task.Result;
                                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                                        _authResult.User.DisplayName, _authResult.User.UserId);

                                    _isSignInInProgress = false;
                                    _signInCompletionSource?.SetResult(Result.Success());
                                });
                            };
                        });
                    else
                    {
                        _isSignInInProgress = false;
                        _signInCompletionSource?.SetResult(Result.Success());
                    }
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
