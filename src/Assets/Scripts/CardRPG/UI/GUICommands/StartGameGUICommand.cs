using CardRPG.UI.Features.IOs;
using CardRPG.UI.Gameplay;
using CardRPG.UseCases;
using System.Threading.Tasks;
using UnityEngine;

namespace CardRPG.UI.GUICommands
{
    public class StartGameGUICommand : MonoBehaviour
    {
        [SerializeField] private BoardIO _boardIO;

        public async Task Execute()
        {
            await new StartRandomGameCommandHandler().Handle(new StartRandomBotGameCommand());
            var dto = await new GetGameStateQueryHandler().Handle(new GetGameStateQuery());

            _boardIO.Instantiate();
            var board = _boardIO.PrefabData.Object.GetComponent<Board>();

            await board.Init(dto);
        }
    }
}
