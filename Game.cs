class Game
{
    private const int MaxPlayers = 5;
    private Deck deck;
    private Dealer dealer;
    private List<Player> players;
    private List<Player> knownPlayers;
    private ConsoleUI ui;
    private GameStorage storage;

    public Game(Random random, ConsoleUI ui, GameStorage storage)
    {
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

        while (players.Count < count)
        {
            string name = ui.AskPlayerName();

            if (players.Exists(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                ui.ShowMessage("Den spelaren ssitter redan vid bordet. Välj ett annat namn.");
            }
            else
            {
                AddPlayer(name);
            }
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
        bool playing = true;

        while (playing)
        {
            PlayRound();
            SaveResults();
            playing = ui.AskPlayAgain();
        }
    }

    private void PlayRound()
    {
       Round round = new Round(deck, dealer, players);
       round.Deal();

        foreach (Player player in players)
        {
            PlayPlayerTurn(player, round);
        }

        round.PlayDealerTurn();
        ui.ShowHand(dealer);

        foreach (Player player in players)
        {
            Outcome outcome = DetermineOutcome(player);
            RecordOutcome(player, outcome);
            ui.ShowOutcome(player, outcome);
        }
    }

    private void PlayPlayerTurn(Player player, Round round)
    {
        ui.ShowHand(dealer);
        ui.ShowHand(player);

        while (round.CanHit(player) && ui.AskMove() == Move.Hit)
        {
            round.Hit(player);
            ui.ShowHand(player);
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
