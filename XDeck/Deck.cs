class Deck // En kortlek med 52 kort som kan blandas och dras ifrån.
{
    private List<Card> cards = new();
    private readonly Random random;

    public Deck(Random random) // Samma Random skickas in från Game, så att alla delar samma slumpgenerator
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
    
    public void ResetIfLow(int threshold) // Ny blandad lek när färre än threshold kort är kvar.
                            // Anropas från Round i början av varje giv.
    {
        if (cards.Count < threshold)
        {
            Reset();
        }
    }
}
