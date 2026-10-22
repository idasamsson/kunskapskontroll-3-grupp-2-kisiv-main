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
        Console.WriteLine("4. Avsluta");
        return ConsoleInput.ReadInt("Välj ett alternativ (1-4): ", 1, 4);
    }

    public int AskPlayerCount(int max = 5)
    {
        return ConsoleInput.ReadInt($"Hur många spelare (1-{max})? ", 1, max);
    }

    public void ShowMessage(string text)
    {
        Console.WriteLine(text);
    }
    public void ShowRules()
    {
        Console.WriteLine();
        Console.WriteLine("--- Regler ---");
        Console.WriteLine("Målet är att komma så nära 21 som möjligt utan att gå över,");
        Console.WriteLine("och ha en högre hand än givaren.");
        Console.WriteLine("Knekt, dam och kung är värda 10. Ess är värt 1 eller 11.");
        Console.WriteLine("Du kan köpa ett kort till eller stanna.");
        Console.WriteLine("Går du över 21 har du spräckt och förlorar direkt.");
        Console.WriteLine("Givaren köper tills handen är 17 eller mer, även mjuk 17.");
        Console.WriteLine("Ess och ett tiokort på de två första korten är naturlig tjugoett.");
        Console.WriteLine("Naturlig tjugoett slår 21 av tre kort eller fler.");
        Console.WriteLine("Lika värde betyder lika, och rundan räknas varken som vinst eller förlust.");
        Console.WriteLine("Vinnaren får 1 poäng, och den som först når 5 poäng vinner spelet.");
        Console.WriteLine("---------------");
        Console.WriteLine();
    }

    public void ShowResults(List<Player> players)
    {
        Console.WriteLine();
        Console.WriteLine("--- Resultat ---");
        Console.WriteLine($"{"Spelare",-20} {"Vunna",6} {"Förlorade",10} {"Lika",6} {"Marker",10}");

        foreach (Player p in players)
        {
            Console.WriteLine($"{p.Name,-20} {p.Wins,6} {p.Losses,10} {p.Pushes,6} {p.Chips,10}");
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

            // GameStorage sparar spelare som namn;vinster;förluster;lika
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
        return ConsoleInput.ReadInt($"{player.Name}, hur mycket vill du satsa (1 - {player.Chips}): ", 1, player.Chips);
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

    // Visar Hi-Lo- räkningen och hur gynnsamt läget är för spelaren.
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
