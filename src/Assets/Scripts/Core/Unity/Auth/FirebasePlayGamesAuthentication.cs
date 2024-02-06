#if UNITY_ANDROID

using Core.Auth;
using Core.Basic;
using Core.Collections;
using Core.Security;
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
    public class FirebasePlayGamesAuthentication : IAuthentication
    {
        private const string AccessTokenKey = "access-token";

        private AuthResult _authResult;

        private TaskCompletionSource<Result> _signInCompletionSource;
        private bool _isSignInInProgress = false;
             
        public string UserId => PlayGamesPlatform.Instance.GetUserId();
        public string UserName => PlayGamesPlatform.Instance.GetUserDisplayName();

        private string _authCode;
        private string _accessToken;

        private bool _firebaseInitialized;

        public async Task<Result<string>> GetAccessToken()
        {
            var result = Result<string>.Success();

            if (Application.internetReachability == NetworkReachability.NotReachable)
                return result.Fail("No internet connection");

            if (_accessToken is not null && Jwt.ValidateAndDecodeToken(_accessToken))
                return result.With(_accessToken);

            _accessToken = SecurePlayerPrefs.GetString(AccessTokenKey);
            if (!_accessToken.IsNullOrEmpty() && Jwt.ValidateAndDecodeToken(_accessToken))
                return result.With(_accessToken);

            SecurePlayerPrefs.DeleteKey(AccessTokenKey);
            var signInResult = await SignIn();
            if (!signInResult.IsSuccess)
                return result.Fail(signInResult.Messages);

            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);

            return result.With(_accessToken);
        }

        private async void OnFirebaseAuthChanged(object sender, EventArgs args)
        {
            var credential = PlayGamesAuthProvider.GetCredential(_authCode);
            _authResult = await FirebaseAuth.DefaultInstance.SignInAndRetrieveDataWithCredentialAsync(credential);
            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);
        }

        public Task<Result> SignIn()
        {
            if (_isSignInInProgress)
                return _signInCompletionSource.Task;

            _isSignInInProgress = true;
            _signInCompletionSource = new();

            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                RequestAuthCode();
                return _signInCompletionSource.Task;
            }

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(signInStatus =>
            {
                if (signInStatus is not SignInStatus.Success)
                {
                    HandleSignInError("Sign In Failed");
                    return;
                }

                RequestAuthCode();
            });

            return _signInCompletionSource.Task;

            void RequestAuthCode()
            {
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
            }
        }

        private void HandleSignInError(string error)
        {
            _isSignInInProgress = false;
            _signInCompletionSource?.SetResult(Result.Failure(error));
        }
    }
}

#endif