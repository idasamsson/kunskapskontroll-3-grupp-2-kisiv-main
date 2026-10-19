// Enkel datorstrategi: köper så länge handen är under 17
// Bryr sig inte om givarens kort.
class SimpleStrategy : IMoveChooser
{
    private const int StandValue = 17;

    public Move ChooseMove(Hand myHand, Card dealerCard)
    {
        if (myHand.GetHandValue() < StandValue)
        {
            return Move.Hit;
        }
        return Move.Stand;
    }
}