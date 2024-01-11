namespace CardGame.Entities;

public class Deck
{
    public Stack<Card> Cards { get; }

    public Deck()
    {
        Cards = new();
    }

    public Deck(IEnumerable<Card> cards)
    {
        Cards = new Stack<Card>(cards);
    }

    public Result<Card> TakeNextCard()
    {
        if (!Cards.TryPop(out var card))
            return Result<Card>.Failure();

        return Result<Card>.Success(card);
    }

    public void Add(Card card)
    {
        Cards.Push(card);
    }
}
