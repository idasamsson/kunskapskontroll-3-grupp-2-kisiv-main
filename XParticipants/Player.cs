class Player : Participant
{
    
    private IMoveChooser? moveChooser;
    private Card? dealerCard; 
    public int Wins;
    public int Losses;
    public int Pushes;

    public void ReceiveMoveChooser(IMoveChooser moveChooser)
    {
        this.moveChooser = moveChooser;
    }

    public void ReceiveDealerCard(Card dealerOpenCard)
    {
        this.dealerCard = dealerOpenCard;
    }

    public override Move MakeMove()
    { 
        if (dealerCard == null)
        {
            throw new InvalidOperationException("Givaren måste ha ett synligt kort."); 
        }
        if (moveChooser == null)
        {
            throw new InvalidOperationException("Spelaren har ingen beslutsfattare. Skicka in en IMoveChooser när Player skapas."); 
        }

       return moveChooser.ChooseMove(Hand, dealerCard);
    }
}

