using CardRPG.Entities.Gameplay;
using CardRPG.Entities.Users;
using System.Linq;
using System.Threading.Tasks;

namespace CardRPG.UseCases
{
    public class GetGameStateQueryHandler
    {
        public async Task<GetGameStateQueryOut> Handle(GetGameStateQuery query)
        {
            return new(
                StartRandomGameCommandHandler.Game.Players.First().Id,
                StartRandomGameCommandHandler.Game);
        }
    }

    public class GetGameStateQuery
    {
    }

    public class GetGameStateQueryOut
    {
        public GetGameStateQueryOut(
            UserId playerId,
            Game game)
        {
            PlayerId = playerId;
            Game = game;
        }

        public UserId PlayerId { get; }
        public Game Game { get; }
    }
}
