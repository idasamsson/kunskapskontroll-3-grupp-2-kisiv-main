class Player : Participant
{
    private IMoveChooser? moveChooser;
    private Card? dealerOpenCard;
    public int Wins;
    public int Losses;
    public int Pushes;

    public const int StartingChips = 200;
    public int Chips = StartingChips;
    public int Bet = 0;
    public int InsuranceBet = 0;
    public int InsuranceAmount => Bet / 2;

    public PlayerType Type = PlayerType.Human;

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

    public bool PlaceInsuranceBet()
    {
        if (InsuranceAmount <= 0 || InsuranceAmount > Chips || InsuranceBet > 0)
        {
            return false;
        }
        InsuranceBet = InsuranceAmount;
        Chips -= InsuranceBet;
        return true;
    }

    public void ReceivePayout(int payout)
    {
        Chips += payout;
        Bet = 0;
    }

    public void ReceiveInsurancePayout(int payout)
    {
        Chips += payout;
        InsuranceBet = 0;
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
