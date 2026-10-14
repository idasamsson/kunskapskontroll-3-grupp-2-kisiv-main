class Hand
{
    private List<Card> cards = new List<Card>();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public int GetHandValue(List<int> cards)
    {
        int Total = 0;
        int Ace = 0;

        foreach (int card in cards)
        {
            if (card == 1)
            {
                Ace++; 
                Total += 11;
            }
            else
            {
                Total += card;
            }
        }   
        while (Total > 21 && Ace > 0)
            {
                Total -= 10;
                Ace--;
            }
        return Total; 
    }

    public bool IsBlackjack()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
    }
}
