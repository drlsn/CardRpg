using CardRPG.Entities.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class StartRandomGameCommandHandler
    {
        public static Game Game { get; private set; }

        public async Task Handle(StartRandomBotGameCommand cmd)
        {
            if (Game is not null)
                return;

            Game = new Game(_players);
        }

        public readonly static Player Player = new(new List<Card>() { DefaultCards.Zawisza });
        public readonly static Player Bot = new(new List<Card>() { DefaultCards.Ulryk });
        private static List<Player> _players = new()
        {
            Player,
            Bot
        };
    }

    public class StartRandomBotGameCommand
    {
    }
}
