class Player : Participant
{
    private IMoveChooser? moveChooser;
    private Card? dealerOpenCard;
    public int Wins;
    public int Losses;
    public int Pushes;
    public int Chips = 200;
    public int Bet = 0;

    public bool PlaceBet(int bet)
    {
        if (bet <= 0 || bet > Chips)
        {
            return false;
        }

        Bet = bet;
        Chips -= bet;
        return true;
    }
    public void ReceivePayout(int payout)
    {
        Chips += payout;
        Bet = 0;
    }

    public void ReceiveMoveChooser(IMoveChooser moveChooser)
    {
        this.moveChooser = moveChooser;
    }

    public void ReceiveDealerCard(Card dealerOpenCard)
    {
        this.dealerOpenCard = dealerOpenCard;
    }


    public override Move MakeMove()
    {
        if (dealerOpenCard == null)
        {
            throw new InvalidOperationException("Givarens öppna kort saknas. ReceiveDealerCard måste anropas med det kortet innan MakeMove.");
        }
        if (moveChooser == null)
        {
            throw new InvalidOperationException("Spelaren har ingen beslutsfattare. ReceiveMoveChooser måste anropas innan MakeMove.");
        }

        return moveChooser.ChooseMove(Hand, dealerOpenCard);
    }
}
