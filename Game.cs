class Game
{
    private const int MaxPlayers = 5;
    private const int InitialCardCount = 2;
    private readonly Random random;
    private Deck deck;
    private Dealer dealer;
    private List<Player> players;
    private List<Player> knownPlayers;
    private ConsoleUI ui;
    private GameStorage storage;

    public Game(Random random, ConsoleUI ui, GameStorage storage)
    {
        this.random = random;
        this.ui = ui;
        this.storage = storage;
        deck = new Deck(random);
        dealer = new Dealer();
        players = new List<Player>();
        knownPlayers = new List<Player>();
    }

    public void Run()
    {
        knownPlayers = storage.LoadPlayers();
        bool running = true;

        while (running)
        {
            int choice = ui.ShowStartMenu();

            switch (choice)
            {
                case 1:
                    NewGame();
                    GameLoop();
                    break;
                case 2:
                    ui.ShowRules();
                    break;
                case 3:
                    ui.ShowResults(knownPlayers);
                    break;
                case 4:
                    SaveResults();
                    running = false;
                    break;
            }
        }
    }

    private void NewGame()
    {
        players.Clear();

        int count = ui.AskPlayerCount();

        for (int i = 0; i < count; i++)
        {
            string name = ui.AskPlayerName();
            AddPlayer(name);
        }
    }

    private void AddPlayer(string name)
    {
        Player? player = knownPlayers.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            player = new Player { Name = name };
            knownPlayers.Add(player);
        }

        players.Add(player);
    }

    private void SaveResults()
    {
        try
        {
            storage.SavePlayers(knownPlayers);
        }
        catch (IOException)
        {
            ui.ShowMessage("Kunde inte spara resultaten.");
        }
        catch (UnauthorizedAccessException)
        {
            ui.ShowMessage("Saknar behörighet att spara resultaten");
        }
    }

    private void GameLoop()
    {
    }

    private void PlayRound()
    {
        deck.ResetIfLow();
        DealInitialCards();

        foreach (Player player in players)
        {
            PlayPlayerTurn(player);
        }

        PlayDealerTurn();

        foreach (Player player in players)
        {
            Outcome outcome = DetermineOutcome(player);
            RecordOutcome(player, outcome);
            ui.ShowOutcome(player, outcome);
        }
    }

    private void PlayPlayerTurn(Player player)
    {
        ui.ShowHand(dealer);
        ui.ShowHand(player);

        while (!player.Hand.IsBust() && player.Hand.GetHandValue() < 21 && ui.AskMove() == Move.Hit)
        {
            player.Hand.AddCard(deck.Draw());
            ui.ShowHand(player);
        }
    }

    private void PlayDealerTurn()
    {
        dealer.PlayTurn(deck);
        ui.ShowHand(dealer);
    }

    private void DealInitialCards()
    {
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
            return Outcome.Blackjack;
        }
        if (dealerBlackjack)
        {
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
