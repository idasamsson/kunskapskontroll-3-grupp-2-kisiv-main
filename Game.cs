class Game
{
    private Deck deck;
    private Dealer dealer;
    private List<Player> players;
    private ConsoleUI ui;
    private GameStorage storage;

    public Game(Random random, ConsoleUI ui, GameStorage storage)
    {
        this.deck = new Deck(random);
        this.dealer = new Dealer { Name = "Dealer", Hand = new Hand() };
        this.players = new List<Player>();
        this.ui = ui;
        this.storage = storage;
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
