class GameStorage
{
    private const char Separator = ';';
    private readonly string filePath;

    public GameStorage(string filePath = "players.txt")
    {
        this.filePath = filePath;
    }

    public void SavePlayers(List<Player> players)
    {
        List<string> lines = new List<string>();
        
        foreach (Player player in players)
        {
            string line = string.Join(Separator, player.Name, player.Wins, player.Losses, player.Pushes);
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines);
    }

    public List<Player> LoadPlayers()
    {
        throw new NotImplementedException();
    }
}
