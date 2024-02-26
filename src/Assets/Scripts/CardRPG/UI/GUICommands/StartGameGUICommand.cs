using CardRPG.UI.Features.IOs;
using CardRPG.UI.Gameplay;
using CardRPG.UI.UseCases;
using CardRPG.UseCases;
using CardRPG.UseCases.Games;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CardRPG.UI.GUICommands
{
    public class StartGameGUICommand : MonoBehaviour
    {
        [SerializeField] private BoardIO _boardIO;
        [Inject] private IGameplayService _gameplayService;

        public async Task Execute()
        {
            var currentGame = await _gameplayService.Query<GetCurrentGameQuery, GetCurrentGameQueryOut>();
            if (currentGame.GameId is null)
            {
                if (!await _gameplayService.Send(new StartGameCommand()))
                    return;
            }

            var gameState = await _gameplayService.Query<GetGameStateQuery, GetGameStateQueryOut>(
                new GetGameStateQuery(currentGame.GameId));

            _boardIO.Instantiate();
            var board = _boardIO.PrefabData.Object.GetComponent<Board>();

            await board.Init(gameState, _gameplayService);
        }
    }
}
