class Dealer : Participant
{
    // Givaren stannar på 17 eller mer, även mjuk 17
    private const int StandValue = 17;

    // Hålkortet är givarens andra kort, ligger dolt tills alla spelare är klara.
    public bool IsHoleCardRevealed { get; private set; }

    public Dealer()
    {
        Name = "Givaren";
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

// Givarens hela tur, vänder hålkortet och drar kort
// tills regeln säger stanna. Givaren spelar alltid färdigt.
// Returnerar korten som drogs.
public List<Card> PlayTurn(Deck deck)
{
    RevealHoleCard();

    List<Card> drawnCards = new List<Card>();
    while (MakeMove() == Move.Hit)
    {
        Card card = deck.Draw();
        Hand.AddCard(card);
        drawnCards.Add(card);
    }
    return drawnCards;
}

// Ny runda. Tom hand och dolt hålkort.
public void StartNewRound()
   {
    Hand.Clear();
    IsHoleCardRevealed = false;
   }
}
