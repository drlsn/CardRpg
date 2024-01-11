using System;

namespace CardGame.Entities.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void PlayNextCard()
    {
        var cards1 = Enumerable.Range(1, 3).Select(i => new Card() { Points = i });
        var deck1 = new Deck(cards1);
        var player1Id = new PlayerId("player-1");
        var player1 = new Player(player1Id, deck1);
        
        var cards2 = Enumerable.Range(2, 3).Select(i => new Card() { Points = i });
        var deck2 = new Deck(cards2);
        var player2Id = new PlayerId("player-2");
        var player2 = new Player(player2Id, deck1);

        var game = new Game(new[] { player1, player2 });

        Assert.IsTrue(game.PlayNextCard(player1Id).IsSuccess);
        Assert.IsTrue(game.PlayNextCard(player2Id).IsSuccess);
        Assert.IsTrue(game.PlayNextCard(player1Id).IsSuccess);
        Assert.IsTrue(game.PlayNextCard(player2Id).IsSuccess);
        Assert.IsTrue(game.PlayNextCard(player1Id).IsSuccess);
        Assert.IsTrue(game.IsGameOver());

        Assert.Pass();
    }

    [Test]
    public void Test()
    {
        var str = "Sup Dawg!";
        if (s is not ['S' or 's', .. { Length: 9 or 8 } ])
        {
            Assert.Fail();
        }

        void Change(string text) => text[0] = "K";
    }
}
