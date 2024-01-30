using Core.Auth;
using Core.Collections;
using Core.Net.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            var signInResult = await _authentication.SignIn();
            _text.text = signInResult.IsSuccess ? "Sign In Success" : "Sign In Failed";
            if (signInResult.IsSuccess)
            {
                var authCode = await _authentication.GetAuthCode();
                try
                {
                    var requestBody = new { Code = authCode };
                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    var response = await PostJsonAsync(_clientAccessor, "/api/v1/token", jsonBody);
                    response.EnsureSuccessStatusCode();

                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenPostResponse>(jsonResult);
                    _text.text = $"Token {tokenResponse.AccessToken.Take(10).ToStr()}";
                }
                catch (HttpRequestException ex)
                {
                    _text.text = $"Access Token Error";
                }
            }
        }

        static async Task<HttpResponseMessage> PostJsonAsync(IHttpClientAccessor clientAccessor, string apiUrl, string jsonBody) =>
            await clientAccessor.Get("public").PostAsync(apiUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

        class TokenPostResponse
        {
            public string AccessToken { get; init; }
        }
    }
}
