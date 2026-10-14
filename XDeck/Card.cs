public class Card
{
    public Rank Rank;  
    public Suit Suit; 
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }
    public int GetValue()
    {
       return Rank switch 
       {
            Rank.Jack or Rank.Queen or Rank.King => 10,
            Rank.Ace => 11,
            _ => (int)Rank // 2-10 är värda sitt eget nummer
       };
    }

    public override string ToString()
    {
        string suit = Suit switch
        {
        Suit.Hearts => "Hjärter",
        Suit.Diamonds => "Ruter",
        Suit.Clubs => "Klöver",
        Suit.Spades => "Spader",
        _ => "?"
        };

        string rank = Rank switch
        {
            Rank.Jack => "Knekt",
            Rank.Queen => "Dam",
            Rank.King => "Kung",
            Rank.Ace => "Ess",
            _ => ((int)Rank).ToString() // 2-10 skrivs som siffror
        };

        return $"{suit} {rank}";
    }
}
