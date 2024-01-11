using CardGame.Entities;
using Mediator;

namespace CardGame.UseCases;

public class CreateGameCommandHandler : ICommandHandler<CreateGameCommand, Result>
{
    private readonly IGameRepository _gameRepository;

    public CreateGameCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async ValueTask<Result> Handle(CreateGameCommand command, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid().ToString().Take(8);
        var gameId = new GameId($"game-{guid}");

        var playerId = new PlayerId(command.PlayerId);
        var deck = new Deck(Enumerable.Range(12, 12).Select(i => new Card { Points = i }).ToArray());
        var player = new Player(playerId, deck);

        var game = new Game(gameId, player);

        if (!_gameRepository.Add(game, playerId))
            return Result.Failure($"Could not add a game.");

        return Result.Success();
    }
}

public record CreateGameCommand(string PlayerId) : ICommand<Result>;
