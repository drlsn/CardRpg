namespace CardGame.Entities;

public class Player
{
    public PlayerId Id { get; }
    public Deck HandDeck { get; }

    public Player(PlayerId id, Deck handDeck)
    {
        Id = id;
        HandDeck = handDeck;
    }

    public Result<Card> TakeNextCard()
    {
        return HandDeck.TakeNextCard();
    }

    public void AddCard(Card card)
    {
        HandDeck.Add(card);
    }
}

public record PlayerId(string Value);
