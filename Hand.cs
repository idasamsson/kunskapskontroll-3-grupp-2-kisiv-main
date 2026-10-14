class Hand
{
    private List<Card> cards = new List<Card>();
    public IReadOnlyList<Card> Cards => cards;


    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public int GetHandValue()
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
        return total;
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
