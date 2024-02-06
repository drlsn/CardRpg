using CardRPG.UI.GUICommands;
using Core.Auth;
using Core.Collections;
using Core.Unity.Auth;
using Core.Unity.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class EmailSignInPanel : MonoBehaviour
    {
        private const string EmailKey = "email";
        private const string PasswordKey = "password";

        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private Button _signInButton;
        [SerializeField] private Button _loadStoredCredentialsButton;
        [SerializeField] private TMP_Text _msgText;

        [Inject] private IAuthentication _authentication;

        public void Init()
        {
            var emailAuth = (_authentication as CustomServerAuthentication).ProviderAuthentication as FirebaseEmailAuthentication;
            
            _signInButton.onClick.AddListener(async () => {
                emailAuth.Email = _emailInput.text;
                emailAuth.Password = _passwordInput.text;

                var result = await _authentication.SignIn();
                if (result.IsSuccess)
                {
                    SecurePlayerPrefs.SetString(EmailKey, emailAuth.Email);
                    SecurePlayerPrefs.SetString(PasswordKey, emailAuth.Password);

                    _msgText.text = "Sign In Success";
                    if (!await GameObject.FindObjectOfType<GoToMenuGUICommand>().Execute())
                        _msgText.text = "Sign In Failed";
                }
                else
                    _msgText.text = result.Message;
            });

            _loadStoredCredentialsButton.onClick.AddListener(() =>
            {
                var email = SecurePlayerPrefs.GetString(EmailKey);
                var password = SecurePlayerPrefs.GetString(PasswordKey);
                if (email.IsNullOrEmpty() || password.IsNullOrEmpty())
                    return;

                _emailInput.text = email;
                _passwordInput.text = password;
            });
        }
    }
}
