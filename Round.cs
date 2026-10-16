class Round
{
    private const int InitialCardCount = 2;
    private readonly Deck deck;
    private readonly Dealer dealer;
    private readonly List<Player> players;

    public Round(Deck deck, Dealer dealer, List<Player> players)
    {
        this.deck = deck;
        this.dealer = dealer;
        this.players = players;
    }

    public void Deal()
    {
        deck.ResetIfLow();
        foreach (Player player in players)
        {
            player.Hand.Clear();
        }
        dealer.StartNewRound();

        for (int i = 0; i < InitialCardCount; i++)
        {
            foreach (Player player in players)
            {
                player.Hand.AddCard(deck.Draw());
            }
            dealer.Hand.AddCard(deck.Draw());
        }
    }
}