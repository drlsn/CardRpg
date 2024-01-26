using Core.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private string _textStr = "Loading";

        public void Start()
        {
            SignIn();
        }

        public void SignIn()
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        private void Update()
        {
            _text.text = _textStr;
        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                var userId = PlayGamesPlatform.Instance.GetUserId();
                var userName = PlayGamesPlatform.Instance.GetUserDisplayName();

                _textStr = $"Sign In Success\nWelcome {userName}";

                PlayGamesPlatform.Instance.RequestServerSideAccess(forceRefreshToken: false, token =>
                {
                    if (token is null)
                        _textStr = "Token error";
                    else
                        _textStr += $"\nToken {token.Take(10).ToStr()}";
                });
                //FirebaseUser firebaseUser = _auth.CurrentUser;
                //if (firebaseUser is not null)
                //{
                //    string accessToken = firebaseUser.TokenAsync(false).Result;
                //    Debug.Log($"Access Token: {accessToken}");
                //    _text.text += $"\nToken {accessToken.Take(10).ToStr()}";
                //}
            }
            else
            {
                _text.text = "Sign In Failed";
            }
        }
    }
}
