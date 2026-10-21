using System.IO.Pipelines;

class Hand
{
    private List<Card> cards = new List<Card>();
    public IReadOnlyList<Card> Cards => cards;

    private const int BlackjackValue = 21;
    
    //“Ess räknas som 11, men kan räknas som 1. Skillnaden är 10.”
    private const int AceReduction = 10;
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
            while (total > BlackjackValue && aceCount > 0)
            {
                total -= AceReduction;
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
        return GetHandValue() == BlackjackValue && cards.Count == 2;
    }

    public bool IsBust()
    {
        return GetHandValue() > BlackjackValue;
    }

    public void Clear()
    {
        cards.Clear();
    }
}
