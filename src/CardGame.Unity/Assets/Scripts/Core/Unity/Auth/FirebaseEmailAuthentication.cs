using Core.Auth;
using Core.Basic;
using Core.Collections;
using Core.Security;
using Core.Unity.Storage;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Unity.Auth
{
    public class FirebaseEmailAuthentication : IAuthentication
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

        public string Email { set; get; }
        public string Password { set; get; }

        public async Task<string> GetAccessToken()
        {
            if (!_accessToken.IsNullOrEmpty() && Jwt.ValidateAndDecodeToken(_accessToken))
                return _accessToken;

            _accessToken = SecurePlayerPrefs.GetString(AccessTokenKey);
            if (!_accessToken.IsNullOrEmpty() && Jwt.ValidateAndDecodeToken(_accessToken))
                return _accessToken;

            if (IsNotValidCredentials())
                return null;

            await SignIn();

            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (!_accessToken.IsNullOrEmpty())
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);

            return _accessToken;
        }

        private async void OnFirebaseAuthChanged(object sender, EventArgs args)
        {
            var credential = PlayGamesAuthProvider.GetCredential(_authCode);
            _authResult = await FirebaseAuth.DefaultInstance.SignInAndRetrieveDataWithCredentialAsync(credential);
            _accessToken = await FirebaseAuth.DefaultInstance.CurrentUser.TokenAsync(forceRefresh: true);
            if (_accessToken is not null)
                SecurePlayerPrefs.SetString(AccessTokenKey, _accessToken);
        }
        static bool IsValidEmail(string email) =>
            Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

        private bool IsNotValidCredentials() =>
            Email.IsNullOrEmpty() || !IsValidEmail(Email) || Password.IsNullOrEmpty() || Password.Length < 6;

        public Task<Result> SignIn()
        {
            if (IsNotValidCredentials())
                return Task.FromResult(Result.Failure("Invalid credentials"));

            if (_isSignInInProgress)
                return _signInCompletionSource.Task;

            _isSignInInProgress = true;
            _signInCompletionSource = new();

            if (!_firebaseInitialized)
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    FirebaseAuth.DefaultInstance.StateChanged += (sender, args) =>
                    {
                        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(Email, Password).ContinueWith(task =>
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

            return _signInCompletionSource.Task;
        }

        private void HandleSignInError(string error)
        {
            _isSignInInProgress = false;
            _signInCompletionSource?.SetResult(Result.Failure(error));
        }
    }
}
