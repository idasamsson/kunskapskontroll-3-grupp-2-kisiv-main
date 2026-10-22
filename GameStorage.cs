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
            string line = string.Join(Separator, player.Name, player.Wins, player.Losses, player.Pushes, player.Chips);
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines);
    }

    public List<Player> LoadPlayers()
    {
        List<Player> players = new List<Player>();

        if (!File.Exists(filePath))
        {
            return players;
        }

        foreach (string line in File.ReadAllLines(filePath))
        {
            Player? player = ParseLine(line);

            if (player != null)
            {
                players.Add(player);
            }
        }

        return players;
    }

    private Player? ParseLine(string line)
    {
        int chips = Player.StartingChips;

        string[] parts = line.Split(Separator);

        if (parts.Length != 4 && parts.Length != 5)
        {
            return null;
        }

        string name = parts[0].Trim();

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        if (!int.TryParse(parts[1], out int wins) ||
            !int.TryParse(parts[2], out int losses) ||
            !int.TryParse(parts[3], out int pushes))
        {
            return null;
        }

        if (parts.Length == 5)
        {
            if (!int.TryParse(parts[4], out chips) || chips < 0)
            {
                return null;
            }
        }
        return new Player { Name = name, Wins = wins, Losses = losses, Pushes = pushes, Chips = chips};
    }
}
