using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;

namespace Core.Unity.Auth
{
    public class PlayGamesAuthentication : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void Start()
        {
            SignIn();
        }

        public void SignIn()
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                var userId = PlayGamesPlatform.Instance.GetUserId();
                var userName = PlayGamesPlatform.Instance.GetUserDisplayName();

                _text.text = $"Sign In Success\nWelcome {userName}";
            }
            else
            {
                _text.text = "Sign In Failed";
            }
        }
    }
}
