using System.Threading.Channels;

class Round
{
    private const int InitialCardCount = 2;
    private const int BlackjackValue = 21;
    private readonly Deck deck;
    private readonly Dealer dealer;
    private readonly List<Player> players;
    private readonly RuleSet rules;

    public Round(Deck deck, Dealer dealer, List<Player> players, RuleSet rules)
    {
        this.deck = deck;
        this.dealer = dealer;
        this.players = players;
        this.rules = rules;
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

        Card dealerOpenCard = dealer.Hand.Cards[0];

        foreach (Player player in players)
        {
            player.ReceiveDealerCard(dealerOpenCard);
        }
    }

    public bool CanHit(Player player)
    {
        return player.Hand.GetHandValue() < BlackjackValue;
    }

    public void Hit(Player player)
    {
        player.Hand.AddCard(deck.Draw());
    }

    public void PlayDealerTurn()
    {
        dealer.PlayTurn(deck);
    }

    public Outcome Settle(Player player)
    {
        Outcome outcome = DetermineOutcome(player);
        RecordOutcome(player, outcome);
        return outcome;
    }

    private Outcome DetermineOutcome(Player player)
    {
        if (player.Hand.IsBust())
        {
            return Outcome.Lose;
        }

        bool playerBlackjack = player.Hand.IsBlackjack();
        bool dealerBlackjack = dealer.Hand.IsBlackjack();

        if (playerBlackjack && dealerBlackjack)
        {
            return Outcome.Push;
        }
        if (playerBlackjack)
        {
            if (!rules.NaturalBeatsThreeCard21 && dealer.Hand.GetHandValue() == BlackjackValue)
            {
                return Outcome.Push;
            }
            return Outcome.Blackjack;
        }
        if (dealerBlackjack)
        {
            if (!rules.NaturalBeatsThreeCard21 && player.Hand.GetHandValue() == BlackjackValue)
            {
                return Outcome.Push;
            }
            return Outcome.Lose;
        }

        if (dealer.Hand.IsBust())
        {
            return Outcome.Win;
        }

        int playerValue = player.Hand.GetHandValue();
        int dealerValue = dealer.Hand.GetHandValue();

        if (playerValue > dealerValue)
        {
            return Outcome.Win;
        }
        if (playerValue < dealerValue)
        {
            return Outcome.Lose;
        }
        return Outcome.Push;
    }

    private void RecordOutcome(Player player, Outcome outcome)
    {
        switch (outcome)
        {
            case Outcome.Win:
            case Outcome.Blackjack:
                player.Wins++;
                break;
            case Outcome.Lose:
                player.Losses++;
                break;
            case Outcome.Push:
                player.Pushes++;
                break;
        }
    }
}