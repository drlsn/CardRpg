using Assets.Scripts.CardRPG.UseCases.Games;
using CardRPG.UI.Features.Gameplay;
using CardRPG.UI.GUICommands;
using CardRPG.UI.UseCases;
using Core.Auth;
using Core.Collections;
using Core.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CardRPG.UI.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private IAuthentication _authentication;
        [Inject] private IHttpClientAccessor _clientAccessor;

        private GameEventsService _gameEventsService;
        private CancellationTokenSource _gameEventsServiceCts;

        private IGameEventsDispatcher _gameEventsDispatcher;

        private async void Start()
        {
            var accessTokenResult = await _authentication.GetAccessToken();
            _gameEventsService = new(_clientAccessor, accessTokenResult.Value);

            _gameEventsServiceCts = new CancellationTokenSource();
            Task.Run(async () => await _gameEventsService.Do(_gameEventsServiceCts.Token));

            await Play();
        }

        private void Update()
        {
            if (_gameEventsService.Messages.Count > 0)
            {
                if (_gameEventsService.Messages.TryDequeue(out var dataStr))
                {
                    var eventStr = dataStr.RemoveStartSubstring("CardsTakenToHandOutEvent", out bool ok);
                    if (ok)
                    {
                        var ev = JsonConvert.DeserializeObject<CardsTakenToHandOutEvent>(eventStr);
                        Debug.Log(ev);
                    }
                }
            }
        }

        private async Task Play()
        {
            Application.targetFrameRate = 60;
            await GameObject.FindObjectOfType<StartGameGUICommand>().Execute();
        }

        private void OnDestroy()
        {
            _gameEventsServiceCts.Cancel();
        }
    }
}
