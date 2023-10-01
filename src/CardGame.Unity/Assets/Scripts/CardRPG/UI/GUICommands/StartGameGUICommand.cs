using CardRPG.UseCases;
using System.Threading.Tasks;
using UnityEngine;
using static CardRPG.UI.Gameplay.CardRpgIOs;

namespace CardRPG.UI.GUICommands
{
    public class StartGameGUICommand : MonoBehaviour
    {
        [SerializeField] private BoardIO _boardIO;

        public async Task Execute()
        {
            await new StartRandomGameCommandHandler().Handle(new StartRandomBotGameCommand());
            var dto = await new GetGameStateQueryHandler().Handle(new GetGameStateQuery());

            var board = _boardIO.Instantiate();

            board.Init(dto);
        }
    }
}
