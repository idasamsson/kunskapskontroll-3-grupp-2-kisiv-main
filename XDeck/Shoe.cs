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

    public void Reset() // Bygger en ny sko med alla lekar.
    {
        cards = new List<Card>();

        for (int i = 0; i < numberOfDecks; i++)
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
            {
                foreach (Rank rank  in Enum.GetValues<Rank>())
                {
                    cards.Add(new Card(rank, suit));
                }                
            }
        }
        
    }
    public int Count()
    {
        return cards.Count();
    }   
}