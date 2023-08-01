using CardRPG.Entities.Gameplay;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class GetGameStateQueryHandler
    {
        public async Task<GetGameStateQueryOut> Handle(GetGameStateQuery query)
        {
            return new(StartRandomGameCommandHandler.Game);
        }
    }

    public class GetGameStateQuery
    {
    }

    public class GetGameStateQueryOut
    {
        public GetGameStateQueryOut(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
    }
}
