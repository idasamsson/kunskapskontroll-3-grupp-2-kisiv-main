using System.IO.Pipelines;

class Hand
{
    private List<Card> cards = new List<Card>();
    public IReadOnlyList<Card> Cards => cards;


    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    private (int total, int aceCount) CalculateTotalAndAceCount()
    {
        int total = 0;
        int aceCount = 0;
        foreach (Card card in cards)
            {
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                }

                total += card.GetValue();

            }
            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }
        return (total, aceCount);
    }

        public int GetHandValue()
    {  
        var (total, _) = CalculateTotalAndAceCount();
        return total;
    }

    public bool IsSoft()
    {
        var (_, aceCount) = CalculateTotalAndAceCount();
        return aceCount > 0;
    }

    public bool IsBlackjack()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        cards.Clear();
    }
}
