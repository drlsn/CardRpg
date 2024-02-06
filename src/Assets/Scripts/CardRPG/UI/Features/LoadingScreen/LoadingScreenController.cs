using UnityEngine;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private SignInPanel _signInPanel;

        private async void Awake()
        {
            await _signInPanel.Reauthenticate();
        }
    }
}
