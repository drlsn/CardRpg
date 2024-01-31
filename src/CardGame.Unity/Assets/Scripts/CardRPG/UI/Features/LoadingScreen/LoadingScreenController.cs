using Core.Auth;
using Core.Collections;
using Core.Functional;
using Core.Net.Http;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace CardRPG.UI.Features.LoadingScreen
{
    public class LoadingScreenController : MonoBehaviour
    {
        [Inject] public IAuthentication _authentication { get; set; }
        [Inject] public IHttpClientAccessor _clientAccessor { get; set; }

        [SerializeField] private TMP_Text _text;

        public async void Load()
        {
            var accessToken = await _authentication.GetAccessToken();
            accessToken.IfNotNull(t =>
                _text.text = $"Token {accessToken.Take(10).ToStr()}");
        }
    }
}
