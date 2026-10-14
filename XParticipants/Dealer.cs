class Dealer : Participant
{
    // Givaren stannar på 17 eller mer, även mjuk 17
    private const int StandValue = 17;

    // Hålkortet är givarens andra kort, ligger dolt tills alla spelare är klara.
    public bool IsHoleCardRevealed { get; private set; }

    public Dealer()
    {
        Name = "Givaren";
        Hand = new Hand();
    }
    // Vänder hålkortet. Anropas när alla spelare är klara.
    public void RevealHoleCard()
    {
        IsHoleCardRevealed = true;
    }
    // Köper på 16 eller lägre, stannar på 17.
    // Givaren väljer aldirg, draget räknas ut från regeln.
    public override Move MakeMove()
    {
        if (HandGetValue() < StandValue)
        {
            return Move.Hit;
        }
        return Move.Stand;
    }
    // Gör givaren redo för en ny runda, tom hand och dolt hålkort.
    public void StartNewRound()
    {
        Hand.Clear();
        IsHoleCardRevealed = false;
    }
}
