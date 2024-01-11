using CardGame.Entities;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace CardGame.UseCases;

public class InMemoryGameRepository : IGameRepository
{
    private ConcurrentDictionary<PlayerId, GameId> _playersGames = new();
    private ConcurrentDictionary<GameId, Game> _emptyGames = new();
    private ConcurrentDictionary<GameId, Game> _fullGames = new();

    public bool Add(Game game, PlayerId player)
    {
        if (_fullGames.ContainsKey(game.Id))
            return false;

        if (_playersGames.ContainsKey(player))
            return false;

        if (!_emptyGames.TryAdd(game.Id, game))
            return false;

        if (!_playersGames.TryAdd(player, game.Id))
            return false;

       return true;
    }

    public bool Add(PlayerId player, GameId game)
    {
        if (!_emptyGames.ContainsKey(game))
            return false;

        return _playersGames.TryAdd(player, game);
    }

    public bool Add(PlayerId player, out Game game)
    {
        var gameData = _emptyGames.FirstOrDefault();
        game = gameData.Value;
        if (game is null)
            return false;

        if (!_emptyGames.Remove(gameData.Key, out game))
            return false;

        if (!_fullGames.TryAdd(game.Id, game))
            return false;

        return _playersGames.TryAdd(player, game.Id);
    }

    public bool Remove(GameId id)
    {
        if (!_fullGames.Remove(id, out var game))
            if (!_emptyGames.Remove(id, out game))
                return false;

        foreach (var player in game.Players)
            _playersGames.Remove(player.Id, out var gameId);

       return true;
    }

    public Game Get(GameId id)
    {
        if (!_fullGames.TryGetValue(id, out var game))
            _emptyGames.TryGetValue(id, out game);

        return game;
    }

    public Game Get(PlayerId id)
    {
        if (!_playersGames.TryGetValue(id, out var gameId))
            return default;

        if (!_fullGames.TryGetValue(gameId, out var game))
            _emptyGames.TryGetValue(gameId, out game);

        return game;
    }
}
