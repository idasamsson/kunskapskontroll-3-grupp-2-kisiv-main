// En sko: flera kortlekar blandade till en hög.
class Shoe : ICardSource
{
    private List<Card> cards = new();
    private readonly Random random;
    private readonly int numberOfDecks;
    // Antal kort kvar när skiljekortet kommer fram.
    private int cutCardPosition;
    public bool CutCardReached { get; private set; }

    // Meddelar den som lyssnar (t.ex. en korträknare) att skon har blandats om.
    public event Action? Reshuffled;

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

    // Bygger en ny sko med alla lekar och blandar den.
    public void Reset()
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
        PlaceCutCard();
        Reshuffled?.Invoke();
    }

    public int Count()
    {
        return cards.Count;
    }

    // Fisher-Yates: gå baklänges och byt varje kort med slumpvalt kort på plats 0..i
    private void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    // Lägg skiljekortet på en slumpmässig plats i den bakre fjärdedelen av skon.
    private void PlaceCutCard()
    {
        int backQuarter = cards.Count / 4;
        cutCardPosition = random.Next(1, backQuarter + 1);
        CutCardReached = false;
    }

    public Card Draw()
    {
        // Nödfall: ska inte hända tack vare skiljekortet och tröskeln,
        // men hellre en ny sko än att spelet kraschar mitt i en runda.
        if (cards.Count == 0)
        {
            Reset();
        }

        Card card = cards[^1];
        cards.RemoveAt(cards.Count - 1);

        if (cards.Count <= cutCardPosition)
        {
            CutCardReached = true;
        }

        return card;
    }

    // Blandar om skon mellan rundorna:
    // - när skiljekortet har dragits (det vanliga), eller
    // - när färre än threshold kort är kvar (skydd så att skon inte tar slut mitt i en runda).
    public void ResetIfLow(int threshold)
    {
        if (CutCardReached || cards.Count < threshold)
        {
            Reset();
        }
    }
}