class Deck
{
    private List<Card> cards;
    private readonly Random random;

    // Samma Random skickas in från Game, så att alla delar samma insats
    public Deck(Random random)
    {
        this.random = random;
        Reset();
    }

    // Fisher-Yates: gå baklänges o byt med ett slumpvalt kort 0..i
    public void Shuffle()
    {
         for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1); // 0 till i, inklusive i
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }


    public Card Draw()
    {
        if (cards.Count == 0)
        {
            Reset();
        }
        Card card = cards[^1];
        cards.RemoveAt(cards.Count - 1);
        return card;
    }

    public void Reset()
    {
    }

    public int Count()
    {
        throw new NotImplementedException();
    }
}
