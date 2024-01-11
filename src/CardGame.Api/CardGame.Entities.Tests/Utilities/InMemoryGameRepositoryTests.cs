using CardGame.Entities;
using CardGame.UseCases;

namespace CardGame.Tests.Utilities;

internal class InMemoryGameRepositoryTests
{
    [Test]
    public void AddGame()
    {
        var repository = new InMemoryGameRepository();

        var game = new Game(new Player[] { });
        var playerId = new PlayerId("player");
        
        Assert.IsTrue(repository.Add(game, playerId));

    }

    [Test]
    public void AddPlayer()
    {
        var repository = new InMemoryGameRepository();

        var game = new Game(new Player[] { });
        var playerId = new PlayerId("player-1");

        Assert.IsTrue(repository.Add(game, playerId));
        Assert.IsTrue(repository.Add(new PlayerId("player-2"), game.Id));
    }
}
