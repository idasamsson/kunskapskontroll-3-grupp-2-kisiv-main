class Game
{
    private const int MaxPlayers = 5;
    private ICardSource deck;
    private HiLoCounter? counter;
    private Dealer dealer;
    private List<Player> players;
    private List<Player> knownPlayers;
    private ConsoleUI ui;
    private GameStorage storage;
    private readonly RuleSet rules;

    public Game(Random random, ConsoleUI ui, GameStorage storage, RuleSet rules)
    {
        this.ui = ui;
        this.storage = storage;
        this.rules = rules;
        
        // En lek ger grundversionen med tröskel. Fler lekar ger en sko med skiljekort.
        if (rules.NumberOfDecks > 1)
        {
            Shoe shoe = new Shoe(random, rules.NumberOfDecks);
            counter = new HiLoCounter();
            shoe.Reshuffled += counter.Reset;
            deck = shoe;
        }
        else
        {
            deck = new Deck(random);
        }
        dealer = new Dealer(rules.DealerHitsSoft17);
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

        player.ReceiveMoveChooser(new HumanMoveChooser(ui));

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
        Round round = new Round(deck, dealer, players, rules);
        round.Deal();

        foreach (Player player in players)
        {
            PlayPlayerTurn(player, round);
        }

        round.PlayDealerTurn();
        ui.ShowHand(dealer);

        foreach (Player player in players)
        {
           Outcome outcome = round.Settle(player);
           ui.ShowOutcome(player, outcome);
        }
    }

    private void PlayPlayerTurn(Player player, Round round)
    {
        ui.ShowHand(dealer);
        ui.ShowHand(player);

        while (round.CanHit(player) && player.MakeMove() == Move.Hit)
        {
            round.Hit(player);
            ui.ShowHand(player);
        }
    }
}