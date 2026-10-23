class Dealer : Participant
{   
    // Stannar på 17 eller mer. Köper på mjuk 17 om husregeln säger det.
    private const int StandValue = 17;
    private readonly bool hitsSoft17;

    // Hålkortet är givarens andra kort, ligger dolt tills alla spelare är klara.
    public bool IsHoleCardRevealed { get; private set; }

    // Givarens öppna kort är det första kortet som delas ut till givaren.
    // Finns först efter utdelningen.
    public Card UpCard => Hand.Cards[0];

    public Dealer(bool hitsSoft17 = false)
    {
        Name = "Givaren";
        this.hitsSoft17 = hitsSoft17;
    }
    // Vänder hålkortet. Anropas när alla spelare är klara.
    public void RevealHoleCard()
    {
        IsHoleCardRevealed = true;
    }
    // Köper på 16 eller lägre, stannar på 17 eller mer utom mjuk 17 om husregeln säger det.
    // Givaren väljer aldrig, draget räknas ut från regeln.
    public override Move MakeMove()
    {
        int value = Hand.GetHandValue();
        if (value < StandValue)
        {
            return Move.Hit;
        }
        if (value == StandValue && hitsSoft17 && Hand.IsSoft())
        {
            return Move.Hit;
        }
        return Move.Stand;
    }

    // Givarens hela tur, vänder hålkortet och drar kort
    // tills regeln säger stanna. Givaren spelar alltid färdigt.
    // Returnerar korten som drogs.
    public List<Card> PlayTurn(ICardSource deck)
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
