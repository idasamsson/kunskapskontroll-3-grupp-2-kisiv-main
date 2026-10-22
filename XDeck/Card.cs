public class Card
{
    // Kortets valör (2–10, Knekt, Dam, Kung, Ess) kortet går inte att ändras utifrån.
    public Rank Rank {get;}

    // Kortets färg (Hjärter, Ruter, Klöver, Spader) kortet går inte att ändras utifrån
    public Suit Suit {get;}

    // Konstruktor: skapar ett nytt kort med angiven valör och färg.
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    // Returnerar kortets poängvärde enligt Blackjack-reglerna.
    public int GetValue()
    {
       return Rank switch 
       {
            Rank.Jack or Rank.Queen or Rank.King => 10, // Klädda kort är värda 10
            Rank.Ace => 11,                            // Ess räknas som 11 här (kan justeras till 1 i handen)
            _ => (int)Rank                            // 2-10 är värda sitt eget nummer
       };
    }

    // Gör om kortet till läsbar svensk text, t.ex. "Spader Ess". Anropas automatiskt när kortet skrivs ut, 
    // t.ex. med Console.WriteLine(card).
    public override string ToString()
                                    
    {
        string suit = Suit switch // Översätt färgen till svenska.
        {
            Suit.Hearts => "Hjärter",
            Suit.Diamonds => "Ruter",
            Suit.Clubs => "Klöver",
            Suit.Spades => "Spader",
        _   => "?" // Reserv om ett okänt värde skulle dyka upp
        };

        // Översätt valören till svenska.
        string rank = Rank switch
        {
            Rank.Jack => "Knekt",
            Rank.Queen => "Dam",
            Rank.King => "Kung",
            Rank.Ace => "Ess",
            _ => ((int)Rank).ToString() // 2-10 skrivs som siffror
        };

        return $"{suit} {rank}"; // Sätt ihop färg och valör, t.ex. "Hjärter 7".
    }
}

