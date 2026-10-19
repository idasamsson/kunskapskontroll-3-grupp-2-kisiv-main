// Korträknare enligt Hi-Lo. Observerar bara kort som har visats
// och ändrar aldrig något i spelet.
class HiLoCounter
{
    private const int CardsPerDeck = 52;
    public int RunningCount { get; private set; }

    // Räknar in ett kort som har visats på bordet.
    public void Observe(Card card)
    {
        RunningCount += GetCountValue(card);
    }

    // 2-6 ger +1, 7-9 ger 0, tiokort och ess ger -1.
    private static int GetCountValue(Card card)
    {
        return card.Rank switch
        {
            Rank.Two or Rank.Three or Rank.Four or Rank.Five or Rank.Six => 1,
            Rank.Seven or Rank.Eight or Rank.Nine => 0,
        _   => -1 // Tio, knekt, dam, kung och ess
        };
    }

     // Löpande räkning delad med antal kvarvarnade lekar.
    public double TrueCount(int cardsRemaining)
    {
        if (cardsRemaining <= 0)
        {
            return RunningCount;
        }

        double decksRemaining = (double)cardsRemaining / CardsPerDeck;
        return RunningCount / decksRemaining;
    }
    // Börja om fron noll. Anropas när skon blandas om.
    public void Reset()
    {
        RunningCount = 0;
    }
}