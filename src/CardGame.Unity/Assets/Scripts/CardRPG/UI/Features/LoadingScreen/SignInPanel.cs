using Core.Auth;
using Core.Collections;
using UnityEngine;
using Zenject;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class SignInPanel : MonoBehaviour
    {
        [SerializeField] private EmailSignInPanel EmailSignInPanel;
        [SerializeField] private GameObject IdpSignInPanel;

        [Inject] private IAuthentication _authentication;

        private async void Awake()
        {
            var token = await _authentication.GetAccessToken();
            if (!token.IsNullOrEmpty())
            {
                EmailSignInPanel.gameObject.SetActive(false);
                IdpSignInPanel.SetActive(false);

                return;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            EmailSignInPanel.gameObject.SetActive(true);
            IdpSignInPanel.SetActive(false);
#elif UNITY_ANDROID
            EmailSignInPanel.SetActive(false);
            IdpSignInPanel.SetActive(true);
#endif
        }
    }
}
