class Game
{
    private const int MaxPlayers = 5;

    private readonly Random random = new Random();
    private Deck deck;
    private Dealer dealer;
    private List<Player> players;
    private List<Player> knownPlayers;
    private ConsoleUI ui;
    private GameStorage storage;

    public Game()
    {
        deck = new Deck(random);
        dealer = new Dealer();
        players = new List<Player>();
        knownPlayers = new List<Player>();
        ui = new ConsoleUI();
        storage = new GameStorage();
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
                running = false;
                break;
            }
        }
    }

    private void NewGame()
    {
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

    private void GameLoop()
    {
    }

    private void PlayRound()
    {
    }

    private Outcome DetermineOutcome(Player p)
    {
        throw new NotImplementedException();
    }
}
