public class Card
{
    public Rank Rank {get;}  // Kortets valör (2–10, Knekt, Dam, Kung, Ess) kortet går inte att ändras utifrån.
    public Suit Suit {get;} // Kortets färg (Hjärter, Ruter, Klöver, Spader) kortet går inte att ändras utifrån

    public Card(Rank rank, Suit suit) // Konstruktor: skapar ett nytt kort med angiven valör och färg.
    {
        Rank = rank;
        Suit = suit;
    }
    public int GetValue() // Returnerar kortets poängvärde enligt Blackjack-reglerna.
    {
       return Rank switch 
       {
            Rank.Jack or Rank.Queen or Rank.King => 10, // Klädda kort är värda 10
            Rank.Ace => 11,                            // Ess räknas som 11 här (kan justeras till 1 i handen)
            _ => (int)Rank                            // 2-10 är värda sitt eget nummer
       };
    }

    public override string ToString() // Gör om kortet till läsbar svensk text, t.ex. "Spader Ess".
                                    // Anropas automatiskt när kortet skrivs ut, t.ex. med Console.WriteLine(card).
    {
        string suit = Suit switch // Översätt färgen till svenska.
        {
            Suit.Hearts => "Hjärter",
            Suit.Diamonds => "Ruter",
            Suit.Clubs => "Klöver",
            Suit.Spades => "Spader",
        _   => "?" // Reserv om ett okänt värde skulle dyka upp
        };

        string rank = Rank switch // Översätt valören till svenska.
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

