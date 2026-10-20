using XConsoleUI;

class ConsoleUI
{
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
        Console.WriteLine($"{"Spelare",-20} {"Vunna",6} {"Förlorade",10} {"Lika",6}");

        foreach (Player p in players)
        {
            Console.WriteLine($"{p.Name,-20} {p.Wins,6} {p.Losses,10} {p.Pushes,6}");
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
    // 1 = människa, 2 = dator med enkel strategi, 3 = dator med grundstrategi
    public int AskPlayerType(int seat)
    {
        Console.WriteLine();
        Console.WriteLine($"Plats {seat}:");
        Console.WriteLine("1. Människa");
        Console.WriteLine("2. Dator (enkel strategi)");
        Console.WriteLine("3. Dator (grundstrategi)");
        return ConsoleInput.ReadInt("Välj (1-3): ", 1, 3);
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
        // The dealer's second card stays hidden until the dealer's turn
        bool holeCardHidden = p is Dealer { IsHoleCardRevealed: false };

        var cards = new List<string>();
        int index = 0;
        foreach (Card card in p.Hand.Cards)
        {
            bool isHoleCard = holeCardHidden && index == 1;
            cards.Add(isHoleCard ? "[dolt kort]" : card.ToString());
            index++;
        }

            string value = holeCardHidden ? "?" : p.Hand.GetHandValue().ToString();
            Console.WriteLine($"{p.Name}: {string.Join("  ", cards)}   (värde: {value})");
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
}
