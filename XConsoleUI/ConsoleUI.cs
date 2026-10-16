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

    public int AskPlayerCount()
    {
        return ConsoleInput.ReadInt("Hur många spelare (1-5)? ", 1, 5);
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

            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            Console.WriteLine("Ogiltigt namn. Försök igen.");
        }
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

    public void ShowHand(Participant p)
    {
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
