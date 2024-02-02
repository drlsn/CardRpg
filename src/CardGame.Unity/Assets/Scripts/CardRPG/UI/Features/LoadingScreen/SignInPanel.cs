using Core.Auth;
using Core.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class SignInPanel : MonoBehaviour
    {
        [SerializeField] private EmailSignInPanel EmailSignInPanel;
        [SerializeField] private GameObject IdpSignInPanel;
        [SerializeField] private TMP_Text _msgText;

        [Inject] private IAuthentication _authentication;

        private async void Awake()
        {
            var token = await _authentication.GetAccessToken();
            if (!token.IsNullOrEmpty())
            {
                EmailSignInPanel.gameObject.SetActive(false);
                IdpSignInPanel.SetActive(false);

                var result = await _authentication.SignIn();
                if (result.IsSuccess)
                {
                    _msgText.text = "Sign In Success";
                    SceneManager.LoadScene("Menu");
                }
                else
                    _msgText.text = result.Message;

                return;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            EmailSignInPanel.gameObject.SetActive(true);
            IdpSignInPanel.SetActive(false);
#elif UNITY_ANDROID
            EmailSignInPanel.gameObject.SetActive(false);
            IdpSignInPanel.SetActive(true);
#endif
        }
    }
}
