// Djärv datorstrategi, köper ända tills handen är 19 eller mer.
// Chansar på att komma nära 21, även om det finns stor risk att spräcka.
class BoldStrategy : IMoveChooser
{
    private const int StandValue = 19;

    public Move ChooseMove(Hand myHand, Card dealerCard)
    {
        if (myHand.GetHandValue() < StandValue)
        {
            return Move.Hit;
        }
        return Move.Stand;
    }
}