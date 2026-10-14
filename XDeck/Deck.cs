class Deck
{
    private List<Card> cards = [];
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
            int j = random.Next(i + 1); 
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    public Card Draw()
    {
        if (cards.Count == 0)
        {
            Reset();
        }
        Card card = cards[^1]; // Tar sista kortet, eftersom det går 
                                // snabbt att ta bort från slutet av en List
        cards.RemoveAt(cards.Count - 1);
        return card;
    }

    public void Reset()
    {
        cards = [];

        foreach (Suit suit in Enum.GetValues<Suit>())
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                cards.Add(new Card(rank, suit));
            }
        }

         Shuffle();
    }
   
    public int Count()
    {
        return cards.Count;
    }
    // Ny blandad lek när färre än 15 kort är kvar.
// Anropas från Game i början av varje runda.
public void ResetIfLow()
    {
        if (cards.Count < 15)
        {
            Reset();
        }
    }
}
