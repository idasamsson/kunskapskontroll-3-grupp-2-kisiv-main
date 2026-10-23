using XConsoleUI;

class ConsoleUI
{
    private readonly CardRenderer cardRenderer = new CardRenderer();
    public int ShowStartMenu()
    {
        Console.WriteLine("Välkommen till KISIV Blackjack!");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Visa regler");
        Console.WriteLine("3. Visa resultat");
        Console.WriteLine("4. Kör tester");
        Console.WriteLine("5. Avsluta");
        return ConsoleInput.ReadInt("Välj ett alternativ (1-5): ", 1, 5);
    }

    // Frågar vilket bord spelaren vill sitta vid. Varje bord är ett regelpaket.
    public int AskTable(List<string> tableNames)
    {
        Console.WriteLine();
        Console.WriteLine("Välj bord:");

        for (int i = 0; i < tableNames.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tableNames[i]}");
        }

        return ConsoleInput.ReadInt($"Välj (1-{tableNames.Count}): ", 1, tableNames.Count);
    }

    public int AskPlayerCount(int max = 5)
    {
        return ConsoleInput.ReadInt($"Hur många spelare (1-{max})? ", 1, max);
    }

    public void ShowMessage(string text)
    {
        Console.WriteLine(text);
    }
    public void ShowRules(bool dealerHitsSoft17, bool naturalBeatsThreeCard21)
    {
        Console.WriteLine();
        Console.WriteLine("--- Regler ---");
        Console.WriteLine("Målet är att komma så nära 21 som möjligt utan att gå över,");
        Console.WriteLine("och ha en högre hand än givaren.");
        Console.WriteLine("Knekt, dam och kung är värda 10. Ess är värt 1 eller 11.");
        Console.WriteLine("Du kan köpa ett kort till eller stanna.");
        Console.WriteLine("Går du över 21 har du spräckt och förlorar direkt.");
        Console.WriteLine(dealerHitsSoft17
            ? "Givaren köper på 16 eller mindre och på mjuk 17, men stannar på hård 17 eller mer."
            : "Givaren köper på 16 eller mindre och stannar på 17 eller mer, även mjuk 17.");
        Console.WriteLine("Ess och ett tiokort på de två första korten är naturlig tjugoett.");
        Console.WriteLine(naturalBeatsThreeCard21
            ? "Naturlig tjugoett slår 21 av tre kort eller fler."
            : "Naturlig tjugoett mot 21 av tre kort eller fler blir lika.");

        Console.WriteLine("Lika värde betyder lika, och rundan räknas varken som vinst eller förlust.");
        Console.WriteLine("---------------");
        Console.WriteLine();
    }

    // Visar spelarna sorterade efter marker. Som standard de fem främsta.
    public void ShowResults(List<Player> players, int maxRows = 5)
    {
        Console.WriteLine();
        Console.WriteLine($"--- Resultat (topp {maxRows} efter marker) ---");

        if (players.Count == 0)
        {
            Console.WriteLine("Inga resultat än.");
            return;
        }

        Console.WriteLine($"{"#",3} {"Spelare",-20} {"Marker",8} {"Vunna",6} {"Förlorade",10} {"Lika",6}");

        var ranked = players
            .OrderByDescending(p => p.Chips)
            .ThenByDescending(p => p.Wins)
            .ThenBy(p => p.Name)
            .Take(maxRows)
            .ToList();

        for (int i = 0; i < ranked.Count; i++)
        {
            Player p = ranked[i];
            Console.WriteLine($"{i + 1,3} {p.Name,-20} {p.Chips,8} {p.Wins,6} {p.Losses,10} {p.Pushes,6}");
        }
    }

    public string AskPlayerName()
    {
        while (true)
        {
            string name = ConsoleInput.ReadLine("Ange spelarnamn: ");

            if (name.Length == 0)
            {
                Console.WriteLine("Namnet får inte vara tomt.");
                continue;
            }

            if (name.Length > 20)
            {
                Console.WriteLine("Namnet får inte vara längre än 20 tecken.");
                continue;
            }

            // GameStorage sparar spelare som namn;vinster;förluster;lika;marker
            if (name.Contains(';'))
            {
                Console.WriteLine("Namnet får inte innehålla semikolon (;).");
                continue;
            }

            return name;
        }
    }

    // Frågar vem som ska sitta på en plats vid bordet
    public int AskPlayerType(int seat)
    {
        Console.WriteLine();
        Console.WriteLine($"Plats {seat}:");
        Console.WriteLine("1. Människa");
        Console.WriteLine("2. Dator (enkel strategi)");
        Console.WriteLine("3. Dator (grundstrategi)");
        Console.WriteLine("4. Dator (försiktig)");
        Console.WriteLine("5. Dator (djärv)");
        return ConsoleInput.ReadInt("Välj (1-5): ", 1, 5);
    }

    public int AskBet(Player player)
    {
        // Ska inte kunna hända: spelare utan marker tas bort av RemoveBrokePlayers.
        if (player.Chips <= 0)
        {
            throw new InvalidOperationException($"{player.Name} har {player.Chips} marker och kan inte satsa. " +
            "AskBet förutsätter att spelaren har minst 1 marker. Kontrollera RemoveBrokePlayers.");
        }
        return ConsoleInput.ReadInt($"{player.Name}, hur mycket vill du satsa (1 - {player.Chips}): ", 1, player.Chips);
    }

    public bool AskInsurance(Player player)
    {
        return ConsoleInput.ReadInt($"{player.Name}, vill du försäkra dig för {player.InsuranceAmount} marker? Tryck 1 för 'ja' och 2 för 'nej': ", 1, 2) == 1;
    }
    public Move AskMove()
    {
        while (true)
        {
            string input = ConsoleInput.ReadLine("Nytt kort (N) eller stanna (S)? ").ToUpper();

            if (input == "N")
            {
                return Move.Hit;
            }
            else if (input == "S")
            {
                return Move.Stand;
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Skriv 'N' för nytt kort eller 'S' för att stanna.");
            }

        }

    }

    public bool AskPlayAgain()
    {
        while (true)
        {
            string input = ConsoleInput.ReadLine("Spela en runda till? (J/N) ").ToUpper();

            if (input == "J")
            {
                return true;
            }
            if (input == "N")
            {
                return false;
            }

            Console.WriteLine("Ogiltigt val. Skriv 'J' för ja eller 'N' för nej.");
        }

    }

    public void ShowHand(Participant p)
    {
        // Givarens andra kort är dolt tills det är givarens tur
        bool holeCardHidden = p is Dealer { IsHoleCardRevealed: false };
        string value = holeCardHidden ? "?" : p.Hand.GetHandValue().ToString();

        Console.WriteLine();
        Console.WriteLine($"{p.Name} (värde: {value})");

        var cards = p.Hand.Cards.ToList();
        var blocks = cardRenderer.RenderCards(cards, holeCardHidden);
        int perRow = Math.Max(1, (GetWidth() + CardRenderer.CardGap) / (CardRenderer.Width + CardRenderer.CardGap));

        for (int start = 0; start < blocks.Count; start += perRow)
        {
            int end = Math.Min(start + perRow, blocks.Count);

            for (int line = 0; line < CardRenderer.Height; line++)
            {
                for (int i = start; i < end; i++)
                {
                    bool hidden = holeCardHidden && i == 1;
                    Console.ForegroundColor = ColorFor(cards[i], hidden);
                    Console.Write(blocks[i][line]);
                    Console.ResetColor();
                    Console.Write(new string(' ', CardRenderer.CardGap));
                }
                Console.WriteLine();
            }
        }
    }

    private static ConsoleColor ColorFor(Card card, bool hidden)
    {
        if (hidden)
        {
            return ConsoleColor.DarkCyan;
        }

        bool red = card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds;
        return red ? ConsoleColor.Red : ConsoleColor.Gray;
    }

    public void ShowPile(int cardsLeft)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        foreach (string line in cardRenderer.RenderPile(cardsLeft))
        {
            Console.WriteLine(line);
        }

        Console.ResetColor();
        Console.WriteLine($"Kort kvar: {cardsLeft}");
    }

    private static int GetWidth()
    {
        try
        {
            return Console.WindowWidth;
        }
        catch (IOException)
        {
            return 80; // utdata är omdirigerad
        }
    }

    public void ShowOutcome(Player p, Outcome o)
    {
        string text = o switch
        {
            Outcome.Win => "vinner!",
            Outcome.Lose => "förlorar.",
            Outcome.Push => "får lika, ingen vinner eller förlorar.",
            Outcome.Blackjack => "har naturlig tjugoett!",
            _ => "har ett okänt resultat."
        };

        Console.WriteLine($"{p.Name} {text}");
    }

    // Visar Hi-Lo räkningen och hur gynnsamt läget är för spelaren.
    public void ShowCount(int runningCount, double trueCount)
    {
        string indicator = trueCount switch
        {
            >= 2 => "gynnsamt för spelaren",
            <= -2 => "ogynnsamt för spelaren",
            _ => "neutralt"
        };

        Console.WriteLine($"Korträkning: löpande {runningCount}, sann {trueCount:F1} ({indicator})");
    }
}
