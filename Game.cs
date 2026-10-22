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
                    ui.ShowRules(rules.DealerHitsSoft17);
                    break;
                case 3:
                    ui.ShowResults(knownPlayers.OrderByDescending(p => p.Chips).ToList());
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

        int count = ui.AskPlayerCount(MaxPlayers);

        while (players.Count < count)
        {
            int type = ui.AskPlayerType(players.Count + 1);

            if (type == 1)
            {
                AddHumanPlayer();
            }
            else
            {
                AddComputerPlayer(type);
            }
        }
    }

    // Frågar efter namn tills det är ett namn som inte redan sitter vid bordet.
    private void AddHumanPlayer()
    {
        while (true)
        {
            string name = ui.AskPlayerName();

            if (players.Exists(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                ui.ShowMessage("Den spelaren sitter redan vid bordet. Välj ett annat namn.");
            }
            else
            {
                AddPlayer(name);
                return;
            }
        }
    }

    // Skapar en datorspelare och ger en strategi. 
    // Datorspelare sparas inte till fil, läggs bara till vid bordet.
    private void AddComputerPlayer(int type)
    {
        IMoveChooser strategy;
        string strategyName;

        switch (type)
        {
            case 2:
                strategy = new SimpleStrategy();
                strategyName = "enkel";
                break;
            case 3:
                strategy = new BasicStrategy();
                strategyName = "grundstrategi";
                break;
            case 4:
                strategy = new CautiousStrategy();
                strategyName = "försiktig";
                break;
            default:
                strategy = new BoldStrategy();
                strategyName = "djärv";
                break;
        }


        Player computer = new Player { Name = $"Dator {players.Count + 1} ({strategyName})" };
        computer.Type = PlayerType.Computer;
        computer.ReceiveMoveChooser(strategy);
        players.Add(computer);
    }

    private void AddPlayer(string name)
    {
        Player? player = knownPlayers.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (player == null)
        {
            player = new Player { Name = name };
            knownPlayers.Add(player);
        }

        if (player.Chips == 0)
        {
            player.Chips = Player.StartingChips;
            ui.ShowMessage($"{player.Name} har fått nya startmarker.");
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
            RemoveBrokePlayers();
            SaveResults();
            if (players.Count == 0)
            {
                ui.ShowMessage("Det finns inga spelare kvar.");
                playing = false;
            }
            else
            {
                playing = ui.AskPlayAgain();
            }
        }
    }

    private void PlayRound()
    {
        Round round = new Round(deck, dealer, players, rules);
        AskForBets();
        round.Deal();
        ui.ShowPile(deck.Count());
        OfferInsurance();

        if (counter != null)
        {
            ui.ShowCount(counter.RunningCount, counter.TrueCount(deck.Count()));
        }

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

        ObserveTable(round);
    }

    // Datorer satsar alltid samma belopp.
    private const int ComputerBetSize = 10;

    private void AskForBets()
    {
        foreach (Player player in players)
        {
            int bet;

            if (player.Type == PlayerType.Computer)
            {
                bet = GetComputerBet(player);
                ui.ShowMessage($"{player.Name} satsar {bet}.");
            }
            else
            {
                bet = ui.AskBet(player);
            }

            // Ska inte kunna hända: ReadInt och Math.Min håller insatsen mellan 1 och spelarens marker.
            // Händer det ändå har ett antagande ändrats, till exempel att spelare utan marker tas bort.
            if (!player.PlaceBet(bet))
            {
                throw new InvalidOperationException(
                    $"{player.Name} kunde inte satsa {bet} med {player.Chips} marker. " +
                    "AskForBets förutsätter att insatsen är giltig. Kontrollera RemoveBrokePlayers och hur insatsen tas fram.");
            }
        }
    }

    // Datorn satsar ett fast belopp, men aldrig mer än den har.
    // Har den 6 marker kvar blir Math.Min(10, 6) = 6.
    private int GetComputerBet(Player player)
    {
        return Math.Min(ComputerBetSize, player.Chips);
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

        // Visa hur datorns tur slutade, eftersom ingen människa svarade.
        if (player.Type == PlayerType.Computer && !player.Hand.IsBust())
        {
            ui.ShowMessage($"{player.Name} stannar på {player.Hand.GetHandValue()}.");
        }
    }


    private void OfferInsurance()
    {
        if (dealer.Hand.Cards[0].Rank != Rank.Ace)
        {
            return;
        }

        ui.ShowHand(dealer);

        foreach (Player player in players)
        {
            if (player.Type == PlayerType.Human && player.InsuranceAmount > 0 && player.InsuranceAmount <= player.Chips)
            {
                if (ui.AskInsurance(player))
                {
                    player.PlaceInsuranceBet();
                }
            }
        }
    }

    // Visar räknaren alla kort på bordet när rundan är slut.
    // Då är även hålkortet vänt, så räknaren ser bara sådant som spelaren också har sett.
    private void ObserveTable(Round round)
    {
        if (counter == null)
        {
            return;
        }

        foreach (Card card in round.CardsOnTable())
        {
            counter.Observe(card);
        }
    }
    private void RemoveBrokePlayers()
    {
        foreach (Player player in players)
        {
            if (player.Chips == 0)
            {
                ui.ShowMessage($"{player.Name} har inga marker kvar och lämnar bordet.");
            }
        }
        players.RemoveAll(p => p.Chips == 0);
    }

}