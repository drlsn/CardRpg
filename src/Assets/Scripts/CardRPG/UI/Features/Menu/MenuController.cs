using CardRPG.UI.Features.Gameplay;
using CardRPG.UseCases.Users;
using Core.Auth;
using Core.Collections;
using Core.Net.Http;
using PlasticPipe.PlasticProtocol.Messages;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CardRPG.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [Inject] private IAuthentication _authentication;
        [Inject] private IHttpClientAccessor _clientAccessor;

        private GetUserQueryResponse _getUserDTO;

        private BroadcastService _broadcastService;
        private async void Start()
        {
            var accessTokenResult = await _authentication.GetAccessToken();
            _broadcastService = new(_clientAccessor, accessTokenResult.Value);
            Task.Run(async () => await _broadcastService.Do());
            //await Init();
        }

        private void Update()
        {
            if (_broadcastService.Messages.Count > 0) 
            {
                if (_broadcastService.Messages.TryDequeue(out var result))
                {
                    Debug.Log(result);
                }
            }
        }

        public async Task Init()
        {
            var result = await _clientAccessor.GetAsync<GetUserQueryResponse>(ClientType.TrinicaAuthorized, "api/v1/users/me");
            if (!result.Response.IsSuccessStatusCode)
                SceneManager.LoadScene("LoadingScreen");

            _getUserDTO = result.Body;
            var tutorialDone = _getUserDTO.TutorialStep < 0 ? "yup" : "no";
            var text = $"User Id - {_getUserDTO.Id.Take(5).ToStr()}";
            text += $"\nTutorial done - {tutorialDone}";
            _text.text = text;
        }

        public void Play()
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
