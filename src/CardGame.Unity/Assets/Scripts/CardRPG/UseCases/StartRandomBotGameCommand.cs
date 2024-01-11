using CardRPG.Entities.Gameplay;
using Core.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class StartRandomGameCommandHandler
    {
        public static Game Game { get; set; }

        public async Task Handle(StartRandomBotGameCommand cmd)
        {
            var cardsShuffled = DefaultCards.All.Shuffle().ToList();
            var playerCards = cardsShuffled.TakeHalf(first: true).ToList();
            var botCards = cardsShuffled.TakeHalf(first: false).ToList();

            var player = new Player("Player", playerCards);
            var bot = new Player("Bot", botCards);

            Game = new Game(new() { player, bot });
        }
    }

    public class StartRandomBotGameCommand
    {
    }
}
