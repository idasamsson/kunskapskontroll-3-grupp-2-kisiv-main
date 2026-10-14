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
            
            if (name.Length > 0 && name.Length > 20)
            {
                Console.WriteLine("Namnet får inte vara längre än 20 tecken. Försök igen.");
                continue;
            }

            if (name.Length > 0 && name.Length <= 20)
            {
                return name;
            }

            Console.WriteLine("Namnet får inte vara tomt eller längre än 20 tecken.");
        

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
            //Console.WriteLine("Välj ett drag:");
            //Console.WriteLine("1. Hit");
            //Console.WriteLine("2. Stand");
            //int choice = ConsoleInput.ReadInt("Ange ditt val (1-2): ", 1, 2);

            //if (choice == 1)
            //{
            //    return Move.Hit;
            //}
            //else if (choice == 2)
            //{
            //    return Move.Stand;
            //}
        }
    }

    public void ShowHand(Participant p)
    {
    }

    public void ShowOutcome(Player p, Outcome o)
    {
    }
}
