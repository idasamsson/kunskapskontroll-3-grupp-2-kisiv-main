class RuleSetLoader
{
    private const char Separator = '=';
    private const int MinReshuffleThreshold = 15;
    private readonly string filePath;

    public List<string> Warnings { get; } = new List<string>();

    public RuleSetLoader(string filePath = "rules.txt")
    {
        this.filePath = filePath;
    }

    public RuleSet Load()
    {
        RuleSet rules = new RuleSet();

        if (!File.Exists(filePath))
        {
            Warnings.Add($"Hittade inte {filePath}. Spelar med standardregler.");
            return rules;
        }

        foreach (string line in File.ReadAllLines(filePath))
        {
            ReadRule(line, rules);
        }

        return rules;
    }

    private void ReadRule(string line, RuleSet rules)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        string[] parts = line.Split(Separator);

        if (parts.Length != 2)
        {
            Warnings.Add($"Ogiltig rad: \"{line}\"");
            return;
        }

        string key = parts[0].Trim();
        string value = parts[1].Trim();

        switch (key)
        {
            case "DealerHitsSoft17":
                if (bool.TryParse(value, out bool hitsSoft17))
                {
                    rules.DealerHitsSoft17 = hitsSoft17;
                }
                else
                {
                    AddInvalidValueWarning(key, value);
                }
                break;

            case "NumberOfDecks":
                if (int.TryParse(value, out int decks) && decks >= 1)
                {
                    rules.NumberOfDecks = decks;
                }
                else
                {
                    AddInvalidValueWarning(key, value);
                }
                break;

            case "ReshuffleThreshold":
                if (int.TryParse(value, out int threshold) && threshold >= MinReshuffleThreshold)
                {
                    rules.ReshuffleThreshold = threshold;
                }
                else
                {
                    AddInvalidValueWarning(key, value);
                }
                break;

            case "NaturalBeatsThreeCard21":
                if (bool.TryParse(value, out bool naturalWins))
                {
                    rules.NaturalBeatsThreeCard21 = naturalWins;
                }
                else
                {
                    AddInvalidValueWarning(key, value);
                }
                break;

            default:
                Warnings.Add($"Okänd regel: \"{key}\"");
                break;
        }
    }

    private void AddInvalidValueWarning(string key, string value)
    {
        Warnings.Add($"Ogiltigt värde för {key}: \"{value}\". Använder standardvärdet.");
    }
}