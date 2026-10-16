class HumanMoveChooser : IMoveChooser
{
    private ConsoleUI ui;

    public HumanMoveChooser(ConsoleUI ui)
    {
        this.ui = ui;
    }

     public Move ChooseMove(Hand myHand, Card dealerCard )
    {
        return ui.AskMove();
    }
}