namespace CardGame.Logic;

public class Game
{
    public GameId Id { get; }
    private Player[] _players { get; }
    private Deck _trashDeck { get; } = new();
    private Dictionary<PlayerId, Card> _floorCards { get; } = new();

    public Game(Player[] players)
    {
        Id = new GameId(Guid.NewGuid().ToString());
        _players = players;
    }

    public Result PlayNextCard(PlayerId playerId)
    {
        if (_floorCards.ContainsKey(playerId))
            return Result.Failure();

        var player = _players.FirstOrDefault(player => player.Id == playerId);
        var result = player.TakeNextCard();
        if (!result.IsSuccess)
            return result;

        var card = result.Value;
        _floorCards.Add(playerId, card);

        if (_floorCards.Count == _players.Length)
        {
            var cardsByPoints = _floorCards.OrderByDescending(card => card.Value.Points);
            var firstCard = cardsByPoints.First();
            
            // Trash loosers cards
            var deadCards = cardsByPoints.TakeWhile(card => card.Value == firstCard.Value);
            foreach (var deadCard in deadCards)
                _trashDeck.Add(deadCard.Value);

            // Return winners Cards
            var winCards = cardsByPoints.Skip(deadCards.Count());
            foreach (var winCard in winCards)
            {
                var currentPlayer = _players.FirstOrDefault(player => player.Id == winCard.Key);
                currentPlayer.AddCard(winCard.Value);
            }

            _floorCards.Clear();
        }

        return result;
    }

    public bool IsGameOver() =>
        _players.Any(player => player.HandDeck.Cards.Count == 0);
}

public record GameId(string Value);
