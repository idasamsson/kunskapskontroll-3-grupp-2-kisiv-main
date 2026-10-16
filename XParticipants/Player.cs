class Player : Participant
{
    /*
    private IMoveChooser moveChooser;

    public Player(IMoveChooser moveChooser)
    {
        this.moveChooser = moveChooser;
    }
    */
    public int Wins;
    public int Losses;
    public int Pushes;

    private Card? dealerCard; 

    public void ReceiveDealerCard(Card dealerCard)
    {
        this.dealerCard = dealerCard;
    }

    public override Move MakeMove()
    {
        //if ()
        {
           throw new InvalidOperationException(); 
        }
        
    }
}

