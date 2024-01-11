using CardGame.Entities;
using Mediator;

namespace CardGame.UseCases;

public class GetGameQueryHandler : IQueryHandler<GetGameQuery, Result<GetGameQueryResponse>>
{
    private readonly IGameRepository _gameRepository;

    public GetGameQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async ValueTask<Result<GetGameQueryResponse>> Handle(GetGameQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetGameQueryResponse>.Success();

        var game = _gameRepository.Get(new PlayerId(query.PlayerId));
        if (game is null)
            return result.Fail($"Could not find the game of he player (id {query.PlayerId})");

        return result.With(new GetGameQueryResponse(
            game.Id.Value, 
            game.FloorCards.Select(cardInfo => new PlayerDTO(cardInfo.Key.Value, cardInfo.Value.Points)).ToArray()));
    }
}

public record GetGameQuery(string PlayerId) : IQuery<Result<GetGameQueryResponse>>;

public record GetGameQueryResponse(
    string GameId,
    PlayerDTO[] Players);

public record PlayerDTO(
    string Id,
    int FloorCardPoints);
