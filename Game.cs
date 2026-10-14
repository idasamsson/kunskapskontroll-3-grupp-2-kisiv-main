class Game
{
    private const int MaxPlayers = 5;

    private readonly Random random = new Random();
    private Deck deck;
    private Dealer dealer;
    private List<Player> players;
    private ConsoleUI ui;
    private GameStorage storage;

    public Game()
    {
        deck = new Deck(random);
        dealer = new Dealer();
        players = new List<Player>();
        ui = new ConsoleUI();
        storage = new GameStorage();
    }

    public void Run()
    {
    }

    private void ShowStartMenu()
    {
    }

    private void NewGame()
    {
    }

    private void AddPlayer(string name)
    {
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
