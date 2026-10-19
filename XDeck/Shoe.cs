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
    }

}