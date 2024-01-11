namespace CardGame.Entities;

public class Game
{
    public GameId Id { get; }
    public List<Player> Players { get; private set; }
    private Deck _trashDeck { get; } = new();
    public Dictionary<PlayerId, Card> FloorCards { get; } = new();

    public Game(IEnumerable<Player> players)
    {
        Id = new GameId(Guid.NewGuid().ToString());
        Players = players.ToList();
    }

    public Game(GameId id, Player player)
    {
        Id = id;
        Players = new List<Player>() { player };
    }

    public bool JoinPlayer(Player player)
    {
        Players ??= new();
        if (Players.Any(p => p.Id == player.Id))
            return false;

        Players.Add(player);

        return true;
    }

    public Result PlayNextCard(PlayerId playerId)
    {
        if (FloorCards.ContainsKey(playerId))
            return Result.Failure();

        var player = Players.FirstOrDefault(player => player.Id == playerId);
        var result = player.TakeNextCard();
        if (!result.IsSuccess)
            return result.Fail("Could not take next card!");

        var card = result.Value;
        FloorCards.Add(playerId, card);

        if (FloorCards.Count == Players.Count)
        {
            var cardsByPoints = FloorCards.OrderByDescending(card => card.Value.Points);
            var firstCard = cardsByPoints.First();
            
            // Trash loosers cards
            var deadCards = cardsByPoints.TakeWhile(card => card.Value == firstCard.Value);
            foreach (var deadCard in deadCards)
                _trashDeck.Add(deadCard.Value);

            // Return winners Cards
            var winCards = cardsByPoints.Skip(deadCards.Count());
            foreach (var winCard in winCards)
            {
                var currentPlayer = Players.FirstOrDefault(player => player.Id == winCard.Key);
                currentPlayer.AddCard(winCard.Value);
            }

            FloorCards.Clear();
        }

        return result;
    }

    public bool IsGameOver() =>
        Players.Any(player => player.HandDeck.Cards.Count == 0);
}

public record GameId(string Value);
