class Shoe //En sko: flera kortlekar blandade till en hög.
{
    private List<Card> cards = new();
    private readonly Random random;
    private readonly int numberOfDecks;

    public Shoe(Random random, int numberOfDecks)
    {
        if (numberOfDecks < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfDecks), "En sko måste ha minst en kortlek.");
        }

        this.random = random;
        this.numberOfDecks = numberOfDecks;
        Reset();
    }

    public void Reset() // Bygger en ny sko med alla lekar o blandar den.
    {
        cards = new List<Card>();

        for (int i = 0; i < numberOfDecks; i++)
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank in Enum.GetValues<Rank>())
                {
                    cards.Add(new Card(rank, suit));
                }         
            }
        }
        
        Shuffle();
    }

    public int Count()
    {
        return cards.Count;
    }

    private void Shuffle() // Fisher-Yates: gå baklänges och byt varje kort med slumpvalt kort på plats 0..i
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
            throw new InvalidOperationException("Skon är tom.");
        }

        Card card = cards[^1];
        cards.RemoveAt(cards.Count - 1);
        return card;
    }
}