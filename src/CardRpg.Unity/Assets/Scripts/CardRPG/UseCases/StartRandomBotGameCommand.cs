using CardRPG.Entities.Gameplay;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class StartRandomGameCommandHandler
    {
        public static Game Game { get; set; }

        public async Task Handle(StartRandomBotGameCommand cmd)
        {
            if (Game is not null)
                return;

            Game = new Game(_players);
        }

        private static Player Player => new("Player", new List<Card>() { DefaultCards.Zawisza.DeepCopy() });
        private static Player Bot => new("Bot", new List<Card>() { DefaultCards.Ulryk.DeepCopy() });
        private static List<Player> _players => new()
        {
            Player,
            Bot
        };
    }

    public class StartRandomBotGameCommand
    {
    }
}
