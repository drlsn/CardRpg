using CardGame.Entities;
using Mediator;

namespace CardGame.UseCases;

public class JoinGameCommandHandler : ICommandHandler<JoinGameCommand, Result>
{
    private readonly IGameRepository _gameRepository;

    public JoinGameCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async ValueTask<Result> Handle(JoinGameCommand command, CancellationToken cancellationToken)
    {
        var playerId = new PlayerId(command.PlayerId);
        var deck = new Deck(Enumerable.Range(0, 12).Select(i => new Card { Points = i }).ToArray());
        var player = new Player(playerId, deck);

        Game game = null;
        if (command.GameId is null)
        {
            if (!_gameRepository.Add(playerId, out game))
                return Result.Failure($"Could not find the game of id {command.GameId}");
        }
        else
        {
            game = _gameRepository.Get(new GameId(command.GameId));
            if (game is null)
                return Result.Failure($"Could not find the game of id {command.GameId}");

            if (!_gameRepository.Add(playerId, game.Id))
                return Result.Failure($"Could not join the player to the game of id {command.GameId}");
        }

        if (!game.JoinPlayer(player))
            return Result.Failure($"Could not join the player to the game of id {command.GameId}");

        return Result.Success();
    }
}

public record JoinGameCommand(string PlayerId, string? GameId = null) : ICommand<Result>;
