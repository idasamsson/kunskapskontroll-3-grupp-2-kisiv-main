// Försiktig datorstrategi, stannar så fort handen är över 12 eller mer.
// Under 12 kan inget kort spräcka handen, så den tar aldrig den risken.
class CautiousStrategy : IMoveChooser
{
    private const int StandValue = 12;

    public Move ChooseMove(Hand myHand, Card dealerCard)
    {
        if (myHand.GetHandValue () < StandValue)
        {
            return Move.Hit;
        }
        return Move.Stand;
    }
}