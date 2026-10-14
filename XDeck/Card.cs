using System.Security.Cryptography.X509Certificates;

class Card
{
    public Rank Rank;
    public Suit Suit;

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
        throw new NotImplementedException();
    }
}
