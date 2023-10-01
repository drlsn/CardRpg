using CardGame.Logic;
using System.Collections.Concurrent;

namespace CardGame.UseCases;

public class PlayNextCardCommand
{

}

public class GameHub
{
    private ConcurrentDictionary<GameId, Game> _games = new();

    public void Add(Game game)
    {
        _games.TryAdd(game.Id, game);
    }

    public bool Remove(GameId id)
    {
        return _games.Remove(id, out var game);
    }
}
