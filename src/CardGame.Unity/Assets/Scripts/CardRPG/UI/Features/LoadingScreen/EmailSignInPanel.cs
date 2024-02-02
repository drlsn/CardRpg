using Core.Auth;
using Core.Unity.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class EmailSignInPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private Button _signInButton;

        [Inject] private IAuthentication _authentication;

        private void Start()
        {
            var emailAuth = _authentication as FirebaseEmailAuthentication;
            
            _signInButton.onClick.AddListener(async () => {
                emailAuth.Email = _emailInput.text;
                emailAuth.Password = _password.text;

                await _authentication.SignIn();
            });
        }
    }
}
