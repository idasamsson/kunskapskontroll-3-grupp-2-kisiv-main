namespace XConsoleUI;

internal static class ConsoleInput
{
    // Skriver ut frågan, läser en rad och trimmar den.
    // Avslutar programmet snyggt om inmatningen är stängd (null),
    // annars skulle frågeloopar köra för evigt.
    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (input == null)
        {
            Environment.Exit(0);
        }

        return input.Trim();
    }

    // Frågar tills användaren skriver ett heltal mellan min och max.
    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            string input = ReadLine(prompt);
            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"Ogiltigt val. Skriv ett tal mellan {min} och {max}.");
        }
    }
}