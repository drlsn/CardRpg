namespace CardGame.Entities;

public interface IGameRepository
{
    Game Get(GameId id);
    Game Get(PlayerId id);
    bool Add(Game game, PlayerId player);
    bool Add(PlayerId player, GameId game);
    bool Add(PlayerId player, out Game game);
    bool Remove(GameId id);
}
