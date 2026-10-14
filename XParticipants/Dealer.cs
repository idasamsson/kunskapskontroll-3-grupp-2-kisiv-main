class Dealer : Participant
{
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
    // Köper på 16 eller lägre, stannar på 17
    public override Move MakeMove()
    {
        throw new NotImplementedException();
    }
}
