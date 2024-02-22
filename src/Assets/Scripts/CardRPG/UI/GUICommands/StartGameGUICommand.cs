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
            var getCurrentGameQueryOut = await _gameplayService.Query<GetCurrentGameQuery, GetCurrentGameQueryOut>();

            await new StartRandomGameCommandHandler().Handle(new StartRandomBotGameCommand());
            var dto = await new GetGameStateQueryHandler().Handle(new GetGameStateQuery());

            _boardIO.Instantiate();
            var board = _boardIO.PrefabData.Object.GetComponent<Board>();

            await board.Init(dto);
        }
    }
}
