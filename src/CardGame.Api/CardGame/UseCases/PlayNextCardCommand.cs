using CardGame.Entities;
using Mediator;

namespace CardGame.UseCases;

public class PlayNextCardCommandHandler : ICommandHandler<PlayNextCardCommand, Result>
{
    private readonly IGameRepository _gameRepository;

    public PlayNextCardCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async ValueTask<Result> Handle(PlayNextCardCommand command, CancellationToken cancellationToken)
    {
        var game = _gameRepository.Get(new PlayerId(command.PlayerId));
        if (game is null)
            return Result.Failure($"Could not find the game of player {command.PlayerId}");

        return game.PlayNextCard(new PlayerId(command.PlayerId));
    }
}

public record PlayNextCardCommand(string PlayerId) : ICommand<Result>;
