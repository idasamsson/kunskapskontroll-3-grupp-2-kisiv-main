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
    }

    public void ShowResults(List<Player> players)
    {
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
    }

    public void ShowOutcome(Player p, Outcome o)
    {
    }
}
